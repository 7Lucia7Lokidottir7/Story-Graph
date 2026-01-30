using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class WaitInEndFrameNode : WaitNode
    {
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            yield return new WaitForEndOfFrame();
            TransitionToNextNodes(storyGraph);
        }
    }
}
