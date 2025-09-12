using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class AlignByInternalPointsNode : TransformNode
    {
        private Transform _transform;
        private Transform _transformPoint;
        [HideInInspector] public string aligmentObjectNameID;
        private Transform _transformAlignmentPoint;
        [SerializeField] private bool _resetLocalRotationBeforeAlign;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            storyGraph.GetObject(targetObjectNameID, out _transformPoint);
            storyGraph.GetObject(aligmentObjectNameID, out _transformAlignmentPoint);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_transform != null && _transformPoint != null && _transformAlignmentPoint != null)
            {
                if (_resetLocalRotationBeforeAlign)
                {
                    _transform.localRotation = Quaternion.identity; 
                }
                Vector3 offset = _transformAlignmentPoint.position - _transformPoint.position;
                _transform.position += offset;
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
