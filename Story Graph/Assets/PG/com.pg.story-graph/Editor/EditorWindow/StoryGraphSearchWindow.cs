using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.Graph
{
    using Nodes;
    using System.Linq;

    public class StoryGraphSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private StoryGraphView _graphView;

        public void Initialize(StoryGraphView graphView)
        {
            _graphView = graphView;
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>();
            OnSearchNodeType(searchTreeEntries, "Create Element", typeof(StoryNode), -1);
            return searchTreeEntries;
        }

        void OnSearchNodeType(List<SearchTreeEntry> searchTreeEntries, string groupName, System.Type targetType, int targetGroup = 0)
        {
            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent(GetName(groupName)), 1 + targetGroup));

            // ������� ��������� ���, ����� ����������� ���� ���� �������
            //var structureTypes = TypeCache.GetTypesDerivedFrom(targetType).OrderBy(t => !t.IsAbstract);
            var structureTypes = TypeCache.GetTypesDerivedFrom(targetType)
    .OrderBy(t => !t.IsAbstract)                // ����������� (�����) ����� �������
    .ThenBy(t => GetName(t.Name));              // ����� ���������� �� �������� � ������ ��������������

            foreach (var type in structureTypes)
            {
                if (!type.IsAbstract)
                {
                    if (type.BaseType == targetType) // ���������������� ������������ �� targetType
                    {
                        searchTreeEntries.Add(
                            new SearchTreeEntry(new GUIContent("    " + GetName(type.Name)))
                            {
                                level = 2 + targetGroup,
                                userData = type
                            });
                    }
                }
                else
                {
                    if (type.BaseType == targetType)
                    {
                        OnSearchNodeType(searchTreeEntries, type.Name, type, 1 + targetGroup);
                    }
                }
            }
        }


        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is System.Type nodeType)
            {
                _graphView.CreateNode(nodeType, _graphView.GetLocalMousePosition(context.screenMousePosition));

                return true;
            }
            return false;
        }
        public static string GetName(string storyNodeName)
        {
            // ������� ������ "Node" �� ����� ������ (��� "s"!)
            string result = System.Text.RegularExpressions.Regex.Replace(
                storyNodeName, "Node$", ""
            );
            // ��������� ������ ����� ������ ��������� ������ ����� ���������
            result = System.Text.RegularExpressions.Regex.Replace(result, "(?<=[a-z])([A-Z])", " $1");
            return result;
        }

    }
}
