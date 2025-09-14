using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class CheckDistanceObjectsConditionNode : ConditionNode
    {
        [HideInInspector]
        [StoryGraphDropdown("objects")]
        public string objectNameID;
        
        [HideInInspector] 
        [StoryGraphDropdown("objects")]
        public string targetObjectNameID;
        
        [HideInInspector] public Vector3 targetVector;
        [SerializeField] private float _targetDistance;

        private enum CheckType { Less, Greater };
        [SerializeField] private CheckType _checkType;
        [HideInInspector] public bool useTargetObject;
        private Transform _transform;
        private Transform _targetTransform;
        public override bool GetResult()
        {
            bool result = false;
            if (useTargetObject)
            {
                switch (_checkType)
                {
                    case CheckType.Less:
                        result = Vector3.Distance(_transform.position, _targetTransform.position) < _targetDistance;
                        break;
                    case CheckType.Greater:
                        result = Vector3.Distance(_transform.position, _targetTransform.position) > _targetDistance;
                        break;
                }
                //Debug.Log($"Distance {Vector3.Distance(_transform.position, _targetTransform.position)} \n Target {_targetDistance}");
            }
            else
            {
                switch (_checkType)
                {
                    case CheckType.Less:
                        result = Vector3.Distance(_transform.position, targetVector) < _targetDistance;
                        break;
                    case CheckType.Greater:
                        result = Vector3.Distance(_transform.position, targetVector) > _targetDistance;
                        break;
                }
                //Debug.Log($"Distance {Vector3.Distance(_transform.position, targetVector)} \n Target {_targetDistance}");
            }
            return result;
        }
        public override void InitializeCondition()
        {
            storyGraph.GetObject(objectNameID, out _transform);
            if (useTargetObject)
            {
                storyGraph.GetObject(targetObjectNameID, out _targetTransform);
            }
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {

        }

        protected override void OnStart(StoryGraph storyGraph)
        {
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
