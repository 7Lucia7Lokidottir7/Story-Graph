using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AnimationStartStateNode : ActionNode
    {
        [SerializeField] private string _nameState;
        private Animator _animator;
        [SerializeField] private float _transitionDuration = 0.1f;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _animator);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _animator.CrossFadeInFixedTime(_nameState, _transitionDuration);
        }
    }
}
