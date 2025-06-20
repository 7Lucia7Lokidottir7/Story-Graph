using UnityEngine;
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    public class CheckPlayerDistanceNode : ActionNode
    {
        private const string PLAYER_TAG = "Player";
        private Transform _transform;
        private CharacterController _characterController;
        private enum CheckType
        {
            Less, Greater
        }
        [SerializeField] private CheckType _checkType;
        [SerializeField] private float _targetDistance;
        private Camera _camera;
        private Vector3 _playerPosition => new Vector3(_camera.transform.position.x, _characterController.center.y - _characterController.height / 2, _camera.transform.position.z);
        protected override void OnEnd(StoryGraph storyGraph)
        {

        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            Transform transform = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
            _camera = transform.GetComponentInChildren<Camera>();
            transform.TryGetComponent(out _characterController);

            
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_characterController != null && _transform != null)
            {
                switch (_checkType)
                {
                    case CheckType.Less:
                        if (Vector3.Distance(_transform.position, _playerPosition) < _targetDistance)
                        {
                            OnTransitionToNextNode(storyGraph);
                        }
                        break;
                    case CheckType.Greater:
                        if (Vector3.Distance(_transform.position, _playerPosition) > _targetDistance)
                        {
                            OnTransitionToNextNode(storyGraph);
                        }
                        break;
                }
            }
        }
    }
}