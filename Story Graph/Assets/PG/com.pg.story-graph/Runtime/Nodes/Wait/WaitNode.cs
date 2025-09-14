using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.StorySystem.Nodes
{

    public abstract class WaitNode : StoryNode
    {
        protected Coroutine _coroutine;

        public override Color colorNode => Color.darkCyan;
        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (_coroutine != null)
            {
                storyGraph.runner.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _coroutine = storyGraph.runner.StartCoroutine(Delay(storyGraph));
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected abstract IEnumerator Delay(StoryGraph storyGraph);
    }
}
