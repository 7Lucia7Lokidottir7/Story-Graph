using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class NavigateToPointNode : NavigationNode
    {
        private Transform _point;
        [SerializeField] private float _targetDistance = 2f;
        private Vector3 _targetPosition;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(targetObjectNameID, out _point);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _agent.ResetPath();
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_agent.remainingDistance < _targetDistance)
            {
                OnTransitionToNextNode(storyGraph);
            }
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _targetPosition = _point.position;
            _agent.destination = _targetPosition;
        }
    }
}
