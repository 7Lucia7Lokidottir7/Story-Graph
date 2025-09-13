using UnityEngine;
namespace PG.StorySystem
{
    using Nodes;
    using UnityEngine.XR.Interaction.Toolkit.Interactables;

    public class ObjectGrabbableEnableNode : InteractNode
    {
        private XRBaseInteractable _controller;
        [SerializeField] private bool _enable = true;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _controller);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _controller.enabled = _enable;
            TransitionToNextNodes(storyGraph);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}