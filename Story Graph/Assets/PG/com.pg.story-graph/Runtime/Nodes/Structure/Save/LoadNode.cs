using System.IO;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class LoadNode : SaveGraphNodes
    {
        protected override void OnStart(StoryGraph storyGraph)
        {
            string path = Path.Combine(Application.persistentDataPath, _filename);

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(json, saveGraph);


                for (int i = 0; i < storyGraph.currentNodes.Count; i++)
                {
                    storyGraph.currentNodes[i].storyNode.EndNode(storyGraph);
                }

                for (int i = 0; i < saveGraph.nodes.Count; i++)
                {
                    StoryNode storyNode = storyGraph.FindNode(saveGraph.nodes[i]);
                    if (storyNode is SaveNode saveNode)
                    {
                        saveNode.TransitionToNextNodes(storyGraph);
                    }
                    else
                    {
                        storyNode.StartNode(storyGraph, storyNode.groupNode);
                    }
                }
                EndNode(storyGraph);
            }
            else
            {
                base.OnStart(storyGraph);
            }

        }
    }
}
