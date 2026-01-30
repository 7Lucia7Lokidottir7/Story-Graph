using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class WaitInFixedUpdateNode : WaitNode
    {
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            yield return new WaitForFixedUpdate();
            TransitionToNextNodes(storyGraph);
        }
    }
}
