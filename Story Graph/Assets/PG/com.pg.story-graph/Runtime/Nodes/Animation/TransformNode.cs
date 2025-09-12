using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class TransformNode : ActionNode
    {
        [HideInInspector] public string targetObjectNameID;
        public override string classGUI => "transform";
    }
}