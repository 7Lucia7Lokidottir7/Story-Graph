using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class PropertyNode : StoryNode
    {
        [HideInInspector] public string storyVariableNameID;
        [HideInInspector] public string storyVariable2NameID;
        public override string classGUI => "variable";
    }
}
