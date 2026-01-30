using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class OrMergeReinitializeNode : MergeNode
    {
        [SerializeField] private string[] _targetNodes;
        protected override void OnEnd(StoryGraph scenarioGraph)
        {
        }

        protected override void OnStart(StoryGraph scenarioGraph)
        {
            foreach (var node in _targetNodes)
            {
                if (scenarioGraph.FindNode(node) is OrMergeNode orMergeNode)
                {
                    orMergeNode.isFinished = false;
                }
            }
            TransitionToNextNodes(scenarioGraph);
        }
    }
}
