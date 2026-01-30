using UnityEngine;
using PG.StorySystem.Nodes;
using System.Collections;
namespace PG.StorySystem
{
    public class CheckDistanceObjectsNode : ActionNode
    {
        [HideInInspector]
        [InspectorLabel("Target Object")]
        [StoryGraphDropdown("objects")]
        public string targetObjectNameID;
        private Transform _targetTransform;
        private Transform _transform;
        public override Color colorNode => Color.aquamarine;
        private enum CheckType
        {
            Less, Greater
        }
        [SerializeField] private CheckType _checkType;
        [SerializeField] private float _targetDistance;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            storyGraph.GetObject(targetObjectNameID, out _targetTransform);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            while (true)
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
                yield return null;
            }
        }
    }
}