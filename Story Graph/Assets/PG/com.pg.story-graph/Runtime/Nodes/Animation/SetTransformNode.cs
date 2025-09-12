using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetTransformNode : TransformNode
    {
        private Transform _transform;

        private Transform _targetTransform;

        public bool useMovement;
        public bool useRotation;
        public bool useScale;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            storyGraph.GetObject(targetObjectNameID, out _targetTransform);
            if (useMovement)
            {
                _transform.position = _targetTransform.position;
            }
            if (useRotation)
            {
                _transform.rotation = _targetTransform.rotation;
            }
            if (useScale)
            {
                _transform.localScale = _targetTransform.lossyScale;
            }



            TransitionToNextNodes(storyGraph);
        }
    }
}