using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class SwitchActiveObjectNode : ActionNode
    {
        private GameObject _gameObject;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _gameObject);
            if (_gameObject)
            {
                _gameObject.SetActive(!_gameObject.activeSelf);
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
