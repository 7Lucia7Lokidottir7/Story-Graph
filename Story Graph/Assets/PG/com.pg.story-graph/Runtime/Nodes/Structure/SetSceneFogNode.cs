using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetSceneFogNode : StructureNode
    {
        [SerializeField] private bool _useFog;
        [HideInInspector] public bool changeFogMode;
        [HideInInspector] public FogMode fogMode;
        [HideInInspector] public bool changeColor;
        [HideInInspector] public Color fogColor = Color.gray;
        [HideInInspector] public bool changeDensity;
        [HideInInspector] public float fogDensity = 0.01f;
        [HideInInspector] public float fogStartDistance;
        [HideInInspector] public float fogEndDistance = 300;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            RenderSettings.fog = _useFog;
            if (changeFogMode)
            {
                RenderSettings.fogMode = fogMode;
            }
            if (changeColor)
            {
                RenderSettings.fogColor = fogColor;
            }
            if (changeDensity)
            {
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.fogStartDistance = fogDensity;
                RenderSettings.fogEndDistance = fogEndDistance;
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
