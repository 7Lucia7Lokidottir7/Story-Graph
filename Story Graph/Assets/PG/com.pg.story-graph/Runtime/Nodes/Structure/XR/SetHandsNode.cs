using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetHandsNode : StructureNode
    {
        private HandsContainer _handsContainer;
        [SerializeField] private string _handsName;
        [SerializeField] private HandsContainer.HandType _handType;
        protected override void Init(StoryGraph storyGraph)
        {
            _handsContainer = FindAnyObjectByType<HandsContainer>();
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _handsContainer.SetHands(_handsName, _handType);
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
