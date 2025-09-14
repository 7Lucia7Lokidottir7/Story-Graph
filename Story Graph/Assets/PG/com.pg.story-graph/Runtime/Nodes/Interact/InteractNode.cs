using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class InteractNode : StoryNode
    {
        [HideInInspector] public string objectNameID;
        public override Color colorNode => Color.paleGoldenRod;
    }
}
