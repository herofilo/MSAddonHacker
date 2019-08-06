MSAddonHacker, by jamoram

Current version: 1.0.0
Last Update: 2019/08/06

A little tool to do a few assorted operations on a selected addon installed on your system:
	- Extracting one or more files in the Manifest file (assetData.jar) into the contents (Data) folder of the addon
	- Updating files in the Manifest file with those in the contents folder of the addon. A backup file of Manifest file can be created in the process.
	- Restoring the Manifest file from a backup previously created
	- Removing the 'meaty' files from the contents folder
	- Creating an addon package file (.addon), either full or with the 'meaty' files removed.
	
The 'meaty' files are those that can be safely removed from the contents (Data) folder of the addon, once this has been published in the Modder's Workshop of Moviestorm, for their info is contained in other meta-info files of the addon. They include the following files:
	- Asset description files (they are included in the Manifest file (assetData.jar):
		DESCRIPTOR
		*.part
		*.template
		*.bodypart
	- Model Cal3D files:
		*.cmf - mesh files (included in the meshData.data and meshData.index files)
		*.crf - material files (not needed any more once the addon has been published)
	

HOW TO USE:
----------
First, select an addon for MSAddonHacker working on, by either:
	1) In Windows Explorer, drag the folder of the addon (or a file in its root folder) and drop it on the application icon.
	2) Launch the application and use the 'Select' addon button
	3) Launch the application from the command line passing the path to the addon folder as an argument
	
Once an addon has been selected, the application create a couple of temporary folders below its home directory for its inner workings. Also two treeviews are populated:
- the one on the left with the contents of the Manifest file (assetData.jar)
- the one on the right with the relevant contents of the Data folder, ie, the files that are also included inside the Manifes file (asset description files):
		DESCRIPTOR
		*.part
		*.template
		*.bodypart	

A contextual menu will pop up if you right-click while the mouse is hovering over any of the treeviews.
		
You can now operate on the currently open addon (see above). These controls are available:
- Select: select and open a new addon folder

- Create pack: create a package file (.addon). 
	If the 'No meat' option is selected, the 'meaty' files are excluded, for they are not really required:
		DESCRIPTOR
		*.part
		*.template
		*.bodypart	
		*.cmf
		*.crf

- Remove Meat: delete 'meaty' files from the contents (Data) folder.

- -> (or 'Copy files in Manifest file to Data Folder' item of the contextual menu of the left treeview): copy every file in the Manifest file into the Data folder, eventually replacing any file already existent.
	The 'Copy selected file in Manifest to Data Folder' item will work the same, just only for the selected file, if any
	
- Restore Backup: restore Manifest file from one the backup files previously created (see below)

- <- (or 'Copy Files in Data Folder into Manifest File' of the contextual menu of the right treeviw): copy every file in the contents (Data) folder of the addon into the Manifest file.
	The 'Copy Selected File into Manifest File' item will work the same, just only for the selected file, if any.
	PLEASE NOTE: this operation will not be effective for adding NEW files to the Manifest file. Should you want to add a new part/template or model to the addon, you'll need to re-publish it with the Modder's Workshop, for the .Addon (signature) file needs to be recreated also, in order to the new files be recognized.
	If the 'Create backup' is checked, a backup file will be created before the Manifest files is actually updated.
	
		
VERSION HISTORY
---------------
* v1.0.0 (20190806): first public release



