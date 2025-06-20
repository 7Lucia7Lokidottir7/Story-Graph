using UnityEngine;
using UnityEngine.Events;

namespace PG.StorySystem.Nodes
{
    public class CompositeUnityEventInvokeNode : ActionNode
    {
        private StoryUnityEventContainer _unityEventContainer;
        [Header("This node always works until it is terminated by another node.")]
        [SerializeField] private string _startNameEvent;
        [SerializeField] private string _uppdateNameEvent;
        [SerializeField] private string _endNameEvent;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (_endNameEvent == "")
            {
                return;
            }
            _unityEventContainer.events.TryGetValue(_endNameEvent, out UnityEvent unityEvent);
            unityEvent?.Invoke();
        }

        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _unityEventContainer);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_startNameEvent == "")
            {
                return;
            }
            _unityEventContainer.events.TryGetValue(_startNameEvent, out UnityEvent unityEvent);
            unityEvent?.Invoke();
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_uppdateNameEvent == "")
            {
                return;
            }
            _unityEventContainer.events.TryGetValue(_uppdateNameEvent, out UnityEvent unityEvent);
            unityEvent?.Invoke();
        }

    }
}
