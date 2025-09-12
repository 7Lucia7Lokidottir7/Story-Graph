using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{

    public abstract class ActionNode : StoryNode
    {
        [HideInInspector] public string objectNameID;
        public override string classGUI => "action";
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            TransitionToNextNodes(storyGraph);
        }
    }
}
