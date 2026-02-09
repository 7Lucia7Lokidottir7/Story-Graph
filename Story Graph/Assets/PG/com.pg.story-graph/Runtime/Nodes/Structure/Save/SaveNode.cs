using System.IO;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SaveNode : SaveGraphNodes
    {
        protected override void OnStart(StoryGraph storyGraph)
        {
            saveGraph.nodes.Clear();
            //for (int i = 0; i < storyGraph.currentNodes.Count; i++)
            //{
            //    saveGraph.nodes.Add(storyGraph.currentNodes[i].storyNode.id);
            //}
            saveGraph.nodes.Add(id);

            string json = JsonUtility.ToJson(saveGraph);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, _filename), json);

            base.OnStart(storyGraph);
        }
    }
}
