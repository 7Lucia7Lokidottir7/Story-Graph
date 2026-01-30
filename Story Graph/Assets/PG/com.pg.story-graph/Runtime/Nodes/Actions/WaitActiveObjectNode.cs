using System.Collections;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class WaitActiveObjectNode : ActionNode
    {
        private Transform _transform;
        [SerializeField] private bool _activeValue;
        public override Color colorNode => Color.green;
        protected override bool useUpdate => true;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_transform == null)
            {
                TransitionToNextNodes(storyGraph);
            }
        }

        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            while (true)
            {
                if (_transform != null)
                {
                    if (_transform.gameObject.activeInHierarchy == _activeValue)
                    {
                        TransitionToNextNodes(storyGraph);
                    }
                }
                yield return null;
            }
        }
    }
}
