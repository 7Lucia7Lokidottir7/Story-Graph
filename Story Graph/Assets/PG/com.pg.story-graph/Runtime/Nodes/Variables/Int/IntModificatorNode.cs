namespace PG.StorySystem.Nodes
{
    using UnityEngine;
    public abstract class IntModificatorNode : PropertyNode
    {
        [HideInInspector] public IntVariable variable1;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public IntVariable variable2;
        [HideInInspector] public int dataInt2;

        protected override void Init(StoryGraph storyGraph)
        {
            variable1 = storyGraph.variablesDictionary[storyVariableNameID] as IntVariable;
            if (isActiveVariable2)
            {
                variable2 = storyGraph.variablesDictionary[storyVariable2NameID] as IntVariable;
            }
        }
    }
}
