using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;


namespace PG.StorySystem.Nodes
{
    public class UseTeleportNode : InteractNode
    {
        private TeleportationAnchor _teleportationAnchor;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _teleportationAnchor);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _teleportationAnchor.teleporting.RemoveListener(args => TransitionToNextNodes(storyGraph));
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            _teleportationAnchor.teleporting.AddListener(args => TransitionToNextNodes(storyGraph));
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}