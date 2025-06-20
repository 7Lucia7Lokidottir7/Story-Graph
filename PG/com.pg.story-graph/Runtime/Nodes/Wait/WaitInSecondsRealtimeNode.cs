using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class WaitInSecondsRealtimeNode : WaitNode
    {

        [SerializeField] private float _duration = 1f;
        public float duration => _duration;
        protected override IEnumerator Delay(StoryGraph storyGraph)
        {
            yield return new WaitForSecondsRealtime(_duration);
            OnTransitionToNextNode(storyGraph);
        }
    }
}
