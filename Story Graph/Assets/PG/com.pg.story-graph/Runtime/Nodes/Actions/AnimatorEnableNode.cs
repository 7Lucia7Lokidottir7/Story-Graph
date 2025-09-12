using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class AnimatorEnableNode : ActionNode
    {
        private Animator _animator;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _animator);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _animator.enabled = true;
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
