using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class InteractNode : StoryNode
    {
        [HideInInspector]
        [InspectorLabel("Object")]
        [StoryGraphDropdown("objects")]
        public string objectNameID;
        public override Color colorNode => Color.paleGoldenRod;
    }
}
