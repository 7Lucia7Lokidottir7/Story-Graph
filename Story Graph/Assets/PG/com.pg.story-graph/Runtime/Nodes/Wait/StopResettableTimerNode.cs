using System.Collections;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class StopResettableTimerNode : WaitNode
    {
        [SerializeField] private string _targetNameNode;
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (storyGraph.FindCurrentNode(_targetNameNode) is ResettableTimerNode resettableTimer)
            {
                resettableTimer.StopTimer();
            }
            TransitionToNextNodes(storyGraph);
        }
    }
}
