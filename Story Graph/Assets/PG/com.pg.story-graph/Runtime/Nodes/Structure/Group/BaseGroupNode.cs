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
        public List<StoryNode> storyNodes = new List<StoryNode>(), 
            currentNodes = new List<StoryNode>();

        public override Color colorNode => Color.yellowNice;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        public override void RestartNode(StoryGraph storyGraph)
        {
            base.RestartNode(storyGraph);
            if (rootNode != null)
            {
                rootNode.isStarted = false;
                rootNode.isEnded = false;
            }
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (rootNode != null)
            {
                rootNode = Instantiate(rootNode);
                rootNode.StartNode(storyGraph, this);
                currentNodes.Add(rootNode);
            }

        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            for (int i = 0; i < currentNodes.Count; i++)
            {
                if (currentNodes[i] != null)
                {
                    var currentNode = currentNodes[i];
                    if (currentNode.isStarted && !currentNode.isEnded)
                    {
                        currentNode.StandardUpdate(storyGraph);
                    }
                }
            }
        }
        public void EndGroup(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
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
