using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace PG.StorySystem.Nodes
{
    public class InteractionWithGazeNode : InteractNode
    {
        private XRSimpleInteractable _simpleInteractable;
        private StoryGraph _graph;



        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (_simpleInteractable.allowGazeInteraction)
            {
                _simpleInteractable.selectEntered?.RemoveListener(OnSelectedObject);
            }
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _graph = storyGraph;

            storyGraph.GetObject(objectNameID, out _simpleInteractable);
            if (_simpleInteractable.allowGazeInteraction)
            {
                _simpleInteractable.selectEntered?.AddListener(OnSelectedObject);
            }
        }
        void OnSelectedObject(SelectEnterEventArgs args)
        {
            if (!isEnded)
            {
                TransitionToNextNodes(_graph);
                if (disableInteractAfterInteract)
                {
                    _simpleInteractable.enabled = false;
                }
            }
        }
    }
}
