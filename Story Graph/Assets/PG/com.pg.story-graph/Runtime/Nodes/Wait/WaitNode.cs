using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.StorySystem.Nodes
{

    public abstract class WaitNode : StoryNode
    {
        protected override bool useUpdate => true;

        public override Color colorNode => Color.darkCyan;
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
