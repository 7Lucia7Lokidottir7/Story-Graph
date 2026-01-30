using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class SwitchActiveObjectNode : ActionNode
    {
        private GameObject _gameObject;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _gameObject);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_gameObject)
            {
                _gameObject.SetActive(!_gameObject.activeSelf);
            }
            TransitionToNextNodes(storyGraph);
        }
    }
}
