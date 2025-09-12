using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class StopAnythingNode : StructureNode
    {
        [SerializeField] private string[] _targetNameNodes;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            foreach (var node in _targetNameNodes)
            {
                StoryNode storyNode = storyGraph.FindCurrentNode(node);
                if (storyNode != null)
                {
                    storyNode.EndNode(storyGraph);
                }
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

    }
}
