using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetSkyboxNode : StructureNode
    {
        [SerializeField] private Material _skyboxMaterial;

        protected override void OnStart(StoryGraph storyGraph)
        {
            RenderSettings.skybox = _skyboxMaterial;
            TransitionToNextNodes(storyGraph);
        }
    }
}
