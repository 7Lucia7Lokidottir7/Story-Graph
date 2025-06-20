using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class PhysicsNode : ActionNode
    {
        public override string classGUI => "physics";
        protected Rigidbody _rigidbody;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _rigidbody);
        }
    }
}
