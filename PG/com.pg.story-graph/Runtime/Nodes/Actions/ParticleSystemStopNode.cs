using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ParticleSystemStopNode : ActionNode
    {
        private ParticlesInvoker _particlesInvoker;
        [SerializeField] private string _nameParticleSystem;
        [SerializeField] private bool _clearParticles;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _particlesInvoker);
            _particlesInvoker.particles.TryGetValue(_nameParticleSystem, out ParticleSystem particleSystem);
            if (_clearParticles)
            {
                particleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            else
            {
                particleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
