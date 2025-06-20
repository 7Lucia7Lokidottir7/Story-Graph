using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class StopAnythingNode : StructureNode
    {
        [SerializeField] private string _targetNameNode;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            StoryNode scenarioNode = storyGraph.FindCurrentNode(_targetNameNode);
            if (scenarioNode != null)
            {
                scenarioNode.End(storyGraph);
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

    }
}
