using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ReturnNode : StructureNode
    {
        public override Color colorNode => Color.yellowNice;

        protected override void OnStart(StoryGraph storyGraph)
        {
            _groupNode?.EndGroup(storyGraph);
            TransitionToNextNodes(storyGraph);
        }
    }
}
