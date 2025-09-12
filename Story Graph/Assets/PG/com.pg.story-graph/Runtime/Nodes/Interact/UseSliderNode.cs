using UnityEngine;
using UnityEngine.UI;

namespace PG.StorySystem.Nodes
{
    public class UseSliderNode : InteractNode
    {
        private Slider _slider;
        private StoryGraph _graph;

        [Tooltip("This parameter determines whether it will be possible to interact with the object after the end of the node.")]
        public bool disableInteractAfterInteract;
        [HideInInspector] public float minValue = 0.25f;
        [HideInInspector] public float maxValue = 0.75f;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _slider.onValueChanged?.RemoveListener(OnValueChange);
        }
        protected override void Init(StoryGraph storyGraph)
        {
            _graph = storyGraph;

            storyGraph.GetObject(objectNameID, out _slider);
            _slider.onValueChanged.AddListener(OnValueChange);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {

        }
        protected override void OnStart(StoryGraph storyGraph)
        {
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