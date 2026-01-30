using System.Collections.Generic;
using UnityEngine;
namespace PG.StorySystem
{
    using Nodes;
    using System.Collections;
    using static PG.StorySystem.Nodes.StoryNode;

    [CreateAssetMenu(fileName ="New Story Graph", menuName ="PG/Story Graph")]
    public sealed class StoryGraph : ScriptableObject
    {

        [HideInInspector] 
        public StoryNode rootNode;


        //[HideInInspector] 
        public List<StoryNode> nodes = new List<StoryNode>();

        [HideInInspector] 
        public int nextId = 0; // —чЄтчик дл€ генерации уникальных ID

        [HideInInspector] 
        public List<int> nodesID = new List<int>();
        [HideInInspector] 
        public List<NodeState> currentNodes = new List<NodeState>();
        [HideInInspector] public StoryGraphRunner runner;


        public List<string> objects = new List<string>();


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
                if (nodes[i] is BaseGroupNode baseGroupNode)
                {
                    for (int a = 0; a < baseGroupNode.state.currentNodes.Count; a++)
                    {
                        baseGroupNode.state.currentNodes[a] = new NodeState();
                    }
                }
                nodes[i].state = new NodeState();
            }
        }
        public void Initialize()
        {
            currentNodes.Add(rootNode.CreateState());
            rootNode.StartNode(this);
        }
        public void RemoveCurrentNode(StoryNode storyNode)
        {
            if (currentNodes.Contains(storyNode.state))
            {
                currentNodes.Remove(storyNode.state);
            }
        }

        public void RestartAllNodes()
        {
            
            foreach (NodeState storyNode in currentNodes)
            {
                if (storyNode.storyNode is BaseGroupNode baseGroupNode)
                {
                    foreach (NodeState subNode in baseGroupNode.state.currentNodes)
                    {
                        if (!subNode.storyNode.reinitNodeOnStart)
                        {
                            continue;
                        }
                        StartNode(subNode.storyNode);
                    }
                }

                if (!storyNode.storyNode.reinitNodeOnStart)
                {
                    continue;
                }
                StartNode(storyNode.storyNode);
            }
        }

        public void StartNode(StoryNode storyNode)
        {
            storyNode.state = new NodeState();
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
                if (currentNodes[i].storyNode.nameNode == nameNode)
                {
                    return currentNodes[i].storyNode;
                }

                if (currentNodes[i].storyNode is BaseGroupNode groupNode)
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

        public StoryNode FindNode(int idNode)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].id == idNode)
                {
                    return nodes[i];
                }

                if (nodes[i] is BaseGroupNode groupNode)
                {
                    var foundNode = groupNode.FindNode(idNode);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }

            Debug.LogWarning($"Node with id '{idNode}' not found in the graph.");
            return null;
        }
        #endregion
    }
}
