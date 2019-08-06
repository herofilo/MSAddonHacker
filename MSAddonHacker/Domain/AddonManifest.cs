using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MSAddonHacker.Persistence;
using SevenZip;

namespace MSAddonHacker.Domain
{
    public class AddonManifest
    {
        /// <summary>
        /// Path to the Manifest file
        /// </summary>
        public string ManifestFilePath { get; private set; }

        /// <summary>
        /// Path to the temporary folder
        /// </summary>
        public string TemporaryFolder { get; private set; }


        public string ManifestContentMirrorPath { get; private set; }

        /// <summary>
        /// List of asset files (relative path)
        /// </summary>
        public List<string> AssetList { get; private set; }

        // ------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pPath">Path to the Manifest file</param>
        /// <param name="pTempBasePath">Path to the temporary base folder</param>
        public AddonManifest(string pPath, string pTempBasePath)
        {
            ManifestFilePath = pPath?.Trim();
            TemporaryFolder = pTempBasePath?.Trim();

            string errorText;
            if (!CheckPaths(out errorText))
                return;

            AssetList = CreateAssetList(ManifestFilePath, TemporaryFolder, ManifestContentMirrorPath, out errorText);
        }




        private bool CheckPaths(out string pErrorText)
        {
            pErrorText = null;

            if (string.IsNullOrEmpty(ManifestFilePath) || !File.Exists(ManifestFilePath))
            {
                pErrorText = "Manifest file: invalid path or file not found";
                return false;
            }

            if (string.IsNullOrEmpty(TemporaryFolder) || !Directory.Exists(TemporaryFolder))
            {
                pErrorText = "Temporary folder: invalid path or folder not found";
                return false;
            }

            try
            {
                ManifestContentMirrorPath = Path.Combine(TemporaryFolder, "_AssetData");
                if (!Directory.Exists(ManifestContentMirrorPath))
                    Directory.CreateDirectory(ManifestContentMirrorPath);
            }
            catch (Exception exception)
            {
                pErrorText = $"CheckPaths(): {exception.Message}";
                return false;
            }

            return true;

        }


        // -------------------------------------------------------------------------------------------------------------------------


        private List<string> CreateAssetList(string pManifestFilePath, string pTemporaryFolder, string pManifestContentMirror, out string pErrorText)
        {
            pErrorText = null;
            string tempAssetZipFile = Path.Combine(pTemporaryFolder, "assetData.zip");
            SevenZipArchiver archiver = new SevenZipArchiver(tempAssetZipFile);

            List<string> fileList = null;

            try
            {
                File.Copy(pManifestFilePath, tempAssetZipFile, true);

                // Extracts contents of Manifest file to TemporaryFolder
                if (archiver.ArchivedFilesExtract(pManifestContentMirror, null) < 0)
                {
                    pErrorText = archiver.LastErrorText;
                    return null;
                }

                // Create List of files
                List<ArchiveFileInfo> archiveFilesInfo;
                archiver.ArchivedFileList(out archiveFilesInfo);
                if (archiveFilesInfo != null)
                {
                    fileList = new List<string>();
                    foreach (ArchiveFileInfo item in archiveFilesInfo)
                    {
                        string fileName = item.FileName;
                        if (fileName.StartsWith("Data", StringComparison.InvariantCultureIgnoreCase))
                            fileList.Add(fileName);
                    }
                }
            }
            catch (Exception exception)
            {
                pErrorText = exception.Message;
                return null;
            }
            finally
            {
                if (File.Exists(tempAssetZipFile))
                    File.Delete(tempAssetZipFile);
            }

            fileList?.Sort();
            return fileList;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Recreate Manifest file from mirror folder
        /// </summary>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        public bool RecreateManifestMirror(out string pErrorText)
        {
            pErrorText = null;
            AssetList = CreateAssetList(ManifestFilePath, TemporaryFolder, ManifestContentMirrorPath, out pErrorText);

            return (AssetList != null);
        }


        /// <summary>
        /// Recreate Manifest file from mirror folder
        /// </summary>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        public bool RefreshManifestFromMirror(out string pErrorText)
        {
            if (!CheckPaths(out pErrorText))
                return false;

            string tempAssetZipFile = Path.Combine(TemporaryFolder, "assetData.zip");
            SevenZipArchiver archiver = new SevenZipArchiver(tempAssetZipFile);

            try
            {

                if (!archiver.ArchiveFolder(ManifestContentMirrorPath))
                {
                    pErrorText = archiver.LastErrorText;
                    return false;
                }

                File.Copy(tempAssetZipFile, ManifestFilePath, true);
            }
            catch (Exception exception)
            {
                pErrorText = exception.Message;
                return false;
            }
            finally
            {
                if (File.Exists(tempAssetZipFile))
                    File.Delete(tempAssetZipFile);
            }

            // Recreate Asset List 
            AssetList = RecreateAssetList();

            return true;
        }




        private List<string> RecreateAssetList()
        {
            string prefix = ManifestContentMirrorPath + "\\";
            int prefixLen = prefix.Length;

            string mirrorPath = ManifestContentMirrorPath + "\\Data";
            List<string> fileList = new List<string>();

            foreach (string fileName in Directory.GetFiles(mirrorPath, "*", SearchOption.AllDirectories))
            {
                string fileRelative = fileName.Remove(0, prefixLen);
                fileList.Add(fileRelative);
            }
            fileList.Sort();
            return fileList;
        }
    }
}
