using UnityEngine;

using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace PG.StorySystem.Nodes
{
    public class UseLeverNode : InteractNode
    {
        private XRLever _lever;
        private StoryGraph _graph;


        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        [SerializeField] private bool _leverActive;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (_leverActive)
            {
                _lever.onLeverActivate?.RemoveListener(OnSelectedObject);
            }
            else
            {
                _lever.onLeverDeactivate?.RemoveListener(OnSelectedObject);
            }
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {

        }
        protected override void Init(StoryGraph storyGraph)
        {
            _graph = storyGraph;
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _lever);
            if (_leverActive)
            {
                _lever.onLeverActivate?.AddListener(OnSelectedObject);
            }
            else
            {
                _lever.onLeverDeactivate?.AddListener(OnSelectedObject);
            }
        }

        void OnSelectedObject()
        {
            TransitionToNextNodes(_graph);
            if (disableInteractAfterInteract)
            {
                _lever.enabled = false;
            }
        }
    }
}