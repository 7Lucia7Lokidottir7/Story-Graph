using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ParticleSystemInvokeNode : ActionNode
    {
        private ParticlesInvoker _particlesInvoker;
        [SerializeField] private string _nameParticleSystem;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _particlesInvoker);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _particlesInvoker.particles.TryGetValue(_nameParticleSystem, out ParticleSystem particleSystem);
            particleSystem?.Play();
            TransitionToNextNodes(storyGraph);
        }
    }
}
