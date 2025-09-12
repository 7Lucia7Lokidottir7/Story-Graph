using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class Vector3ConditionNode : ConditionNode
    {
        [HideInInspector] public string variable1NameID;
        public Vector3Variable variable1 => storyGraph.variablesDictionary[storyVariableNameID] as Vector3Variable;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public string variable2NameID;
        public Vector3Variable variable2 => storyGraph.variablesDictionary[storyVariable2NameID] as Vector3Variable;
        [HideInInspector] public Vector3 data2;
    }
}
