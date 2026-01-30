using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class NavigateToPointNode : NavigationNode
    {
        private Transform _point;
        [SerializeField] private float _targetDistance = 2f;
        protected override bool useUpdate => true;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(targetObjectNameID, out _point);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _agent.ResetPath();
        }
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            Vector3 targetPosition;
            targetPosition = _point.position;
            _agent.destination = targetPosition;
            while (true)
            {
                if (_agent.remainingDistance < _targetDistance)
                {
                    TransitionToNextNodes(storyGraph);
                }
                yield return null;
            }
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
    }
}
