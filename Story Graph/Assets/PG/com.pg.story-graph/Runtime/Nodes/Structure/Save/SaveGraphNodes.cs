using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class SaveGraphNodes : StructureNode
    {
        [SerializeField] protected string _filename = "Graph.json";
        [HideInInspector] public SaveGraph saveGraph = new SaveGraph();

        protected override void OnStart(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
        }

        [System.Serializable]
        public class SaveGraph
        {
            public List<int> nodes = new List<int>();
        }
    }
}
