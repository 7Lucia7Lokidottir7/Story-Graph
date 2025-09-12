using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace PG.StorySystem.Nodes
{
    public class ClickOnObjectNode : InteractNode
    {
        private XRSimpleInteractable _simpleInteractable;
        private StoryGraph _graph;

        public bool useTags;
        public string tag;
        private NearFarInteractor[] _nearFarInteractors;

        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (useTags)
            {
                foreach (var item in _nearFarInteractors)
                {
                    item.selectEntered?.RemoveListener(OnSelectedObject);
                }
            }
            else
            {
                if (_simpleInteractable != null)
                {
                    _simpleInteractable.selectEntered?.RemoveListener(OnSelectedObject);
                }
            }
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _graph = storyGraph;
            if (useTags)
            {
                _nearFarInteractors = FindObjectsByType<NearFarInteractor>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
                foreach (var item in _nearFarInteractors)
                {
                    item.selectEntered?.AddListener(OnSelectedObject);
                }
            }
            else
            {
                storyGraph.GetObject(objectNameID, out _simpleInteractable);
                _simpleInteractable.selectEntered?.AddListener(OnSelectedObject);
            }
        }
        void OnSelectedObject(SelectEnterEventArgs args)
        {
            if (useTags)
            {
                if (!args.interactableObject.transform.CompareTag(tag))
                {
                    return;
                }
            }
            else
            {
                if (args.interactableObject.transform != _simpleInteractable.transform)
                {
                    return;
                }
            }
            if (!isEnded)
            {
                TransitionToNextNodes(_graph);
                if (disableInteractAfterInteract)
                {
                    if (useTags)
                    {
                        args.interactableObject.transform.GetComponent<XRSimpleInteractable>().enabled = false;
                    }
                    else
                    {
                        _simpleInteractable.enabled = false;
                    }
                }
            }
        }
    }
}
