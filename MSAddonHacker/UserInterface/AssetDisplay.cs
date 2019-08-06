using System.Collections.Generic;
using System.Windows.Forms;
using MSAddonHacker.Domain;

namespace MSAddonHacker.UserInterface
{
    public class AssetDisplay
    {

        public TreeView TreeView { get; private set; }

        public List<string> FileList { get; private set; }

        public FileHierarchy FileHierarchy { get; private set; }


        public AssetDisplay(TreeView pTreeView)
        {
            TreeView = pTreeView;
        }


        public AssetDisplay(TreeView pTreeView, List<string> pFileList)
        {
            TreeView = pTreeView;
            if(pFileList != null)
                SetData(pFileList);
        }


        // --------------------------------------------------------------------------------------------------------------------------------------

        public void Reset()
        {
            TreeView.Nodes.Clear();
            FileHierarchy = null;
        }


        public void SetData(List<string> pFileList)
        {
            TreeView.Nodes.Clear();

            FileHierarchy = null;
            FileList = null;


            if ((pFileList == null) || (pFileList.Count == 0))
                return;


            if (pFileList == null)
            {
                return;
            }

            FileList = pFileList;
            FileHierarchy = new FileHierarchy(pFileList);

            DisplayNode(null, FileHierarchy.Root);
            TreeView.ExpandAll();
            
        }


        private void DisplayNode(TreeNode pParentNode, FileHierNode pFileNode)
        {
            TreeNode pNode;
            string fileIndex = (pFileNode.IsFolder) ? "" : pFileNode.FullPath;

            if (pParentNode == null)
            {
                
                TreeView.Nodes.Add(fileIndex, pFileNode.Name);
                pNode = TreeView.Nodes[0];
            }
            else
            {
                pParentNode.Nodes.Add(fileIndex, pFileNode.Name);
                pNode = pParentNode.LastNode;
            }

            if (pFileNode.Children != null)
            {
                for (int index = 0; index < pFileNode.Children.Count; ++index)
                {
                    DisplayNode(pNode, pFileNode.Children[index]);
                }
            }
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Returns full path of the file currently selected
        /// </summary>
        /// <returns>Full path of the file currently selected</returns>
        public string GetSelectedFile()
        {
            if ((FileList == null) || (FileList.Count == 0))
                return "";

            string key = TreeView.SelectedNode.Name;
            return key;
        }
    }
}
