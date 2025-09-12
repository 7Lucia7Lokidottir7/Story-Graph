using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class WaitInSecondsNode : WaitNode
    {

        [SerializeField] private float _duration = 1f;
        public float duration => _duration;
        protected override IEnumerator Delay(StoryGraph storyGraph)
        {
            yield return new WaitForSeconds(_duration);
            TransitionToNextNodes(storyGraph);
        }
    }
}
