using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;

public class StringListSearchProvider : ScriptableObject, ISearchWindowProvider
{
    private List<string> options;
    private Action<string> onItemSelected;

    /// <summary>
    /// ���, ������������ � �������� ��������� ���� ������.
    /// </summary>
    public string listName = "Select Value";

    /// <summary>
    /// ������������� ���������� ������� ����� � callback'�� ��� ���������� ��������.
    /// </summary>
    public void Init(List<string> options, Action<string> onItemSelected)
    {
        this.options = options;
        this.onItemSelected = onItemSelected;
    }

    /// <summary>
    /// ��������������� ����� ��� ���������� ������ ����� � ���������.
    /// </summary>
    private class TreeNode
    {
        public string name;
        public Dictionary<string, TreeNode> childrenGroups = new Dictionary<string, TreeNode>();
        public List<string> leaves = new List<string>();
    }

    /// <summary>
    /// ��������� ����������� ������ (SearchTree) �� ����� � ������ ���������� �� "/" 
    /// � ���������, ��� ������ ������ ���� ������� � ������ �������.
    /// </summary>
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>();

        // ��������� �������� ������
        tree.Add(new SearchTreeGroupEntry(new GUIContent(listName), 0));

        // ������ ������ �� �����.
        // ���� ������ �� �������� '/', �� ��� ��������� ������ �� ������� ������.
        TreeNode rootNode = new TreeNode { name = listName };

        foreach (string option in options)
        {
            string[] parts = option.Split('/');
            TreeNode current = rootNode;

            if (parts.Length == 1)
            {
                // ���� ����� ��� ������������, ��������� ��� ���� �������� ������.
                current.leaves.Add(option.Trim());
            }
            else
            {
                // ��� ����� � ������������ ��������� ��������.
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    string groupName = parts[i].Trim();
                    if (!current.childrenGroups.ContainsKey(groupName))
                    {
                        current.childrenGroups[groupName] = new TreeNode { name = groupName };
                    }
                    current = current.childrenGroups[groupName];
                }
                // ��������� ������� � ��� ���� (�������� �����)
                current.leaves.Add(parts[parts.Length - 1].Trim());
            }
        }

        // ����������� ����� ��� ���������� ����� ������ � ������ SearchTreeEntry.
        void AddEntries(TreeNode node, int level)
        {
            // ������� ��������� ��� �������� ������, �������� �� �� ��������.
            List<string> groupKeys = new List<string>(node.childrenGroups.Keys);
            groupKeys.Sort();
            foreach (var groupKey in groupKeys)
            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent(groupKey), level));
                // ���������� ��������� ��������� ������ � ������ ��� ������� ������,
                // ���������� ������� �� 1.
                AddEntries(node.childrenGroups[groupKey], level + 1);
            }
            // ����� ��������� ������ ����� ����, ��������������� �� ��������.
            node.leaves.Sort();
            foreach (var leaf in node.leaves)
            {
                tree.Add(new SearchTreeEntry(new GUIContent("    " + leaf))
                {
                    level = level,
                    userData = /* ���� ����� ������� ������ ����, ����� ��������� ����:
                                  ��������, node.name + "/" + leaf, ����� ������ leaf */
                              leaf
                });
            }
        }

        // ��������� ������ �� ��������� ���� �� ������ 1.
        AddEntries(rootNode, 1);

        return tree;
    }

    /// <summary>
    /// ���������� ��� ������ �������� � SearchWindow.
    /// </summary>
    public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
    {
        if (entry.userData != null)
        {
            onItemSelected?.Invoke(entry.userData as string);
        }
        return true;
    }
}
