using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class RemoveRigidbodyComponentNode : PhysicsNode
    {

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_rigidbody != null)
            {
                Destroy(_rigidbody);
            }
            TransitionToNextNodes(storyGraph);
        }
    }
}
