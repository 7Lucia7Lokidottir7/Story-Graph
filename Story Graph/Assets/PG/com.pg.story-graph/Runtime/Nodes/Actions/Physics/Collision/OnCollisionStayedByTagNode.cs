﻿using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnCollisionStayedByTagNode : CollisionNode
    {
        [SerializeField] private string[] _tags;
        public override void CheckCollision(Collision collision)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                bool value = _isInvertTarget ? !collision.collider.CompareTag(_tags[i]) : collision.collider.CompareTag(_tags[i]);
                if (value)
                {
                    TransitionToNextNodes(storyGraph);
                }
            }
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onCollisionStayed -= CheckCollision;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onCollisionStayed += CheckCollision;
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
