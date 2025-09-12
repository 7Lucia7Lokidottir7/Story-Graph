using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class UseGravityNode : PhysicsNode
    {
        [SerializeField] private bool _useGravity = true;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _rigidbody.useGravity = _useGravity;
            TransitionToNextNodes(storyGraph);
        }
    }
}
