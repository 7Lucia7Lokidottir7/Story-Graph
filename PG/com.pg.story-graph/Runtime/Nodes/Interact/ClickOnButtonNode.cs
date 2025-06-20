using UnityEngine.UI;

namespace PG.StorySystem.Nodes
{
    public class ClickOnButtonNode : InteractNode
    {
        private Button _button;
        private StoryGraph _graph;


        protected override void OnEnd(StoryGraph storyGraph)
        {
            _button.onClick?.RemoveListener(OnClick);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _graph = storyGraph;

            storyGraph.GetObject(objectNameID, out _button);
            _button.onClick?.AddListener(OnClick);
        }
        void OnClick()
        {
            OnTransitionToNextNode(_graph);
        }
    }
}
