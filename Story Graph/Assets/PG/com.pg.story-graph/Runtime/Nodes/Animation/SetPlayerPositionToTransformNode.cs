using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetPlayerPositionToTransformNode : ActionNode
    {
        private Transform _transform;
        private Transform _playerTransform;

        public bool useMovement;
        public bool useRotation;
        public bool useScale;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            if (useMovement)
            {
                _playerTransform.position = _transform.position;
            }
            if (useRotation)
            {
                _playerTransform.rotation = _transform.rotation;
            }
            if (useScale)
            {
                _playerTransform.localScale = _transform.lossyScale;
            }



            TransitionToNextNodes(storyGraph);
        }
    }
}