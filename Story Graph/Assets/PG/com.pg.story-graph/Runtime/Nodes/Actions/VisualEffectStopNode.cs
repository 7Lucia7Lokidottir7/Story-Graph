using UnityEngine;
using UnityEngine.VFX;

namespace PG.StorySystem.Nodes
{
    public class VisualEffectStopNode : ActionNode
    {
        private VFXInvoker _vfxInvoker;
        [SerializeField] private string _nameVFX;
        [SerializeField] private bool _clearParticles;
        protected override void Init(StoryGraph storyGraph)
        {

            storyGraph.GetObject(objectNameID, out _vfxInvoker);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _vfxInvoker.visualEffects.TryGetValue(_nameVFX, out VisualEffect visualEffect);
            visualEffect?.Stop();
            if (_clearParticles)
            {
                visualEffect?.Reinit();
            }
            TransitionToNextNodes(storyGraph);
        }
    }
}
