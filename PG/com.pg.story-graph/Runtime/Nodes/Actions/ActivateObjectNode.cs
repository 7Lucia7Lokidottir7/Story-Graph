using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class ActivateObjectNode : ActionNode
    {
        private Transform _transform;

        public override string classGUI => "activate-object";
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            if (_transform != null)
            {
                _transform.gameObject.SetActive(true);
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
