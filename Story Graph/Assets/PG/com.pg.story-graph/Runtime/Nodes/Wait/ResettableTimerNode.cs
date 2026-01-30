using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ResettableTimerNode : WaitNode
    {
        [SerializeField] private float _time = 10f;
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            while (true)
            {
                foreach (var item in childrenID)
                {
                    StoryNode storyNode = storyGraph.GetNodeByID(item, _groupNode);
                    storyNode.state = new NodeState();
                }
                OnNextNode(storyGraph);
                foreach (var item in childrenID)
                {
                    StoryNode storyNode = storyGraph.GetNodeByID(item, _groupNode);
                    storyNode.state = new NodeState();
                }
                yield return new WaitForSeconds(_time);
                
            }
        }
        public void StopTimer()
        {
            TransitionToNextNodes(storyGraph);
        }
        public override void TransitionToNextNodes(StoryGraph storyGraph)
        {
            if (_groupNode != null)
            {
                if (!_groupNode.state.currentNodes.Contains(state)) return;

                _groupNode.state.currentNodes.Remove(state);
            }
            else
            {
                if (!storyGraph.currentNodes.Contains(state)) return;

                storyGraph.currentNodes.Remove(state);
            }
        }
    }
}
