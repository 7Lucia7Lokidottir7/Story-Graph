
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class RootNode : StructureNode
    {
        public override string classGUI => "root";
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
        }
    }

}