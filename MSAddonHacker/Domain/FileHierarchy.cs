using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSAddonHacker.Domain
{
    public class FileHierarchy
    {

        public FileHierNode Root { get; private set; } = null;


        public FileHierarchy(List<string> pNameList)
        {
            FromNameList(pNameList);
        }



        public void FromNameList(List<string> pNameList)
        {
            Root = null;
            if ((pNameList == null) || (pNameList.Count == 0))
                return;

            Root = new FileHierNode("\\", "\\");
            foreach (string item in pNameList)
            {
                ExtractNodes(item);
            }
        }

        private void ExtractNodes(string pFileName)
        {
            pFileName = pFileName?.Trim();
            if (string.IsNullOrEmpty(pFileName))
                return;

            FileHierNode currentNode = Root;
            string[] components = pFileName.Split("\\".ToCharArray());

            for (int index = 0; index < components.Length; ++index)
            {
                string component = components[index];

                currentNode.AddChild(component, pFileName);
                currentNode = currentNode.GetChildByName(component);
            }
        }
    }



    public class FileHierNode
    {
        public string Name { get; private set; }

        public string FullPath { get; private set; }

        public List<FileHierNode> Children { get; private set; }

        public bool IsFolder => (Children != null);



        public FileHierNode(string pName, string pFullPath)
        {
            Name = pName;
            FullPath = pFullPath;
        }


        public void AddChild(string pChildName, string pFullPath)
        {
            pChildName = pChildName?.Trim();
            if (string.IsNullOrEmpty(pChildName))
                return;

            if (Children == null)
            {
                Children = new List<FileHierNode>();
            }

            foreach (FileHierNode item in Children)
            {
                if (item.Name == pChildName)
                    return;
            }

            Children.Add(new FileHierNode(pChildName, pFullPath));
        }


        public FileHierNode GetChildByName(string pChildName)
        {
            pChildName = pChildName?.Trim();
            if (string.IsNullOrEmpty(pChildName) || (Children == null))
                return null;

            foreach (FileHierNode item in Children)
            {
                if (item.Name == pChildName)
                    return item;
            }

            return null;
        }


    }


}
