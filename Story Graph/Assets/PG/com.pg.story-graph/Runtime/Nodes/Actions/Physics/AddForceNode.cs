using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AddForceNode : PhysicsNode
    {
        [HideInInspector] public bool useDirectionFromObject;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public float power;
        [HideInInspector] public string targetObjectNameID;
        private Transform _targetObject;
        [Tooltip("Force – Add a continuous force to the rigidbody, using its mass. \n\n" +
            "Acceleration – Add a continuous acceleration to the rigidbody, ignoring its mass. \n\n" +
            "Impulse – Add an instant force impulse to the rigidbody, using its mass. \n\n" +
            "VelocityChange – Add an instant velocity change to the rigidbody, ignoring its mass.")]public ForceMode forceMode;
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
                direction = (_targetObject.position - _rigidbody.position).normalized;
            }
            _rigidbody.AddForce(direction * power, forceMode);
            TransitionToNextNodes(storyGraph);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
