using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AndMergeNode : MergeNode
    {
        [HideInInspector]
        public int previousNodesCount = 0;  // Общее количество предыдущих нод для слияния
        [SerializeField]
        private int _endPreviousNodes = 0;  // Счётчик завершённых предыдущих нод
        private bool _isStarted = false;

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            // Нет логики обновления для AndMergeNode
        }

        public override void OnDublicate()
        {
            base.OnDublicate();
            previousNodesCount = 0;
        }

        public override void RestartNode(StoryGraph storyGraph)
        {
            base.RestartNode(storyGraph);
            _endPreviousNodes = 0;
            _isStarted = false;
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
            _endPreviousNodes = 0;
            _isStarted = false;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            TryTransition(storyGraph);
        }

        protected override void OnNotFirstStart(StoryGraph storyGraph)
        {
            TryTransition(storyGraph);
        }

        /// <summary>
        /// Инкрементирует счётчик завершённых предыдущих нод и выполняет переход при достижении порога.
        /// </summary>
        private void TryTransition(StoryGraph storyGraph)
        {
            _endPreviousNodes++;
            if (_endPreviousNodes >= previousNodesCount && !_isStarted)
            {
                _isStarted = true;
                OnTransitionToNextNode(storyGraph);
                _endPreviousNodes = 0;
            }
        }
    }
}
