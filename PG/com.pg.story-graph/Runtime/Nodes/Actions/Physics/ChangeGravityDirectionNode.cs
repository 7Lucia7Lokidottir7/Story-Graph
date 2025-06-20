using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class  ChangeGravityDirectionNode : PhysicsNode
    {
        [SerializeField][Tooltip("Base Earth gravity = 0, -9.81, 0")] private Vector3 _gravityDirection = new Vector3(0, -9.81f,0);
        protected override void Init(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            Physics.gravity = _gravityDirection;
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

    }
}
