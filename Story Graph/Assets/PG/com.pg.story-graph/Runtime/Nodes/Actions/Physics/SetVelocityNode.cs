using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetVelocityNode : PhysicsNode
    {
        [HideInInspector] public bool useDirectionFromObject;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public float speed;
        [HideInInspector] public int targetObjectID;
        [HideInInspector] public string targetObjectNameID;
        private Transform _targetObject;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(targetObjectNameID, out _targetObject);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (useDirectionFromObject)
            {
                velocity = (_targetObject.position - _rigidbody.position).normalized * speed;
            }
            _rigidbody.linearVelocity = velocity;
            TransitionToNextNodes(storyGraph);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
