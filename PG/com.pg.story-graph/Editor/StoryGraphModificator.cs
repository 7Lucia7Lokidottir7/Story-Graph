using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PG.StorySystem
{
    using Nodes;
    public static class StoryGraphModificator
    {
        public static StoryNode CreateNode(this StoryGraph storyGraph, System.Type type, Vector2 position)
        {
            StoryNode node = ScriptableObject.CreateInstance(type) as StoryNode;
            node.storyGraph = storyGraph;
            node.name = type.Name;
            node.nodePosition = position;

            Undo.RecordObject(storyGraph, "Story Graph (Create Node)");
            storyGraph.nodes.Add(node);

            storyGraph.nextId++;
            node.id = storyGraph.nextId;

            node.guid = GUID.Generate().ToString();
            AssetDatabase.AddObjectToAsset(node, storyGraph);

            Undo.RegisterCreatedObjectUndo(node, "Story Graph (Create Node)");

            AssetDatabase.SaveAssets();
            return node;
        }
        public static StoryNode CreateBaseNode(this StoryGraph storyGraph, System.Type type, Vector2 position)
        {
            StoryNode node = ScriptableObject.CreateInstance(type) as StoryNode;
            node.storyGraph = storyGraph;
            node.name = type.Name;
            node.nodePosition = position;

            Undo.RecordObject(storyGraph, "Story Graph (Create Node)");

            storyGraph.nextId++;
            node.id = storyGraph.nextId;

            node.guid = GUID.Generate().ToString();
            AssetDatabase.AddObjectToAsset(node, storyGraph);

            Undo.RegisterCreatedObjectUndo(node, "Story Graph (Create Node)");

            AssetDatabase.SaveAssets();
            return node;
        }
        public static StoryNode CreateNodeToGroup(this StoryGraph storyGraph, System.Type type, BaseGroupNode groupNode, Vector2 position)
        {
            StoryNode node = ScriptableObject.CreateInstance(type) as StoryNode;
            node.storyGraph = storyGraph;
            node.name = type.Name;
            node.nodePosition = position;

            Undo.RecordObject(storyGraph, "Story Graph (Create Node)");
            groupNode.storyNodes.Add(node);

            storyGraph.nextId++;
            node.id = storyGraph.nextId;

            node.guid = GUID.Generate().ToString();
            AssetDatabase.AddObjectToAsset(node, groupNode);

            Undo.RegisterCreatedObjectUndo(node, "Story Graph (Create Node)");

            AssetDatabase.SaveAssets();
            return node;
        }
        public static void DeleteNode(this StoryGraph storyGraph, StoryNode node, BaseGroupNode groupNode = null)
        {
            Undo.RecordObject(storyGraph, "Story Graph (Delete Node)");
            if (groupNode != null)
            {
                if (groupNode.storyNodes.Contains(node))
                {
                    groupNode.storyNodes.Remove(node);
                }
            }
            else
            {
                if (storyGraph.nodes.Contains(node))
                {
                    storyGraph.nodes.Remove(node);
                }
            }

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }
        public static void DeleteGroupNode(this StoryGraph storyGraph, BaseGroupNode node)
        {
            Undo.RecordObject(storyGraph, "Story Graph (Delete GroupNode)");
            storyGraph.nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            List<StoryNode> nodes = new List<StoryNode>(node.storyNodes);
            for (int i = nodes.Count-1; i >= 0; i--)
            {
                if (nodes[i] is BaseGroupNode baseGroupNode)
                {
                    DeleteGroupNode(storyGraph, baseGroupNode);
                }
                else
                {
                    Undo.DestroyObjectImmediate(nodes[i]);
                }
            }
            AssetDatabase.SaveAssets();
        }
        public static void AddChild(this StoryGraph storyGraph, StoryNode parent, StoryNode child)
        {
            parent.storyGraph = storyGraph;
            child.storyGraph = storyGraph;
            StoryNode scenarioNode = parent;
            if (scenarioNode != null)
            {
                Undo.RecordObject(scenarioNode, "Story Graph (Add Child)");
                BaseGroupNode groupNode = StoryGraphEditorWindow.graphView.currentGroupNode;
                if (groupNode != null)
                {
                    scenarioNode.childrenID.Add(child.id);
                }
                else
                {
                    scenarioNode.childrenID.Add(child.id);
                }
                EditorUtility.SetDirty(scenarioNode);
            }

        }
        public static void RemoveChild(this StoryGraph storyGraph, StoryNode parent, StoryNode child)
        {
            parent.storyGraph = storyGraph;
            child.storyGraph = storyGraph;
            StoryNode scenarioNode = parent as StoryNode;
            if (scenarioNode != null)
            {
                Undo.RecordObject(scenarioNode, "Story Graph (Remove Child)");
                BaseGroupNode groupNode = StoryGraphEditorWindow.graphView.currentGroupNode;
                if (groupNode != null)
                {
                    scenarioNode.childrenID.Remove(child.id);
                }
                else
                {
                    scenarioNode.childrenID.Remove(child.id);
                }
                EditorUtility.SetDirty(scenarioNode);
            }

        }
        public static List<StoryNode> GetChildren(this StoryGraph storyGraph, StoryNode parent)
        {
            List<StoryNode> children = new List<StoryNode>();
            StoryNode scenarioNode = parent as StoryNode;
            if (scenarioNode != null)
            {
                for (int i = 0; i < scenarioNode.childrenID.Count; i++)
                {
                    children.Add(storyGraph.GetNodeByID(scenarioNode.childrenID[i]));
                }
            }
            return children;
        }

        public static string GetName(this StoryNode scenarioNode)
        {
            // Удаляем слово "Node"
            string result = scenarioNode.GetType().Name.Replace("Node", "");
            // Добавляем пробел перед первой заглавной буквой после маленьких
            result = System.Text.RegularExpressions.Regex.Replace(result, "(?<=[a-z])([A-Z])", " $1");
            return result;
        }
    }
}
