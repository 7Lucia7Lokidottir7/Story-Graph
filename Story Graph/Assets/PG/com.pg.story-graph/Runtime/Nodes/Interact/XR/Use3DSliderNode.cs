using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace PG.StorySystem.Nodes
{
    public class Use3DSliderNode : InteractNode
    {
        private XRSlider _slider;
        private StoryGraph _graph;


        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        [HideInInspector] public float minValue = 0.25f;
        [HideInInspector] public float maxValue = 0.75f;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _slider.onValueChange.RemoveListener(OnValueChange);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {

        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _graph = storyGraph;

            storyGraph.GetObject(objectNameID, out _slider);
            _slider.onValueChange.AddListener(OnValueChange);
        }
        void OnValueChange(float value)
        {
            if (value > minValue && value < maxValue)
            {
                TransitionToNextNodes(_graph);
                if (disableInteractAfterInteract)
                {
                    _slider.enabled = false;
                }
            }
        }
    }
}