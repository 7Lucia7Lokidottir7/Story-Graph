using System.Collections;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class MoveObjectNode : ActionNode
    {
        private Transform _transform;
        [Header("This node always works until it is terminated by another node.")]
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _speed;
        [SerializeField] private Space _space;
        protected override bool useUpdate => true;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
        }

        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            while (true)
            {
                _transform.Translate(_direction * _speed * Time.deltaTime, _space);
                yield return null;
            }
        }

    }
}
