using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class WaitActiveObjectNode : ActionNode
    {
        private Transform _transform;
        [SerializeField] private bool _activeValue;
        public override string classGUI => "activate-object";
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_transform == null)
            {
                TransitionToNextNodes(storyGraph);
            }
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_transform != null)
            {
                if (_transform.gameObject.activeInHierarchy == _activeValue)
                {
                    TransitionToNextNodes(storyGraph);
                }
            }
        }
    }
}
