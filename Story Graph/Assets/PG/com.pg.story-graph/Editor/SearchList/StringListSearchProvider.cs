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
    /// Имя, отображаемое в корневом заголовке окна поиска.
    /// </summary>
    public string listName = "Select Value";

    /// <summary>
    /// Инициализация провайдера списком опций и callback'ом для выбранного элемента.
    /// </summary>
    public void Init(List<string> options, Action<string> onItemSelected)
    {
        this.options = options;
        this.onItemSelected = onItemSelected;
    }

    /// <summary>
    /// Вспомогательный класс для построения дерева групп и элементов.
    /// </summary>
    private class TreeNode
    {
        public string name;
        public Dictionary<string, TreeNode> childrenGroups = new Dictionary<string, TreeNode>();
        public List<string> leaves = new List<string>();
    }

    /// <summary>
    /// Формирует древовидный список (SearchTree) из строк с учетом разделения по "/" 
    /// и гарантией, что группы всегда идут первыми в каждом разделе.
    /// </summary>
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>();

        // Добавляем корневую группу
        tree.Add(new SearchTreeGroupEntry(new GUIContent(listName), 0));

        // Строим дерево из опций.
        // Если строка не содержит '/', то она считается листом на верхнем уровне.
        TreeNode rootNode = new TreeNode { name = listName };

        foreach (string option in options)
        {
            string[] parts = option.Split('/');
            TreeNode current = rootNode;

            if (parts.Length == 1)
            {
                // Если опция без разделителей, добавляем как лист верхнего уровня.
                current.leaves.Add(option.Trim());
            }
            else
            {
                // Для строк с разделителем формируем иерархию.
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    string groupName = parts[i].Trim();
                    if (!current.childrenGroups.ContainsKey(groupName))
                    {
                        current.childrenGroups[groupName] = new TreeNode { name = groupName };
                    }
                    current = current.childrenGroups[groupName];
                }
                // Последний элемент – это лист (конечный пункт)
                current.leaves.Add(parts[parts.Length - 1].Trim());
            }
        }

        // Рекурсивный метод для добавления узлов дерева в список SearchTreeEntry.
        void AddEntries(TreeNode node, int level)
        {
            // Сначала добавляем все дочерние группы, сортируя их по алфавиту.
            List<string> groupKeys = new List<string>(node.childrenGroups.Keys);
            groupKeys.Sort();
            foreach (var groupKey in groupKeys)
            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent(groupKey), level));
                // Рекурсивно добавляем вложенные группы и листья для текущей группы,
                // увеличивая уровень на 1.
                AddEntries(node.childrenGroups[groupKey], level + 1);
            }
            // Затем добавляем листья этого узла, отсортированные по алфавиту.
            node.leaves.Sort();
            foreach (var leaf in node.leaves)
            {
                tree.Add(new SearchTreeEntry(new GUIContent("    " + leaf))
                {
                    level = level,
                    userData = /* Если нужно вернуть полный путь, можно сохранить путь:
                                  например, node.name + "/" + leaf, иначе просто leaf */
                              leaf
                });
            }
        }

        // Добавляем записи из корневого узла на уровне 1.
        AddEntries(rootNode, 1);

        return tree;
    }

    /// <summary>
    /// Вызывается при выборе элемента в SearchWindow.
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
