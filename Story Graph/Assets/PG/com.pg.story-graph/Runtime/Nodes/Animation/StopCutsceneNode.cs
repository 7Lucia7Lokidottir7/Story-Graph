using UnityEngine.Playables;

namespace PG.StorySystem.Nodes
{
    public class StopCutsceneNode : ActionNode
    {
        private PlayableDirector _playableDirector;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(objectNameID, out _playableDirector);
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _playableDirector.Stop();
            TransitionToNextNodes(storyGraph);
        }
    }
}
