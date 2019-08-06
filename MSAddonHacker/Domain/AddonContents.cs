using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MSAddonHacker.Domain
{
    public class AddonContents
    {

        public const string ManifestAssetMaskRegexString = @"DESCRIPTOR|.*\.template|.*\.part|.*\.bodypart";

        public const string MeatFileMaskRegexString = @"DESCRIPTOR|.*\.template|.*\.part|.*\.bodypart|.*\.crf|.*\.cmf";

        /// <summary>
        /// Path to the addon Data folder
        /// </summary>
        public string AddonDataFolder { get; private set; }


        /// <summary>
        /// List of asset files (relative path)
        /// </summary>
        public List<string> AssetList { get; private set; }


        public List<string> MeatFileList { get; private set; }


        private bool _checkedOk = false;

        private Regex _mftAssetMaskRegex = null;

        private static Regex _meatFileMaskRegex = null;

        private static SHA256 _Sha256 = null;



        // -----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pPath">Path to the root data folder of the addon</param>
        public AddonContents(string pPath)
        {
            if (string.IsNullOrEmpty(pPath = pPath?.Trim()))
                return;

            if (!Directory.Exists(pPath))
                return;

            AssetList = _RefreshAssetList(pPath);


            AddonDataFolder = pPath;

            _checkedOk = true;
        }

        // ---------------------------------------------------------------------------------------------------------------------


        private List<string> _RefreshAssetList(string pPath)
        {
            string prefix = GetAddonDataFolderPrefix(pPath); // pPath.Remove(pPath.Length - "\\Data".Length + 1);
            int prefixLen = prefix.Length;
            _mftAssetMaskRegex = new Regex(ManifestAssetMaskRegexString, RegexOptions.IgnoreCase);
            GetMeatyFileMaskRegex();

            List<string> fileList = new List<string>();
            MeatFileList = new List<string>();
            foreach (string fileName in Directory.GetFiles(pPath, "*", SearchOption.AllDirectories))
            {
                if (fileName.StartsWith(prefix))
                {
                    string fileRelative = fileName.Remove(0, prefixLen);
                    string file = Path.GetFileName(fileRelative);
                    if (_mftAssetMaskRegex.IsMatch(file))
                        fileList.Add(fileRelative);
                    if (_meatFileMaskRegex.IsMatch(file))
                        MeatFileList.Add(fileRelative);
                }

            }


            MeatFileList.Sort();
            fileList.Sort();

            return fileList;
        }


        public List<string> GetAssetList()
        {
            if (!_checkedOk)
                return null;

            return MeatFileList;
        }


        public static Regex GetMeatyFileMaskRegex()
        {
            if (_meatFileMaskRegex != null)
                return _meatFileMaskRegex;
            _meatFileMaskRegex = new Regex(MeatFileMaskRegexString, RegexOptions.IgnoreCase);
            return _meatFileMaskRegex;
        }



        private List<string> RefreshAssetList()
        {
            if (!_checkedOk)
                return null;

            return _RefreshAssetList(AddonDataFolder);
        }



        private string GetAddonDataFolderPrefix(string pPath)
        {
            pPath = pPath?.Trim();
            if (string.IsNullOrEmpty(pPath))
                return null;
            if (!pPath.ToLower().EndsWith("\\data") || (pPath.Length < 6))
                return pPath;

            return pPath.Remove(pPath.Length - "\\Data".Length + 1);
        }



        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Copy a list of files from the Manifest file into the Data Folder Hierarchy
        /// </summary>
        /// <param name="pFileList">List of files to copy</param>
        /// <param name="pManifestManifestContentMirrorPath">Path of the mirror folder for the Manifest file</param>
        /// <param name="pNeedsRefresh">Display for Data Folder needs refreshing.</param>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        /// <remarks>CAVEAT: it will return pNeedsRefresh=true for partial lists</remarks>
        public bool CopyFilesFromManifestMirror(List<string> pFileList, string pManifestManifestContentMirrorPath, out bool pNeedsRefresh, out string pErrorText)
        {
            pNeedsRefresh = false;
            pErrorText = null;
            if (pFileList == null)
            {
                pErrorText = "List of files to copy is null is null";
                return false;
            }

            bool copyOk = true;
            if (pFileList.Count > 0)
            {
                if (!Directory.Exists(pManifestManifestContentMirrorPath))
                {
                    pErrorText = "Source path not found";
                    return false;
                }

                string prefix = GetAddonDataFolderPrefix(AddonDataFolder);
                foreach (string file in pFileList)
                {
                    try
                    {
                        string sourceFile = Path.Combine(pManifestManifestContentMirrorPath, file);
                        // FileInfo sourceFileInfo = new FileInfo(sourceFile);
                        if (!File.Exists(sourceFile))
                        {
                            pErrorText = $"File '{file}' not found";
                            break;
                        }
                        string destinationFile = Path.Combine(prefix, file);
                        FileInfo destFileInfo = new FileInfo(destinationFile);
                        if (destFileInfo.Exists)
                        {
                            string sourceHash = GetFileHash(sourceFile);
                            string destinationHash = GetFileHash(destinationFile);
                            if (sourceHash == destinationHash)
                                continue;
                            destFileInfo.IsReadOnly = false;
                        }
                        File.Copy(sourceFile, destinationFile, true);
                    }
                    catch (Exception exception)
                    {
                        pErrorText = $"EXCEPTION: {exception.Message}";
                        pNeedsRefresh = true;
                        copyOk = false;
                        break;
                    }
                }

            }

            pNeedsRefresh = CompareFileList(pFileList);

            if (pNeedsRefresh)
                RefreshAssetList();

            return copyOk;
        }


        private bool CompareFileList(List<string> pFileList)
        {
            if ((pFileList == null) && (MeatFileList != null) ||
                (pFileList != null) && (MeatFileList == null))
                return true;

            if (pFileList == null)
                return false;

            if (pFileList.Count != (MeatFileList?.Count ?? 0))
            {
                return true;
            }

            if (pFileList.Count == 0)
                return false;

            pFileList.Sort();
            for (int index = 0; index < pFileList.Count; ++index)
            {
                if (pFileList[index] != MeatFileList[index])
                    return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Copy a list of files from the Manifest file into the Data Folder Hierarchy
        /// </summary>
        /// <param name="pFileList">List of files to copy</param>
        /// <param name="pManifestManifestContentMirrorPath">Path of the mirror folder for the Manifest file</param>
        /// <param name="pManifestNeedsRefreshing">Manifest file needs recreating</param>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        public bool CopyFilesToManifestMirror(List<string> pFileList, string pManifestManifestContentMirrorPath, out bool pManifestNeedsRefreshing, out string pErrorText)
        {
            pManifestNeedsRefreshing = false;
            pErrorText = null;
            if (pFileList == null)
            {
                pErrorText = "List of files to copy is null is null";
                return false;
            }

            bool copyOk = true;
            if (pFileList.Count > 0)
            {
                if (!Directory.Exists(pManifestManifestContentMirrorPath))
                {
                    pErrorText = "Source path not found";
                    return false;
                }

                int filesCopied = 0;
                string prefix = GetAddonDataFolderPrefix(AddonDataFolder);
                foreach (string file in pFileList)
                {
                    try
                    {
                        string sourceFile = Path.Combine(prefix, file);
                        if (!File.Exists(sourceFile))
                        {
                            pErrorText = $"File '{file}' not found";
                            break;
                        }

                        string destinationFile = Path.Combine(pManifestManifestContentMirrorPath, file);
                        FileInfo destFileInfo = new FileInfo(destinationFile);
                        if (!destFileInfo.Exists)
                            continue;

                        string sourceHash = GetFileHash(sourceFile);
                        string destinationHash = GetFileHash(destinationFile);
                        if(sourceHash == destinationHash)
                            continue;

                        destFileInfo.IsReadOnly = false;
                        File.Copy(sourceFile, destinationFile, true);
                        filesCopied++;
                    }
                    catch (Exception exception)
                    {
                        pErrorText = $"EXCEPTION: {exception.Message}";
                        // pManifestNeedsRefreshing = true;
                        copyOk = false;
                        break;
                    }
                }
                pManifestNeedsRefreshing = (filesCopied > 0);
            }

            return copyOk;
        }
        

        // -----------------------------------------------------------------------------------------------------------------------


        private string GetFileHash(string pPath)
        {
            string result = "";
            if (!File.Exists(pPath))
                return result;

             if(_Sha256  == null)
                _Sha256 = SHA256.Create();

            byte[] hashBytes;
            using (FileStream stream = File.OpenRead(pPath))
            {
                hashBytes = _Sha256.ComputeHash(stream);
            }
            
            foreach (byte b in hashBytes)
                result += b.ToString("x2");

            return result;
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Remove meaty files from the Data folder
        /// </summary>
        /// <param name="pNeedsRefreshing">Needs refreshing display</param>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of the operation</returns>
        public bool RemoveMeatyFiles(out bool pNeedsRefreshing, out string pErrorText)
        {
            pNeedsRefreshing = false;
            pErrorText = null;

            if (!Directory.Exists(AddonDataFolder))
            {
                pErrorText = "Addon Data folder not found";
                return false;
            }

            if ((MeatFileList == null) || (MeatFileList.Count == 0))
                return true;

            bool isOk = false;
            bool deleteOpComplete = false;
            try
            {
                string prefix = GetAddonDataFolderPrefix(AddonDataFolder);
                foreach (string fileName in MeatFileList)
                {
                    string fullPath = Path.Combine(prefix, fileName);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        pNeedsRefreshing = true;
                    }
                }

                deleteOpComplete = true;
                AssetList = _RefreshAssetList(AddonDataFolder);
                isOk = true;
            }
            catch (Exception exception)
            {
                pErrorText = exception.Message;
            }
            finally
            {
                if(!deleteOpComplete)
                    AssetList = _RefreshAssetList(AddonDataFolder);
            }

            return isOk;
        }
    }
}
