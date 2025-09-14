using System.IO;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class LoadNode : SaveGraphNodes
    {
        protected override void OnStart(StoryGraph storyGraph)
        {
            for (int i = 0; i < storyGraph.currentNodes.Count; i++)
            {
                saveGraph.nodes.Add(storyGraph.currentNodes[i].id);
            }

            string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, _filename));
            JsonUtility.FromJsonOverwrite(json, saveGraph);

            for (int i = 0; i < storyGraph.currentNodes.Count; i++)
            {
                storyGraph.currentNodes[i].EndNode(storyGraph);
            }

            for (int i = 0; i < saveGraph.nodes.Count; i++)
            {
                StoryNode storyNode = storyGraph.FindNode(saveGraph.nodes[i]);
                storyNode.StartNode(storyGraph, storyNode.groupNode);
            }

            base.OnStart(storyGraph);
        }
    }
}
