﻿using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class NavigateToDynamicPointNode : NavigationNode
    {
        private Transform _point;
        [SerializeField] private float _targetDistance = 2f;
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
            _agent.destination = _point.position;
            if (_agent.remainingDistance < _targetDistance)
            {
                OnTransitionToNextNode(storyGraph);
            }
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
    }
}
