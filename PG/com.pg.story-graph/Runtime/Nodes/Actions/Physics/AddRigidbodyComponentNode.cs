using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AddRigidbodyComponentNode : PhysicsNode
    {
        protected Transform _transform;
        [SerializeField] private float _mass = 1f;
        [SerializeField] private float _damping;
        [SerializeField] private float _angularDamping = 0.05f;
        [SerializeField] private bool _useGravity;
        [SerializeField] private bool _isKinematic;
        [SerializeField] private RigidbodyInterpolation _interpolation;
        [SerializeField] private CollisionDetectionMode _collisionDetectionMode;
        [SerializeField] private RigidbodyConstraints _rigidbodyConstraints;


        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_transform != null && !_transform.TryGetComponent(out Rigidbody component))
            {
                Rigidbody rigidbody = _transform.gameObject.AddComponent<Rigidbody>();
                rigidbody.mass = _mass;
                rigidbody.linearDamping = _damping;
                rigidbody.angularDamping = _angularDamping;
                rigidbody.useGravity = _useGravity;
                rigidbody.isKinematic = _isKinematic;
                rigidbody.interpolation = _interpolation;
                rigidbody.collisionDetectionMode = _collisionDetectionMode;
                rigidbody.constraints = _rigidbodyConstraints;
            }
            OnTransitionToNextNode(storyGraph);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
