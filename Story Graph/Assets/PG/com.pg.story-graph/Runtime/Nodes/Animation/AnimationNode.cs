using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AnimationNode : ActionNode
    {
        private Animator _animator;
        [HideInInspector] public string parameter;
        [HideInInspector] public enum AnimatorParameterType { Trigger, Float, Int, Bool }
        [HideInInspector] public AnimatorParameterType parameterType;
        [HideInInspector] public float floatValue;
        [HideInInspector] public int intValue;
        [HideInInspector] public bool boolValue;
        [SerializeField] private bool _waitEndAnimation;
        private int _currentStateHash;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _animator);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_animator != null)
            {
                switch (parameterType)
                {
                    case AnimatorParameterType.Float:
                        _animator.SetFloat(parameter, floatValue);
                        break;
                    case AnimatorParameterType.Int:
                        _animator.SetInteger(parameter, intValue);
                        break;
                    case AnimatorParameterType.Bool:
                        _animator.SetBool(parameter, boolValue);
                        break;
                    case AnimatorParameterType.Trigger:
                        _animator.SetTrigger(parameter);
                        break;
                }

                if (!_waitEndAnimation)
                {
                    TransitionToNextNodes(storyGraph);
                }
                // Сохраняем хэш текущего состояния
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                _currentStateHash = stateInfo.fullPathHash;
            }
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_animator != null)
            {
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);


                // Если анимация завершилась и не зациклена
                if (_waitEndAnimation && stateInfo.normalizedTime >= 1f)
                {
                    TransitionToNextNodes(storyGraph);
                }
            }
        }

    }
}
