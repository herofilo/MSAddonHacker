using System.IO;
using MSAddonUtilLib.Util;

namespace MSAddonHacker.Util
{
    public static class MiscUtils
    {

        // -------------------------------------------------------------------------------------------------------


        public static string CreateManifestBackupFile(string pAddonManifestFilePath, out string pErrorText)
        {
            pErrorText = null;
            string destinationFolder = Utils.GetBackupDirectory();

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




    }



    
}
