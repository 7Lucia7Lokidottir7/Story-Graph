using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ParticleSystemInvokeNode : ActionNode
    {
        private ParticlesInvoker _particlesInvoker;
        [SerializeField] private string _nameParticleSystem;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _particlesInvoker);
            _particlesInvoker.particles.TryGetValue(_nameParticleSystem, out ParticleSystem particleSystem);
            particleSystem?.Play();
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
