using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class AnimatorDisableNode : ActionNode
    {
        private Animator _animator;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _animator);
            _animator.enabled = false;
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
