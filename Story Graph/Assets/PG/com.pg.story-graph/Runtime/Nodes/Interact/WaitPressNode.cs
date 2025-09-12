using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.StorySystem.Nodes
{
    public class WaitPressNode : InteractNode
    {
        [SerializeField] private InputActionProperty _actionProperty;
        private InputAction _action;
        protected override void Init(StoryGraph storyGraph)
        {
            _action = InputSystem.actions.FindAction(_actionProperty.reference.name);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _action.performed -= OnPress;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _action.performed += OnPress;
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        void OnPress(InputAction.CallbackContext context)
        {
            TransitionToNextNodes(storyGraph);
        }
    }
}
