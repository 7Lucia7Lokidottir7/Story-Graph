using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class WaitInEndFrameNode : WaitNode
    {
        protected override IEnumerator Delay(StoryGraph storyGraph)
        {
            yield return new WaitForEndOfFrame();
            OnTransitionToNextNode(storyGraph);
        }
    }
}
