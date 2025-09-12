using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace PG.StorySystem.Nodes
{
    public class ObjectToSocketInteractorNode : InteractNode
    {
        private XRGrabInteractable _grabInteractable;
        [HideInInspector] public string targetObjectNameID;
        public bool useTargetObjectTag;
        public string tag;

        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        private XRSocketInteractor _socketInteractor;
        private StoryGraph _graph;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (_socketInteractor != null)
            {
                _socketInteractor.selectEntered?.RemoveListener(OnSelectedObject);
            }
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _graph = storyGraph;
            storyGraph.GetObject(objectNameID, out _socketInteractor);
            if (!useTargetObjectTag)
            {
                storyGraph.GetObject(targetObjectNameID, out _grabInteractable);
            }
            if (_socketInteractor != null)
            {
                // Make sure any previous listeners are removed
                _socketInteractor.selectEntered?.AddListener(OnSelectedObject);
            }
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        void OnSelectedObject(SelectEnterEventArgs args)
        {
            // Проверяем, что событие исходит от нашего сокета
            if (args.interactorObject as XRSocketInteractor != _socketInteractor) return;

            if (!useTargetObjectTag)
            {
                // Сравниваем с заданным объектом
                if (args.interactableObject.transform == _grabInteractable.transform)
                {
                    TransitionToNextNodes(_graph);
                    if (disableInteractAfterInteract)
                    {
                        _socketInteractor.enabled = false;
                    }
                }
            }
            else
            {
                if (args.interactableObject.transform.CompareTag(tag))
                {
                    TransitionToNextNodes(_graph);
                    if (disableInteractAfterInteract)
                    {
                        _socketInteractor.enabled = false;
                    }
                }
            }
        }

    }
}