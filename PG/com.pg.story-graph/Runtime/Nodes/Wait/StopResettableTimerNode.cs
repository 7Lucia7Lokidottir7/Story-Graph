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
            OnTransitionToNextNode(storyGraph);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

        protected override IEnumerator Delay(StoryGraph storyGraph)
        {
            yield return null;
        }
    }
}
