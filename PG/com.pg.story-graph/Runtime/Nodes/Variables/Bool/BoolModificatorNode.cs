using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class BoolModificatorNode : PropertyNode
    {
        [HideInInspector] public BoolVariable variable1;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public BoolVariable variable2;
        [HideInInspector] public bool dataBool2;
        protected override void Init(StoryGraph storyGraph)
        {
            variable1 = storyGraph.variablesDictionary[storyVariableNameID] as BoolVariable;
            if (isActiveVariable2)
            {
                variable2 = storyGraph.variablesDictionary[storyVariable2NameID] as BoolVariable;
            }
        }
    }
}
