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
                    StoryNode storyNode = storyGraph.GetNodeByID(item, _groupNode);
                    storyNode.isStarted = false;
                    storyNode.isEnded = false;
                }
                OnNextNode(storyGraph);
                foreach (var item in childrenID)
                {
                    StoryNode storyNode = storyGraph.GetNodeByID(item, _groupNode);
                    storyNode.isStarted = false;
                    storyNode.isEnded = false;
                }
                yield return new WaitForSeconds(_time);
                
            }
        }
        public void StopTimer()
        {
            storyGraph.runner.StopCoroutine(_coroutine);
            TransitionToNextNodes(storyGraph);
        }
        public override void TransitionToNextNodes(StoryGraph storyGraph)
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
