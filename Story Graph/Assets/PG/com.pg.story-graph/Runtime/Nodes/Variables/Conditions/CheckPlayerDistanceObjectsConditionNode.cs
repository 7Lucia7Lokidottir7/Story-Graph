using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class CheckPlayerDistanceObjectsConditionNode : ConditionNode
    {
        private const string PLAYER_TAG = "Player";
        [HideInInspector] public string objectNameID;
        [HideInInspector] public Vector3 targetVector;
        [SerializeField] private float _targetDistance;

        private enum CheckType { Less, Greater };
        [SerializeField] private CheckType _checkType;
        [HideInInspector] public bool useTargetObject;
        private Camera _camera;
        private Vector3 _playerPosition =>  new Vector3(_camera.transform.position.x, _characterController.center.y - _characterController.height / 2, _camera.transform.position.z);

        private CharacterController _characterController;
        private Transform _targetTransform;
        public override bool GetResult()
        {
            bool result = false;
            if (useTargetObject)
            {
                switch (_checkType)
                {
                    case CheckType.Less:
                        result = Vector3.Distance(_playerPosition, _targetTransform.position) < _targetDistance;
                        break;
                    case CheckType.Greater:
                        result = Vector3.Distance(_playerPosition, _targetTransform.position) > _targetDistance;
                        break;
                }
                //Debug.Log($"Distance {Vector3.Distance(_transform.position, _targetTransform.position)} \n Target {_targetDistance}");
            }
            else
            {
                switch (_checkType)
                {
                    case CheckType.Less:
                        result = Vector3.Distance(_playerPosition, targetVector) < _targetDistance;
                        break;
                    case CheckType.Greater:
                        result = Vector3.Distance(_playerPosition, targetVector) > _targetDistance;
                        break;
                }
                //Debug.Log($"Distance {Vector3.Distance(_transform.position, targetVector)} \n Target {_targetDistance}");
            }
            return result;
        }
        public override void InitializeCondition()
        {
            Transform transform = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
            _camera = transform.GetComponentInChildren<Camera>();
            transform.TryGetComponent(out _characterController);
            if (useTargetObject)
            {
                storyGraph.GetObject(objectNameID, out _targetTransform);
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
