using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class UseKinemacticNode : PhysicsNode
    {
        [SerializeField] private bool _isKinematic = true;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _rigidbody.isKinematic = _isKinematic;
            OnTransitionToNextNode(storyGraph);
        }
    }
}
