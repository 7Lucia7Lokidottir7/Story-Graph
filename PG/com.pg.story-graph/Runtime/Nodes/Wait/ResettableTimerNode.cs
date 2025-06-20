using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ResettableTimerNode : WaitNode
    {
        [SerializeField] private float _time = 10f;
        protected override IEnumerator Delay(StoryGraph storyGraph)
        {
            while (true)
            {
                foreach (var item in childrenID)
                {
                    StoryNode scenarioNode = storyGraph.GetNodeByID(item, _groupNode);
                    scenarioNode.isStarted = false;
                    scenarioNode.isEnded = false;
                }
                OnNextNode(storyGraph);
                foreach (var item in childrenID)
                {
                    StoryNode scenarioNode = storyGraph.GetNodeByID(item, _groupNode);
                    scenarioNode.isStarted = false;
                    scenarioNode.isEnded = false;
                }
                yield return new WaitForSeconds(_time);
                
            }
        }
        public void StopTimer()
        {
            storyGraph.runner.StopCoroutine(_coroutine);
            OnTransitionToNextNode(storyGraph);
        }
        public override void OnTransitionToNextNode(StoryGraph storyGraph)
        {
            isEnded = false;
            isStarted = false;
            if (_groupNode != null)
            {
                if (!_groupNode.currentNodes.Contains(this)) return;

                _groupNode.currentNodes.Remove(this);
            }
            else
            {
                if (!storyGraph.currentNodes.Contains(this)) return;

                storyGraph.currentNodes.Remove(this);
            }
        }
    }
}
