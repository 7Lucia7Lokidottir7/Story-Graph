using UnityEngine;
using UnityEngine.VFX;

namespace PG.StorySystem.Nodes
{
    public class VisualEffectInvokeNode : ActionNode
    {
        private VFXInvoker _vfxInvoker;
        [SerializeField] private string _nameParticleSystem;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _vfxInvoker);
            _vfxInvoker.visualEffects.TryGetValue(_nameParticleSystem, out VisualEffect visualEffect);
            visualEffect?.Play();
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
