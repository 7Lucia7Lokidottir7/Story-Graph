using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class NavigateToDynamicPointNode : NavigationNode
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
            while (_agent.remainingDistance > _targetDistance)
            {
                _agent.destination = _point.position;
                yield return null;
            }
            TransitionToNextNodes(storyGraph);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
    }
}
