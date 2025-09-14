using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class SeparatorNode : StoryNode
    {
        [HideInInspector]
        public int previousNodesCount;
        public override Color colorNode => Color.clear;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        public override void OnDublicate()
        {
            base.OnDublicate();
            previousNodesCount = 0;
        }
    }
}

