using UnityEngine;
using UnityEngine.XR.Content.Interaction;

namespace PG.StorySystem.Nodes
{
    public class PressOnButtonObjectNode : InteractNode
    {

        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        private XRPushButton _pushButton;
        private StoryGraph _graph;

        protected override void OnEnd(StoryGraph storyGraph)
        {
            _pushButton.onPress?.RemoveListener(OnSelectedObject);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {

        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _graph = storyGraph;

            storyGraph.GetObject(objectNameID, out _pushButton);
            _pushButton.onPress?.AddListener(OnSelectedObject);
        }
        void OnSelectedObject()
        {
            TransitionToNextNodes(_graph);
            if (disableInteractAfterInteract)
            {
                _pushButton.enabled = false;
            }
        }
    }
}