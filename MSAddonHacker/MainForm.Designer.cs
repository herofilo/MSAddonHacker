namespace MSAddonHacker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAddonFolder = new System.Windows.Forms.TextBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.pbSelectFolder = new System.Windows.Forms.Button();
            this.selectFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbAddonName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLightPack = new System.Windows.Forms.CheckBox();
            this.pbPackAddon = new System.Windows.Forms.Button();
            this.gbManifest = new System.Windows.Forms.GroupBox();
            this.tvManifestFiles = new System.Windows.Forms.TreeView();
            this.cmManifestFileMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiMftCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiMftCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiMftRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContents = new System.Windows.Forms.Panel();
            this.pbRestoreBackup = new System.Windows.Forms.Button();
            this.cbManifestBackup = new System.Windows.Forms.CheckBox();
            this.pbDataToManifest = new System.Windows.Forms.Button();
            this.pbManifestToData = new System.Windows.Forms.Button();
            this.gbContentsData = new System.Windows.Forms.GroupBox();
            this.tvDataFiles = new System.Windows.Forms.TreeView();
            this.cmContentFilesMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiDataCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDataCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdCreatePack = new System.Windows.Forms.SaveFileDialog();
            this.ofdSelectBackupFile = new System.Windows.Forms.OpenFileDialog();
            this.pbRemoveMeatyFiles = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.gbManifest.SuspendLayout();
            this.cmManifestFileMenu.SuspendLayout();
            this.pnlContents.SuspendLayout();
            this.gbContentsData.SuspendLayout();
            this.cmContentFilesMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Addon Folder:";
            // 
            // tbAddonFolder
            // 
            this.tbAddonFolder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbAddonFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAddonFolder.Location = new System.Drawing.Point(83, 32);
            this.tbAddonFolder.Name = "tbAddonFolder";
            this.tbAddonFolder.ReadOnly = true;
            this.tbAddonFolder.Size = new System.Drawing.Size(726, 13);
            this.tbAddonFolder.TabIndex = 1;
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(3, 474);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(991, 101);
            this.tbLog.TabIndex = 2;
            // 
            // pbSelectFolder
            // 
            this.pbSelectFolder.Location = new System.Drawing.Point(910, 3);
            this.pbSelectFolder.Name = "pbSelectFolder";
            this.pbSelectFolder.Size = new System.Drawing.Size(75, 23);
            this.pbSelectFolder.TabIndex = 3;
            this.pbSelectFolder.Text = "Select";
            this.pbSelectFolder.UseVisualStyleBackColor = true;
            this.pbSelectFolder.Click += new System.EventHandler(this.pbSelectFolder_Click);
            // 
            // selectFolderDialog
            // 
            this.selectFolderDialog.RootFolder = System.Environment.SpecialFolder.UserProfile;
            this.selectFolderDialog.ShowNewFolderButton = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbAddonName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbLightPack);
            this.panel1.Controls.Add(this.pbPackAddon);
            this.panel1.Controls.Add(this.tbAddonFolder);
            this.panel1.Controls.Add(this.pbSelectFolder);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(991, 69);
            this.panel1.TabIndex = 4;
            // 
            // tbAddonName
            // 
            this.tbAddonName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbAddonName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAddonName.Location = new System.Drawing.Point(83, 13);
            this.tbAddonName.Name = "tbAddonName";
            this.tbAddonName.ReadOnly = true;
            this.tbAddonName.Size = new System.Drawing.Size(726, 13);
            this.tbAddonName.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Addon Name:";
            // 
            // cbLightPack
            // 
            this.cbLightPack.AutoSize = true;
            this.cbLightPack.Location = new System.Drawing.Point(905, 42);
            this.cbLightPack.Name = "cbLightPack";
            this.cbLightPack.Size = new System.Drawing.Size(66, 17);
            this.cbLightPack.TabIndex = 5;
            this.cbLightPack.Text = "No meat";
            this.cbLightPack.UseVisualStyleBackColor = true;
            // 
            // pbPackAddon
            // 
            this.pbPackAddon.Location = new System.Drawing.Point(824, 38);
            this.pbPackAddon.Name = "pbPackAddon";
            this.pbPackAddon.Size = new System.Drawing.Size(75, 23);
            this.pbPackAddon.TabIndex = 4;
            this.pbPackAddon.Text = "Create pack";
            this.pbPackAddon.UseVisualStyleBackColor = true;
            this.pbPackAddon.Click += new System.EventHandler(this.pbPackAddon_Click);
            // 
            // gbManifest
            // 
            this.gbManifest.Controls.Add(this.tvManifestFiles);
            this.gbManifest.Location = new System.Drawing.Point(3, 53);
            this.gbManifest.Name = "gbManifest";
            this.gbManifest.Size = new System.Drawing.Size(463, 325);
            this.gbManifest.TabIndex = 5;
            this.gbManifest.TabStop = false;
            this.gbManifest.Text = "Assets in Manifest File";
            // 
            // tvManifestFiles
            // 
            this.tvManifestFiles.ContextMenuStrip = this.cmManifestFileMenu;
            this.tvManifestFiles.Location = new System.Drawing.Point(9, 19);
            this.tvManifestFiles.Name = "tvManifestFiles";
            this.tvManifestFiles.Size = new System.Drawing.Size(448, 300);
            this.tvManifestFiles.TabIndex = 0;
            // 
            // cmManifestFileMenu
            // 
            this.cmManifestFileMenu.Enabled = false;
            this.cmManifestFileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiMftCopyAll,
            this.cmiMftCopySelected,
            this.cmiMftRestore});
            this.cmManifestFileMenu.Name = "cmManifestFileMenu";
            this.cmManifestFileMenu.Size = new System.Drawing.Size(307, 70);
            // 
            // cmiMftCopyAll
            // 
            this.cmiMftCopyAll.Enabled = false;
            this.cmiMftCopyAll.Name = "cmiMftCopyAll";
            this.cmiMftCopyAll.Size = new System.Drawing.Size(306, 22);
            this.cmiMftCopyAll.Text = "Copy files in Manifest File to Data Folder";
            this.cmiMftCopyAll.Click += new System.EventHandler(this.cmiMftCopyAll_Click);
            // 
            // cmiMftCopySelected
            // 
            this.cmiMftCopySelected.Enabled = false;
            this.cmiMftCopySelected.Name = "cmiMftCopySelected";
            this.cmiMftCopySelected.Size = new System.Drawing.Size(306, 22);
            this.cmiMftCopySelected.Text = "Copy selected file in Manifest to Data Folder";
            this.cmiMftCopySelected.Click += new System.EventHandler(this.cmiMftCopySelected_Click);
            // 
            // cmiMftRestore
            // 
            this.cmiMftRestore.Enabled = false;
            this.cmiMftRestore.Name = "cmiMftRestore";
            this.cmiMftRestore.Size = new System.Drawing.Size(306, 22);
            this.cmiMftRestore.Text = "Restore Manifest File from Backup";
            this.cmiMftRestore.Click += new System.EventHandler(this.cmiMftRestore_Click);
            // 
            // pnlContents
            // 
            this.pnlContents.Controls.Add(this.pbRemoveMeatyFiles);
            this.pnlContents.Controls.Add(this.pbRestoreBackup);
            this.pnlContents.Controls.Add(this.cbManifestBackup);
            this.pnlContents.Controls.Add(this.pbDataToManifest);
            this.pnlContents.Controls.Add(this.pbManifestToData);
            this.pnlContents.Controls.Add(this.gbContentsData);
            this.pnlContents.Controls.Add(this.gbManifest);
            this.pnlContents.Location = new System.Drawing.Point(3, 87);
            this.pnlContents.Name = "pnlContents";
            this.pnlContents.Size = new System.Drawing.Size(991, 381);
            this.pnlContents.TabIndex = 6;
            // 
            // pbRestoreBackup
            // 
            this.pbRestoreBackup.Enabled = false;
            this.pbRestoreBackup.Location = new System.Drawing.Point(282, 24);
            this.pbRestoreBackup.Name = "pbRestoreBackup";
            this.pbRestoreBackup.Size = new System.Drawing.Size(106, 23);
            this.pbRestoreBackup.TabIndex = 10;
            this.pbRestoreBackup.Text = "Restore Backup";
            this.pbRestoreBackup.UseVisualStyleBackColor = true;
            this.pbRestoreBackup.Click += new System.EventHandler(this.pbRestoreBackup_Click);
            // 
            // cbManifestBackup
            // 
            this.cbManifestBackup.AutoSize = true;
            this.cbManifestBackup.Enabled = false;
            this.cbManifestBackup.Location = new System.Drawing.Point(533, 28);
            this.cbManifestBackup.Name = "cbManifestBackup";
            this.cbManifestBackup.Size = new System.Drawing.Size(96, 17);
            this.cbManifestBackup.TabIndex = 9;
            this.cbManifestBackup.Text = "Create backup";
            this.cbManifestBackup.UseVisualStyleBackColor = true;
            // 
            // pbDataToManifest
            // 
            this.pbDataToManifest.Enabled = false;
            this.pbDataToManifest.Location = new System.Drawing.Point(478, 24);
            this.pbDataToManifest.Name = "pbDataToManifest";
            this.pbDataToManifest.Size = new System.Drawing.Size(49, 23);
            this.pbDataToManifest.TabIndex = 8;
            this.pbDataToManifest.Text = "<-";
            this.pbDataToManifest.UseVisualStyleBackColor = true;
            this.pbDataToManifest.Click += new System.EventHandler(this.pbDataToManifest_Click);
            // 
            // pbManifestToData
            // 
            this.pbManifestToData.Enabled = false;
            this.pbManifestToData.Location = new System.Drawing.Point(411, 24);
            this.pbManifestToData.Name = "pbManifestToData";
            this.pbManifestToData.Size = new System.Drawing.Size(49, 23);
            this.pbManifestToData.TabIndex = 7;
            this.pbManifestToData.Text = "->";
            this.pbManifestToData.UseVisualStyleBackColor = true;
            this.pbManifestToData.Click += new System.EventHandler(this.pbManifestToData_Click);
            // 
            // gbContentsData
            // 
            this.gbContentsData.Controls.Add(this.tvDataFiles);
            this.gbContentsData.Location = new System.Drawing.Point(472, 53);
            this.gbContentsData.Name = "gbContentsData";
            this.gbContentsData.Size = new System.Drawing.Size(513, 325);
            this.gbContentsData.TabIndex = 6;
            this.gbContentsData.TabStop = false;
            this.gbContentsData.Text = "Content Files";
            // 
            // tvDataFiles
            // 
            this.tvDataFiles.ContextMenuStrip = this.cmContentFilesMenu;
            this.tvDataFiles.Location = new System.Drawing.Point(6, 19);
            this.tvDataFiles.Name = "tvDataFiles";
            this.tvDataFiles.Size = new System.Drawing.Size(501, 300);
            this.tvDataFiles.TabIndex = 0;
            // 
            // cmContentFilesMenu
            // 
            this.cmContentFilesMenu.Enabled = false;
            this.cmContentFilesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiDataCopyAll,
            this.cmiDataCopySelected});
            this.cmContentFilesMenu.Name = "cmContentFilesMenu";
            this.cmContentFilesMenu.Size = new System.Drawing.Size(299, 48);
            // 
            // cmiDataCopyAll
            // 
            this.cmiDataCopyAll.Enabled = false;
            this.cmiDataCopyAll.Name = "cmiDataCopyAll";
            this.cmiDataCopyAll.Size = new System.Drawing.Size(298, 22);
            this.cmiDataCopyAll.Text = "Copy Files in Data Folder into Manifest File";
            this.cmiDataCopyAll.Click += new System.EventHandler(this.cmiDataCopyAll_Click);
            // 
            // cmiDataCopySelected
            // 
            this.cmiDataCopySelected.Enabled = false;
            this.cmiDataCopySelected.Name = "cmiDataCopySelected";
            this.cmiDataCopySelected.Size = new System.Drawing.Size(298, 22);
            this.cmiDataCopySelected.Text = "Copy Selected File into Manifest File";
            this.cmiDataCopySelected.Click += new System.EventHandler(this.cmiDataCopySelected_Click);
            // 
            // sfdCreatePack
            // 
            this.sfdCreatePack.Filter = "Moviestorm addon files|*.addon";
            // 
            // ofdSelectBackupFile
            // 
            this.ofdSelectBackupFile.Filter = "Backup files|*.jar";
            this.ofdSelectBackupFile.RestoreDirectory = true;
            // 
            // pbRemoveMeatyFiles
            // 
            this.pbRemoveMeatyFiles.Location = new System.Drawing.Point(875, 22);
            this.pbRemoveMeatyFiles.Name = "pbRemoveMeatyFiles";
            this.pbRemoveMeatyFiles.Size = new System.Drawing.Size(96, 23);
            this.pbRemoveMeatyFiles.TabIndex = 11;
            this.pbRemoveMeatyFiles.Text = "Remove Meat";
            this.pbRemoveMeatyFiles.UseVisualStyleBackColor = true;
            this.pbRemoveMeatyFiles.Click += new System.EventHandler(this.pbRemoveMeatyFiles_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 576);
            this.Controls.Add(this.pnlContents);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MSAddonHacker  ver.";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbManifest.ResumeLayout(false);
            this.cmManifestFileMenu.ResumeLayout(false);
            this.pnlContents.ResumeLayout(false);
            this.pnlContents.PerformLayout();
            this.gbContentsData.ResumeLayout(false);
            this.cmContentFilesMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAddonFolder;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button pbSelectFolder;
        private System.Windows.Forms.FolderBrowserDialog selectFolderDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button pbPackAddon;
        private System.Windows.Forms.CheckBox cbLightPack;
        private System.Windows.Forms.GroupBox gbManifest;
        private System.Windows.Forms.TreeView tvManifestFiles;
        private System.Windows.Forms.Panel pnlContents;
        private System.Windows.Forms.GroupBox gbContentsData;
        private System.Windows.Forms.TreeView tvDataFiles;
        private System.Windows.Forms.Button pbManifestToData;
        private System.Windows.Forms.CheckBox cbManifestBackup;
        private System.Windows.Forms.Button pbDataToManifest;
        private System.Windows.Forms.Button pbRestoreBackup;
        private System.Windows.Forms.ContextMenuStrip cmManifestFileMenu;
        private System.Windows.Forms.ContextMenuStrip cmContentFilesMenu;
        private System.Windows.Forms.ToolStripMenuItem cmiMftCopyAll;
        private System.Windows.Forms.ToolStripMenuItem cmiMftCopySelected;
        private System.Windows.Forms.ToolStripMenuItem cmiMftRestore;
        private System.Windows.Forms.ToolStripMenuItem cmiDataCopyAll;
        private System.Windows.Forms.ToolStripMenuItem cmiDataCopySelected;
        private System.Windows.Forms.SaveFileDialog sfdCreatePack;
        private System.Windows.Forms.TextBox tbAddonName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog ofdSelectBackupFile;
        private System.Windows.Forms.Button pbRemoveMeatyFiles;
    }
}

