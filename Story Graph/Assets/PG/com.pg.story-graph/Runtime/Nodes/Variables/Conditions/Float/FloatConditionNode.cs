using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class FloatConditionNode : ConditionNode
    {
        [HideInInspector] public string variable1NameID;
        public FloatVariable variable1 => storyGraph.variablesDictionary[storyVariableNameID] as FloatVariable;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public string variable2NameID;
        public FloatVariable variable2 => storyGraph.variablesDictionary[storyVariable2NameID] as FloatVariable;
        [HideInInspector] public float data2;

    }
}
