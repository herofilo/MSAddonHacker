using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MSAddonHacker.Util
{
    public static class Utils
    {

        private static string _executableDirectory = null;

        private static string _tempDirectory = null;

        private static string _backupDirectory = null;


        // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static string GetExecutableDirectory()
        {
            return _executableDirectory ?? (_executableDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
        }


        public static string GetTempDirectory()
        {
            return _tempDirectory ?? (_tempDirectory = $"{GetExecutableDirectory()}\\Temp");
        }


        public static string GetBackupDirectory()
        {
            return _backupDirectory ?? (_backupDirectory = $"{GetExecutableDirectory()}\\Backup");
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------

        public static bool ResetTemporaryFolders(out string pErrorText)
        {
            return ResetFolder(GetTempDirectory(), out pErrorText) &&
                ResetFolder(GetBackupDirectory(), out pErrorText);
        }


        public static bool ResetTempFolder(out string pErrorText)
        {
            return ResetFolder(GetTempDirectory(), out pErrorText);
        }



        private static bool ResetFolder(string pPath, out string pErrorText)
        {
            pErrorText = null;
            if (string.IsNullOrEmpty(pPath?.Trim()))
            {
                pErrorText = "Folder specification blank";
                return false;
            }

            bool gotOk = false;
            if (!Directory.Exists(pPath))
            {
                try
                {
                    Directory.CreateDirectory(pPath);
                    gotOk = true;
                }
                catch (Exception exception)
                {
                    pErrorText = $"{pPath}: {exception.Message}";
                }

                return gotOk;
            }

            // Folder already exists

            try
            {
                _DeleteDirectory(pPath);
                Thread.Sleep(1000);
                _CreateDirectory(pPath);
                gotOk = true;
            }
            catch (Exception exception)
            {
                pErrorText = $"{pPath}: {exception.Message}";
            }

            return gotOk;
        }


        private static void _DeleteDirectory(string pPath)
        {
            DirectoryInfo baseDirInfo = new DirectoryInfo(pPath);
            foreach (FileInfo file in baseDirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                file.IsReadOnly = false;
                file.Delete();
            }

            Exception exception = null;
            for (int count = 0; count < 10;)
            {
                try
                {
                    Directory.Delete(pPath, true);
                    return;
                }
                catch (Exception e)
                {
                    exception = e;
                }
                count += 2;
                Thread.Sleep(1000 * (count / 2));
            }
            if(exception != null)
                throw exception;
        }

        private static void _CreateDirectory(string pPath)
        {

            Exception exception = null;
            for (int count = 0; count < 10;)
            {
                try
                {
                    Directory.CreateDirectory(pPath);
                    if(Directory.Exists(pPath))
                        return;
                }
                catch (Exception e)
                {
                    exception = e;
                }
                count += 2;
                Thread.Sleep(1000 * (count / 2));
            }
            if (exception != null)
                throw exception;
        }

        // -------------------------------------------------------------------------------------------------------


        public static string CreateManifestBackupFile(string pAddonManifestFilePath, out string pErrorText)
        {
            pErrorText = null;
            string destinationFolder = GetBackupDirectory();

            if (!Directory.Exists(destinationFolder))
            {
                pErrorText = "Backup folder not found";
                return null;
            }

            if (string.IsNullOrEmpty(pAddonManifestFilePath = pAddonManifestFilePath?.Trim()) ||
                !File.Exists(pAddonManifestFilePath))
            {
                pErrorText = "Manifest file: invalid path or file not found";
                return null;
            }


            int count = 0;
            while (true)
            {
                string destinationFile = $"{destinationFolder}\\assetData-{count}.jar";
                if (!File.Exists(destinationFile))
                {
                    File.Copy(pAddonManifestFilePath, destinationFile);
                    return destinationFile;
                }
            }
        }



        // -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Extension method for Tooltips 
        /// </summary>
        /// <param name="pToolTip">tooltip</param>
        public static void SetDefaults(this ToolTip pToolTip)
        {
            // Set up the delays for the ToolTip.
            pToolTip.AutoPopDelay = 5000;
            pToolTip.InitialDelay = 1000;
            pToolTip.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            pToolTip.ShowAlways = true;
        }


    }



    
}
