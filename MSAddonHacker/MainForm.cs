using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MSAddonHacker.Domain;
using MSAddonHacker.UserInterface;
using MSAddonHacker.Util;

namespace MSAddonHacker
{
    public partial class MainForm : Form
    {

        private string[] _args = null;

        private string _tempFolder;

        private string _backupFolder;

        private string _moviestormAddonRootFolder;

        private Addon _addon = null;

        private AssetDisplay _manifestAssetDisplay = null;

        private AssetDisplay _contentsAssetDisplay = null;

        private int _mftBackupFiles = 0;


        // ----------------------------------------------------------------------------------------------------------------------------------------------------------

        public MainForm(string[] pArgs)
        {
            InitializeComponent();

            _args = pArgs;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Version version =
                Assembly.GetExecutingAssembly().GetName().Version;

            Text = $@"Moviestorm Addon Hacker     (version {version.Major}.{version.Minor}.{version.Build})";


            InitializationChores();
        }


        private void InitializationChores()
        {
            // initialize controls and global variables
            _moviestormAddonRootFolder = Addon.GetMoviestormAddonRootFolder();

            _tempFolder = Utils.GetTempDirectory();
            _backupFolder = Utils.GetBackupDirectory();

            _manifestAssetDisplay = new AssetDisplay(tvManifestFiles);
            _contentsAssetDisplay = new AssetDisplay(tvDataFiles);

            // ContextHelp.HelpNamespace = Utility.GetHelpFilename();

            SetToolTips();

            sfdCreatePack.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            pbSelectFolder.Focus();

            // -----------------------------------------------------
            string errorText;

            Utils.ResetTemporaryFolders(out errorText);


            string addonFolder = CheckArguments(_args, out errorText);
            if (!string.IsNullOrEmpty(errorText))
            {
                tbLog.AppendText(errorText);
                return;
            }


            if (addonFolder != null)
            {
                GetAddon(addonFolder);
            }


        }

        private void SetToolTips()
        {
            ToolTip formToolTip = new ToolTip();
            formToolTip.SetDefaults();

            // Sets up the ToolTip text for the Button and Checkbox.

            // formToolTip.SetToolTip(this, "Press F1 for help");

            formToolTip.SetToolTip(pbSelectFolder, "Select new addon folder to open");
            formToolTip.SetToolTip(pbPackAddon, "Create pack file for distribution");
            formToolTip.SetToolTip(pbDataToManifest, "Copy content files into the Asset Manifest file");
            formToolTip.SetToolTip(pbManifestToData, "Copy files in the Asset Manifest file into the content file hierarchy");
            formToolTip.SetToolTip(pbRestoreBackup, "Restore Asset Manifest file from backup");
            formToolTip.SetToolTip(pbRemoveMeatyFiles, "Delete Meaty Files from the Data Folder");

            formToolTip.SetToolTip(cbLightPack, "Create pack file for distribution without meaty files");
            formToolTip.SetToolTip(cbManifestBackup, "Create backup of current Asset Manifest file");



        }


        /// <summary>
        /// Checks the line command arguments
        /// </summary>
        /// <param name="pArgs">Arguments</param>
        /// <param name="pErrorText">Error text</param>
        /// <returns>Path to the addon folder</returns>
        private string CheckArguments(string[] pArgs, out string pErrorText)
        {
            string addonFolder = null;
            pErrorText = null;
            if ((pArgs != null) && (pArgs.Length > 0))
            {
                foreach (string argument in pArgs)
                {
                    string arg = argument.ToLower().Trim();
                    if (string.IsNullOrEmpty(arg))
                    {
                        // TODO : options
                        continue;
                    }

                    if (arg.StartsWith("-") || arg.StartsWith("/"))
                    {
                        // An option
                        continue;
                    }

                    addonFolder = arg;
                    break;
                }
            }

            return addonFolder;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------


        private bool GetAddon(string pPath)
        {

            tbLog.Clear();

            tbAddonName.Text = tbAddonFolder.Text = null;

            _manifestAssetDisplay.Reset();
            _contentsAssetDisplay.Reset();

            ResetControls();

            _mftBackupFiles = 0;

            tbLog.AppendText($"New addon path to check: {pPath}\n");

            bool gotAddonFolder = false;
            string errorText = null;

            if (!Utils.ResetTemporaryFolders(out errorText))
            {
                _addon = null;
                tbLog.AppendText(errorText + "\n");
                return gotAddonFolder;
            }


            if (pPath != null)
            {
                try
                {
                    _addon = new Addon(pPath, _tempFolder);
                    gotAddonFolder = true;
                    tbLog.AppendText($"New addon folder checked OK: {_addon.AddonFolder}\n");
                }
                catch (Exception exception)
                {
                    errorText = exception.Message;
                }
            }

            tbAddonName.Text = _addon?.Name;
            tbAddonFolder.Text = _addon?.AddonFolder;

            // Set control status
            if (gotAddonFolder)
            {
                _manifestAssetDisplay.SetData(_addon.AddonManifest.AssetList);
                _contentsAssetDisplay.SetData(_addon.AddonContents.AssetList);
                pbDataToManifest.Enabled = pbManifestToData.Enabled = pbPackAddon.Enabled = pbRemoveMeatyFiles.Enabled = true;
                cbLightPack.Enabled =
                    cbManifestBackup.Enabled = cbManifestBackup.Checked = true;
                cmContentFilesMenu.Enabled = cmManifestFileMenu.Enabled = true;
                cmiMftCopyAll.Enabled = cmiMftCopySelected.Enabled =
                    cmiDataCopyAll.Enabled = cmiDataCopySelected.Enabled = true;
                cmiMftRestore.Enabled = false;
            }



            if (errorText != null)
            {
                tbLog.AppendText(errorText + "\n");
            }

            return gotAddonFolder;
        }


        private void ResetControls()
        {
            pbDataToManifest.Enabled = pbManifestToData.Enabled = pbPackAddon.Enabled = pbRemoveMeatyFiles.Enabled = pbRestoreBackup.Enabled = false;
            cbLightPack.Enabled = cbLightPack.Checked =
                cbManifestBackup.Enabled = cbManifestBackup.Checked = false;
            cmContentFilesMenu.Enabled = cmManifestFileMenu.Enabled = false;
        }


        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------


        private void pbSelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = selectFolderDialog.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }

            string newAddonFolder = selectFolderDialog.SelectedPath;
            GetAddon(newAddonFolder);
        }


        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Copy all files in the Manifest file to Data Folder
        /// </summary>
        private void cmiMftCopyAll_Click(object sender, EventArgs e)
        {
            _ManifestCopyToData(_manifestAssetDisplay.FileList);
        }

        /// <summary>
        /// Copy all files in the Manifest file to Data Folder
        /// </summary>
        private void pbManifestToData_Click(object sender, EventArgs e)
        {
            _ManifestCopyToData(_manifestAssetDisplay.FileList);
        }


        /// <summary>
        /// Copy selected file in the Manifest file to Data Folder
        /// </summary>
        private void cmiMftCopySelected_Click(object sender, EventArgs e)
        {
            string file = _manifestAssetDisplay.GetSelectedFile();
            if ((file == null) || (file.Trim().Length == 0))
                return;

            List<string> fileList = new List<string>() { file };

            _ManifestCopyToData(fileList);
        }



        /// <summary>
        /// Copy files in the Manifest file to Data Folder
        /// <param name="pFileList">List of files to copy</param>        
        /// </summary>
        private void _ManifestCopyToData(List<string> pFileList)
        {
            if ((pFileList == null) || (pFileList.Count == 0))
                return;

            tbLog.AppendText("Copying files in Manifest file to Data folder\n");

            bool dataDisplayNeedsRefreshing;
            string errorText;
            if (!_addon.CopyFilesFromManifestToData(pFileList, out dataDisplayNeedsRefreshing, out errorText))
            {
                if (errorText != null)
                    tbLog.AppendText(errorText + "\n");
            }
            else
                tbLog.AppendText("Copy of files: OK\n");
            if (dataDisplayNeedsRefreshing)
            {
                tbLog.AppendText("Refreshing info of files in Data folder\n");
                _contentsAssetDisplay.SetData(_addon.AddonContents.MeatFileList);
            }
        }




        /// <summary>
        /// Restore Manifest file from backup
        /// </summary>
        private void cmiMftRestore_Click(object sender, EventArgs e)
        {
            _RestoreManifestFromBackup();
        }


        /// <summary>
        /// Restore Manifest file from backup
        /// </summary>
        private void pbRestoreBackup_Click(object sender, EventArgs e)
        {
            _RestoreManifestFromBackup();
        }

        /// <summary>
        /// Restore Manifest file from backup
        /// </summary>
        private void _RestoreManifestFromBackup()
        {
            if (_mftBackupFiles == 0)
                return;

            ofdSelectBackupFile.InitialDirectory = _backupFolder;
            ofdSelectBackupFile.FileName = null;
            if (ofdSelectBackupFile.ShowDialog(this) != DialogResult.OK)
                return;

            // Restore Manifest file from backup
            string backupFile = ofdSelectBackupFile.FileName;
            if (string.IsNullOrEmpty(backupFile))
                return;
            File.Copy(backupFile, _addon.AddonManifest.ManifestFilePath, true);
            tbLog.AppendText($"Manifest file restored from backup file '{Path.GetFileName(backupFile)}'");

            // Recreate Manifest folder
            string errorText;
            Utils.ResetTempFolder(out errorText);
            if (!Directory.Exists(_tempFolder))
            {
                Directory.CreateDirectory(_tempFolder);
            }

            if (!_addon.RecreateManifestMirror(out errorText))
            {
                ResetControls();
                tbLog.AppendText($"Oops! An error has occurred while recreating the Manifest mirror folder: {errorText}\n");
                tbLog.AppendText("You'd better try relaunching the program and/or reloading the addon\n");
                return;
            }
            tbLog.AppendText("Manifest mirror folder re-created.\n");


            // Refresh display

            tbLog.AppendText("Refreshing info of files in Manifest file\n");
            _manifestAssetDisplay.SetData(_addon.AddonManifest.AssetList);
        }


        // --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Copy all files in the Data Folder into the Manifest file, optionally making a backup of the current version
        /// </summary>
        private void cmiDataCopyAll_Click(object sender, EventArgs e)
        {
            _CopyDataFilesIntoManifest(_contentsAssetDisplay.FileList);
        }

        /// <summary>
        /// Copy all files in the Data Folder into the Manifest file, optionally making a backup of the current version
        /// </summary>
        private void pbDataToManifest_Click(object sender, EventArgs e)
        {
            _CopyDataFilesIntoManifest(_contentsAssetDisplay.FileList);
        }


        /// <summary>
        /// Copy the selected file in the Data Folder into the Manifest file, optionally making a backup of the current version
        /// </summary>
        private void cmiDataCopySelected_Click(object sender, EventArgs e)
        {
            string file = _contentsAssetDisplay.GetSelectedFile();
            if ((file == null) || (file.Trim().Length == 0))
                return;

            List<string> fileList = new List<string>() { file };

            _CopyDataFilesIntoManifest(fileList);
        }


        /// <summary>
        /// Copy all files in the Data Folder into the Manifest file, optionally making a backup of the current version
        /// <param name="pFileList">List of files to copy</param>
        /// </summary>
        private void _CopyDataFilesIntoManifest(List<string> pFileList)
        {
            if ((pFileList == null) || (pFileList.Count == 0))
                return;

            tbLog.AppendText("Copying files Data folder into Manifest file\n");

            string errorText;
            bool someFileCopied;

            if (!_addon.CopyFilesFromDataToManifest(pFileList, out someFileCopied, out errorText))
            {
                if (errorText != null)
                    tbLog.AppendText(errorText + "\n");

                if (someFileCopied)
                    _RecreateMirror();
                return;
            }

            // Process ok

            if (!someFileCopied)
            {
                tbLog.AppendText("No file needing to be copied into Manifest file\n");
                return;
            }

            // One or more files have been replaced in the Manifest file
            tbLog.AppendText("Copy of files into Manifest mirror: OK\n");

            if (cbManifestBackup.Checked)
            {
                string backupFile = Utils.CreateManifestBackupFile(_addon.AddonManifest.ManifestFilePath, out errorText);
                if (backupFile == null)
                {
                    tbLog.AppendText($"Creating Manifest backup, ERROR: {errorText}\n");
                    return;
                }
                tbLog.AppendText($"Manifest backup created: {backupFile}\n");
                _mftBackupFiles++;
                pbRestoreBackup.Enabled = true;
            }

            if (!_addon.RefreshManifestFromMirror(out errorText))
            {
                tbLog.AppendText($"Oops! An error has occurred while updating the Manifest file: {errorText}\n");
                return;
            }

            tbLog.AppendText("Refreshing info of files in Manifest file\n");
            _manifestAssetDisplay.SetData(_addon.AddonManifest.AssetList);
        }


        private bool _RecreateMirror()
        {
            string errorText;
            bool isOk = _addon.RecreateManifestMirror(out errorText);
            tbLog.AppendText(
                isOk
                    ? "Manifest folder has been recreated\n"
                    : $"Oops! An error has occurred while trying to recreate the Manifest mirror folder: {errorText}\n");
            return isOk;
        }


        // --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Create pack file of the addon for distribution, optionally removing all meaty files
        /// </summary>
        private void pbPackAddon_Click(object sender, EventArgs e)
        {
            sfdCreatePack.FileName = _addon.Name;
            if (sfdCreatePack.ShowDialog(this) != DialogResult.OK)
                return;

            string newAddonFilePath = sfdCreatePack.FileName;
            string errorText;

            tbLog.AppendText(_addon.CreateAddonFile(newAddonFilePath, cbLightPack.Checked, out errorText)
                ? $"Created addon file: {newAddonFilePath}\n"
                : $"Creating addon file: {newAddonFilePath}, ERROR: {errorText}\n");
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------

        private void pbRemoveMeatyFiles_Click(object sender, EventArgs e)
        {
            tbLog.AppendText("Deleting meaty files from the Contents folder");

            bool needsRefreshing;
            string errorText;
            if (!_addon.RemoveMeatyFiles(out needsRefreshing, out errorText))
            {
                tbLog.AppendText($"Error: {errorText}\n");
            }

            // Refresh display
            if (needsRefreshing)
            {
                tbLog.AppendText("Refreshing info of files in Contents folder\n");
                _contentsAssetDisplay.SetData(_addon.AddonContents.AssetList);
            }
        }



        // ---------------------------------------------------------------------------------------------------------------------------------------------


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string errorText;
            Utils.ResetTemporaryFolders(out errorText);
        }


    }
}
