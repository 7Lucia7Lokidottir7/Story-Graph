using UnityEngine;
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    public class CheckDistanceObjectsNode : ActionNode
    {
        [HideInInspector]public int targetObjectID;
        [HideInInspector] public string targetObjectNameID;
        private Transform _targetTransform;
        private Transform _transform;
        private enum CheckType
        {
            Less, Greater
        }
        [SerializeField] private CheckType _checkType;
        [SerializeField] private float _targetDistance;

        protected override void OnEnd(StoryGraph storyGraph)
        {

        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            storyGraph.GetObject(targetObjectNameID, out _targetTransform);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_targetTransform != null && _transform != null)
            {
                switch (_checkType)
                {
                    case CheckType.Less:
                        if (Vector3.Distance(_transform.position, _targetTransform.position) < _targetDistance)
                        {
                            TransitionToNextNodes(storyGraph);
                        }
                        break;
                    case CheckType.Greater:
                        if (Vector3.Distance(_transform.position, _targetTransform.position) > _targetDistance)
                        {
                            TransitionToNextNodes(storyGraph);
                        }
                        break;
                }
            }
        }
    }
}