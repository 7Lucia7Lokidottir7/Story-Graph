using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class Vector2ConditionNode : ConditionNode
    {
        [HideInInspector] public string variable1NameID;
        public Vector2Variable variable1 => storyGraph.variablesDictionary[storyVariableNameID] as Vector2Variable;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public string variable2NameID;
        public Vector2Variable variable2 => storyGraph.variablesDictionary[storyVariable2NameID] as Vector2Variable;
        [HideInInspector] public Vector2 data2;
    }
}
