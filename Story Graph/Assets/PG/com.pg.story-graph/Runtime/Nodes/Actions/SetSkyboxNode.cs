using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetSkyboxNode : StructureNode
    {
        [SerializeField] private Material _skyboxMaterial;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            RenderSettings.skybox = _skyboxMaterial;
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {

        }
    }
}
