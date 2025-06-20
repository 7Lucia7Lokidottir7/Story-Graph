using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.StorySystem.Nodes
{
    public class WaitPressNode : InteractNode
    {
        [SerializeField] private InputActionProperty _actionProperty;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _actionProperty.action.performed -= OnPress;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _actionProperty.action.performed += OnPress;
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        void OnPress(InputAction.CallbackContext context)
        {
            OnTransitionToNextNode(storyGraph);
        }
    }
}
