using System.Collections.Generic;
using UnityEngine;
namespace PG.StorySystem
{
    using Nodes;
    using System;
    using System.Collections;

    [CreateAssetMenu(fileName ="New Story Graph", menuName ="PG/Story Graph")]
    public sealed class StoryGraph : ScriptableObject
    {

        [HideInInspector] 
        public StoryNode rootNode;


        [HideInInspector] 
        public List<StoryNode> nodes = new List<StoryNode>();

        [HideInInspector] 
        public int nextId = 0; // Счётчик для генерации уникальных ID

        [HideInInspector] 
        public List<int> nodesID = new List<int>();
        [HideInInspector] 
        public List<StoryNode> currentNodes = new List<StoryNode>();
        [HideInInspector] public StoryGraphRunner runner;

        [HideInInspector]
        public List<string> objects = new List<string>();
        [HideInInspector]
        public List<StoryVariable> variables = new List<StoryVariable>();
        public Dictionary<string, StoryVariable> variablesDictionary = new Dictionary<string, StoryVariable>();


        public System.Action initialized;
        public void StopCoroutine(Coroutine routine)
        {
            runner.StopCoroutine(routine);
        }
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return runner.StartCoroutine(routine);
        }
        public Coroutine StartCoroutine(string routine, object value = null)
        {
            return runner.StartCoroutine(routine, value);
        }

        [ContextMenu("Reset Nodes")]
        void ResetNodesState()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].isStarted = false;
                nodes[i].isEnded = false;
            }
        }
        public void Initialize()
        {
            for (int i = 0; i < variables.Count; i++)
            {
                variablesDictionary.Add(variables[i].variableName, variables[i]);
            }
            currentNodes.Add(rootNode);
            rootNode.StartNode(this);
        }
        public void RemoveCurrentNode(StoryNode storyNode)
        {
            if (currentNodes.Contains(storyNode))
            {
                currentNodes.Remove(storyNode);
            }
        }
        public void OnUpdate()
        {
            if (currentNodes.Count == 0) return;

            for (int i = 0; i < currentNodes.Count; i++)
            {
                var currentNode = currentNodes[i];
                if (currentNode == null)
                {
                    continue;
                }
                if (currentNode.isStarted && !currentNode.isEnded)
                {
                    currentNode.StandardUpdate(this);
                }
            }
        }

        public void RestartAllNodes()
        {
            foreach (StoryNode storyNode in currentNodes)
            {
                if (storyNode is BaseGroupNode baseGroupNode)
                {
                    foreach (StoryNode subNode in baseGroupNode.currentNodes)
                    {
                        if (!subNode.reinitNodeOnStart)
                        {
                            continue;
                        }
                        StartNode(subNode);
                    }
                }

                if (!storyNode.reinitNodeOnStart)
                {
                    continue;
                }
                StartNode(storyNode);
            }
        }

        public void StartNode(StoryNode storyNode)
        {
            storyNode.isStarted = false;
            storyNode.isEnded = false;
            storyNode.StartNode(this, storyNode.groupNode);
        }


        #region Objects
        public void GetObject<T>(int objectID, out T type)
        {
            runner.gameObjects.TryGetValue(objects[objectID], out GameObject gameObject);
            gameObject.TryGetComponent(out type);
        }
        public void GetObject<T>(string objectID, out T type)
        {
            runner.gameObjects.TryGetValue(objectID, out GameObject gameObject);
            gameObject.TryGetComponent(out type);
        }
        #endregion

        #region Variables
        public StoryVariable GetVariable(string variableName)
        {
            runner.variables.TryGetValue(variableName, out StoryVariable variable);
            return variable;
        }
        public object GetVariableValue(string variableName)
        {
            runner.variables.TryGetValue(variableName, out StoryVariable variable);
            return variable.GetValue();
        }
        public void SetVariableValue(string variableName, object value)
        {
            runner.variables.TryGetValue(variableName, out StoryVariable variable);
            variable.SetValue(value);
        }
        #endregion

        public StoryGraph CloneGraph()
        {
            StoryGraph graph = Instantiate(this);

            Dictionary<StoryNode, StoryNode> nodeMap = new Dictionary<StoryNode, StoryNode>();

            // Создаем копии всех узлов и добавляем их в nodeMap
            List<StoryNode> storyNodes = nodes.ConvertAll(node =>
            {
                var newNode = Instantiate(node);
                nodeMap[node] = newNode;
                nodeMap[node].baseNode = node;
                return newNode;
            });

            graph.nodes = storyNodes;

            // Обновляем связи между узлами
            for (int i = 0; i < storyNodes.Count; i++)
            {
                var currentNode = storyNodes[i];
                if (currentNode is BaseGroupNode groupNode)
                {
                    List<StoryNode> subStoryNodes = groupNode.storyNodes.ConvertAll(node =>
                    {
                        var subNode = Instantiate(node);
                        nodeMap[node] = subNode;
                        nodeMap[node].baseNode = node;
                        return subNode;
                    });

                    groupNode.storyNodes = subStoryNodes;
                }
            }

            // Устанавливаем rootNode и currentNodes
            if (nodeMap.TryGetValue(rootNode, out var newRootNode))
            {
                graph.rootNode = newRootNode;
            }
            graph.currentNodes = currentNodes.ConvertAll(node => nodeMap.ContainsKey(node) ? nodeMap[node] : Instantiate(node));

            return graph;
        }


        #region Find Nodes
        public StoryNode GetNodeByID(int id, BaseGroupNode groupNode = null)
        {
            var list = groupNode != null ? groupNode.storyNodes : nodes;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                    i--;
                    continue;
                }
                if (list[i].id == id)
                {
                    return list[i];
                }
            }
            return null;
        }

        public StoryNode FindCurrentNode(string nameNode)
        {
            for (int i = 0; i < currentNodes.Count; i++)
            {
                if (currentNodes[i].nameNode == nameNode)
                {
                    return currentNodes[i];
                }

                if (currentNodes[i] is BaseGroupNode groupNode)
                {
                    var foundNode = groupNode.FindCurrentNode(nameNode);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }

            Debug.LogWarning($"Current node with name '{nameNode}' not found.");
            return null;
        }

        public StoryNode FindNode(string nameNode)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].nameNode == nameNode)
                {
                    return nodes[i];
                }

                if (nodes[i] is BaseGroupNode groupNode)
                {
                    var foundNode = groupNode.FindNode(nameNode);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }

            Debug.LogWarning($"Node with name '{nameNode}' not found in the graph.");
            return null;
        }
        #endregion
    }
}
