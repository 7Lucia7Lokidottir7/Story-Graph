using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class OrMergeNode : MergeNode
    {
        private bool _isStarted;

        public override void RestartNode(StoryGraph storyGraph)
        {
            _isStarted = false;
            base.RestartNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            TryTransition(storyGraph);
        }

        /// <summary>
        /// Переход на следующий узел выполняется только один раз.
        /// </summary>
        public override void TransitionToNextNodes(StoryGraph storyGraph)
        {
            if (!isEnded && !_isStarted)
            {
                _isStarted = true;
                base.TransitionToNextNodes(storyGraph);
            }
        }

        private void TryTransition(StoryGraph storyGraph)
        {
            // Вызываем OnTransitionToNextNode, условие уже проверяется в данном методе.
            TransitionToNextNodes(storyGraph);
        }
    }
}
