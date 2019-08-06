using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MSAddonHacker.Persistence;

namespace MSAddonHacker.Domain
{
    public class Addon
    {

        public const string AddonSummaryFileName = ".Addon";

        public const string ManifestFileName = "assetData.jar";

        public const string DataFolderName = "Data";


        public string Name { get; private set; }

        public string AddonFolder { get; private set; } = null;

        public AddonManifest AddonManifest { get; private set; } = null;

        public AddonContents AddonContents { get; private set; } = null;

        private string _tempPath;


        // -----------------------------------------------------------------------------------------------------------


        public Addon(string pPath, string pTempPath)
        {
            pPath = pPath.ToLower().Trim();
            if (!string.IsNullOrEmpty(pPath))
            {
                AddonFolder = CheckAddonFolder(pPath);
            }

            if (AddonFolder == null)
                throw new Exception("No valid addon folder or file specified");

            Name = Path.GetFileName(AddonFolder);

            _tempPath = pTempPath;

            AddonManifest = new AddonManifest(Path.Combine(pPath, ManifestFileName), pTempPath);

            AddonContents = new AddonContents(Path.Combine(pPath, DataFolderName));

        }



        // -----------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Gets the Moviestorm addon root folder
        /// </summary>
        /// <returns>Moviestorm addon root folder</returns>
        public static string GetMoviestormAddonRootFolder()
        {
            string tentativeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Moviestorm\\Addon";

            return Directory.Exists(tentativeFolder) ? tentativeFolder : null;
        }



        /// <summary>
        /// Checks whether it's a path to a valid Moviestorm folder
        /// </summary>
        /// <param name="pPath">Path to check</param>
        /// <returns>Path to the addon folder or, null if not valid</returns>
        public static string CheckAddonFolder(string pPath)
        {
            string tentativeFolder = null;

            // Verify whether it's an existent folder or file
            if (Directory.Exists(pPath))
                tentativeFolder = pPath;
            else
            {
                if (File.Exists(pPath))
                {
                    tentativeFolder = Path.GetDirectoryName(pPath);
                }
            }

            if (tentativeFolder == null)
                return null;

            if (!File.Exists(Path.Combine(tentativeFolder, AddonSummaryFileName)) ||
                !File.Exists(Path.Combine(tentativeFolder, ManifestFileName)) ||
                !Directory.Exists(Path.Combine(tentativeFolder, DataFolderName)))
                return null;

            return tentativeFolder;
        }


        // ------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Copy a list of files from the Manifest file into the Data Folder Hierarchy
        /// </summary>
        /// <param name="pFileList">List of files to copy</param>
        /// <param name="pNeedsRefresh">Display for Data Folder needs refreshing</param>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        public bool CopyFilesFromManifestToData(List<string> pFileList, out bool pNeedsRefresh, out string pErrorText)
        {
            return AddonContents.CopyFilesFromManifestMirror(pFileList, AddonManifest.ManifestContentMirrorPath, out pNeedsRefresh, out pErrorText);
        }


        /// <summary>
        /// Copy a list of files from the Data Folder Hierarchy into the Manifest file
        /// </summary>
        /// <param name="pFileList">List of files to copy</param>
        /// <param name="pManifestNeedsRefreshing">Manifest file needs recreating</param>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        public bool CopyFilesFromDataToManifest(List<string> pFileList, out bool pManifestNeedsRefreshing, out string pErrorText)
        {
            return AddonContents.CopyFilesToManifestMirror(pFileList, AddonManifest.ManifestContentMirrorPath, out pManifestNeedsRefreshing, out pErrorText);
        }


        // -----------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Recreate Manifest file from mirror folder
        /// </summary>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        public bool RecreateManifestMirror(out string pErrorText)
        {
            return AddonManifest.RecreateManifestMirror(out pErrorText);
        }


        // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Recreate Manifest file from mirror folder
        /// </summary>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of operation</returns>
        public bool RefreshManifestFromMirror(out string pErrorText)
        {
            return AddonManifest.RefreshManifestFromMirror(out pErrorText);
        }


        // ------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public bool CreateAddonFile(string newAddonFilePath, bool pNoMeatFiles, out string pErrorText)
        {
            pErrorText = null;
            string addonFolder = AddonFolder;
            if (pNoMeatFiles)
            {
                // No meaty files
                Regex meatyFileMaskRegex = AddonContents.GetMeatyFileMaskRegex();
                string tempAddonFolder = GetTempAddonFolder();

                if (!DirectoryCopy(AddonFolder, tempAddonFolder, true, meatyFileMaskRegex, out pErrorText))
                    return false;

                addonFolder = tempAddonFolder;
            }

            SevenZipArchiver archiver = new SevenZipArchiver(newAddonFilePath);
            if (!archiver.ArchiveFolder(addonFolder))
            {
                pErrorText = archiver.LastErrorText;
                return false;
            }

            return true;
        }



        private string GetTempAddonFolder()
        {
            int count = 0;
            string tempAddonFolderBase = $"{_tempPath}\\{Name}-";
            while (true)
            {
                string tempAddonFolder = $"{tempAddonFolderBase}{count}";
                if (!Directory.Exists(tempAddonFolder))
                    return tempAddonFolder;
            }
        }


        // ----------------------------------------------------------------------------------------------------------------------------


        private bool DirectoryCopy(string pSourceDirName, string pDestDirName, bool pCopySubDirs, Regex pExclusionMask, out string pErrorText)
        {
            pErrorText = null;

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(pSourceDirName);

            if (!dir.Exists)
            {
                pErrorText = $"Source directory does not exist or could not be found: {pSourceDirName}";
                return false;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(pDestDirName))
            {
                Directory.CreateDirectory(pDestDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (pExclusionMask != null)
                {
                    if (pExclusionMask.IsMatch(file.Name))
                        continue;
                }
                string temppath = Path.Combine(pDestDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (pCopySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(pDestDirName, subdir.Name);
                    if (!DirectoryCopy(subdir.FullName, temppath, pCopySubDirs, pExclusionMask, out pErrorText))
                        return false;
                }
            }

            return true;
        }


        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Remove meaty files from the Data folder
        /// </summary>
        /// <param name="pNeedsRefreshing">Needs refreshing display</param>
        /// <param name="pErrorText">Text of error, if any</param>
        /// <returns>Result of the operation</returns>
        public bool RemoveMeatyFiles(out bool pNeedsRefreshing, out string pErrorText)
        {
            return AddonContents.RemoveMeatyFiles(out pNeedsRefreshing, out pErrorText);
        }
    }
}
