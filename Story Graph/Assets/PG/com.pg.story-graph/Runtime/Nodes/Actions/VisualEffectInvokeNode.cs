using UnityEngine;
using UnityEngine.VFX;

namespace PG.StorySystem.Nodes
{
    public class VisualEffectInvokeNode : ActionNode
    {
        private VFXInvoker _vfxInvoker;
        [SerializeField] private string _nameParticleSystem;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _vfxInvoker);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _vfxInvoker.visualEffects.TryGetValue(_nameParticleSystem, out VisualEffect visualEffect);
            visualEffect?.Play();
            TransitionToNextNodes(storyGraph);
        }
    }
}
