
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class RootNode : StructureNode
    {
        public override Color colorNode => Color.white;

        protected override void OnStart(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
        }
    }

}