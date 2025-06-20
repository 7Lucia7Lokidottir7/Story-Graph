using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ReturnNode : StructureNode
    {
        public override string classGUI => "group";
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _groupNode?.EndGroup(storyGraph);
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
