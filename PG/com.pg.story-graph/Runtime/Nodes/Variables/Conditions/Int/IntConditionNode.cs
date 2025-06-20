using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class IntConditionNode : ConditionNode
    {
        [HideInInspector] public string intVariableNameID;
        public IntVariable variable1 => storyGraph.variablesDictionary[storyVariableNameID] as IntVariable;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public string intVariable2NameID;
        public IntVariable variable2 => storyGraph.variablesDictionary[storyVariable2NameID] as IntVariable;
        [HideInInspector] public int data2;
    }
}
