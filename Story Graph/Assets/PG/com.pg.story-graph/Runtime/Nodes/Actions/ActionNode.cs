using System.Collections;
using UnityEngine;

namespace PG.StorySystem.Nodes
{

    public abstract class ActionNode : StoryNode
    {
        [HideInInspector]
        [InspectorLabel("Object")]
        [StoryGraphDropdown("objects")]
        public string objectNameID;
        public override Color colorNode => Color.darkRed;
        protected override void OnStart(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
        }
    }
}
