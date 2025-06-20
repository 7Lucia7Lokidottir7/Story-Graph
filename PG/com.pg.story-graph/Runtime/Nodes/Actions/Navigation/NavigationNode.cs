using UnityEngine;
using UnityEngine.AI;
namespace PG.StorySystem.Nodes
{
    public abstract class NavigationNode : ActionNode
    {
        public override string classGUI => "navigation";
        protected NavMeshAgent _agent;
        [SerializeField] protected float _speed = 3.5f;
        [SerializeField] protected float _angularSpeed = 120f;
        [SerializeField] protected float _acceleration = 2f;
        [SerializeField] protected float _stoppingDistance = 1f;
        [HideInInspector] public string targetObjectNameID;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _agent);
            _agent.speed = _speed;
            _agent.angularSpeed = _angularSpeed;
            _agent.acceleration = _acceleration;
            _agent.stoppingDistance = _stoppingDistance;
        }
    }
}
