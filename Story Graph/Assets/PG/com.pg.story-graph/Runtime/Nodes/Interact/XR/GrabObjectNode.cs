using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace PG.StorySystem.Nodes
{
    public class GrabObjectNode : InteractNode
    {
        private XRGrabInteractable _grabInteractable;
        private StoryGraph _graph;


        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _grabInteractable.selectEntered?.RemoveListener(OnSelectedObject);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _graph = storyGraph;

            storyGraph.GetObject(objectNameID, out _grabInteractable);
            _grabInteractable.selectEntered?.AddListener(OnSelectedObject);


        }
        void OnSelectedObject(SelectEnterEventArgs args)
        {
            TransitionToNextNodes(_graph);
            if (disableInteractAfterInteract)
            {
                _grabInteractable.enabled = false;
            }
        }
    }
}
