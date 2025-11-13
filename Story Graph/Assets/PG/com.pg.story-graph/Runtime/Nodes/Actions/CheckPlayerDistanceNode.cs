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

        [NodeDataTitle]
        [SerializeField] private float _targetDistance;
        protected override void OnEnd(StoryGraph storyGraph)
        {

        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            Transform transform = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
            transform.TryGetComponent(out _characterController);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_characterController != null && _transform != null)
            {
                switch (_checkType)
                {
                    case CheckType.Less:
                        if (Vector3.Distance(_transform.position, _characterController.transform.position) < _targetDistance)
                        {
                            TransitionToNextNodes(storyGraph);
                        }
                        break;
                    case CheckType.Greater:
                        if (Vector3.Distance(_transform.position, _characterController.transform.position) > _targetDistance)
                        {
                            TransitionToNextNodes(storyGraph);
                        }
                        break;
                }
            }
        }
    }
}