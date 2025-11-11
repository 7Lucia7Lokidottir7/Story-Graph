using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.StorySystem.Nodes
{
    public class WaitPressNode : InteractNode
    {
        [SerializeField] private InputActionProperty _actionProperty;
        private InputAction _action;

        [SerializeField] private InputActionProperty[] _actionPropertyModificators;
        private InputAction[] _actions;
        protected override void Init(StoryGraph storyGraph)
        {
            _action = InputSystem.actions.FindAction(_actionProperty.reference.name);
            _actions = new InputAction[_actionPropertyModificators.Length];
            for (int i = 0; i < _actionPropertyModificators.Length; i++)
            {
                _actions[i] = InputSystem.actions.FindAction(_actionPropertyModificators[i].reference.name);
            }
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
            if (_actions.Length > 0)
            {
                foreach (var action in _actions)
                {
                    if (!action.IsPressed())
                    {
                        return;
                    }
                }
            }
            TransitionToNextNodes(storyGraph);
        }
    }
}
