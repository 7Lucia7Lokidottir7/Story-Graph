using System;
using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class BaseGroupNode : StructureNode
    {
        [HideInInspector]
        public RootNode rootNode;

        [HideInInspector]
        public ReturnNode returnNode;

        public string nameGroup;
        [HideInInspector]
        public List<StoryNode> storyNodes = new List<StoryNode>(); 
        public Color color = Color.yellowNice;

        public override Color colorNode => color;
        public override void RestartNode(StoryGraph storyGraph)
        {
            base.RestartNode(storyGraph);
            if (rootNode != null)
            {
                rootNode.state = new NodeState();
            }
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (rootNode != null)
            {
                rootNode = Instantiate(rootNode);
                rootNode.StartNode(storyGraph, this);
                state.currentNodes.Add(rootNode.state);
            }

        }
        public void EndGroup(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
        }
        public StoryNode FindCurrentNode(string nameNode)
        {
            for (int i = 0; i < state.currentNodes.Count; i++)
            {
                if (state.currentNodes[i].storyNode.nameNode == nameNode)
                {
                    return state.currentNodes[i].storyNode;
                }

                if (state.currentNodes[i].storyNode is BaseGroupNode groupNode)
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
            for (int i = 0; i < storyNodes.Count; i++)
            {
                if (storyNodes[i].nameNode == nameNode)
                {
                    return storyNodes[i];
                }

                if (storyNodes[i] is BaseGroupNode groupNode)
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
            for (int i = 0; i < storyNodes.Count; i++)
            {
                if (storyNodes[i].id == idNode)
                {
                    return storyNodes[i];
                }

                if (storyNodes[i] is BaseGroupNode groupNode)
                {
                    var foundNode = groupNode.FindNode(idNode);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }

            Debug.LogWarning($"Node with name '{nameNode}' not found in the graph.");
            return null;
        }

    }
}
