using UnityEngine;
using UnityEngine.Events;

namespace PG.StorySystem.Nodes
{
    public class UnityEventInvokeNode : ActionNode
    {
        private StoryUnityEventContainer _unityEventContainer;
        [SerializeField] private string _nameEvent;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _unityEventContainer);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _unityEventContainer.events.TryGetValue(_nameEvent, out UnityEvent unityEvent);
            unityEvent?.Invoke();
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

    }
}
