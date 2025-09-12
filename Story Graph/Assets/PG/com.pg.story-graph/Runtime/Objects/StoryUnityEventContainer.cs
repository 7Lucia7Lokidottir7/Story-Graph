using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PG.StorySystem
{
    public class StoryUnityEventContainer : MonoBehaviour
    {
        [SerializeField] private UnityEventElement[] _events;
        public Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();
        [System.Serializable]
        public class UnityEventElement
        {
            public string name;
            public UnityEvent unityEvent;
        }
        private void Start()
        {
            for (int i = 0; i < _events.Length; i++)
            {
                events.Add(_events[i].name, _events[i].unityEvent);
            }
        }
        public void OnInvoke(int index)
        {
            _events[index]?.unityEvent?.Invoke();
        }
    }
}
