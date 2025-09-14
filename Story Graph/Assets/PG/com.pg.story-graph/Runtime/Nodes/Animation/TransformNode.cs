using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class TransformNode : ActionNode
    {
        [HideInInspector]
        [InspectorLabel("Target Object")]
        [StoryGraphDropdown("objects")] 
        public string targetObjectNameID;
        public override Color colorNode => Color.aliceBlue;
    }
}