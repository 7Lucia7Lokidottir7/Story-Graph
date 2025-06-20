using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class FloatModificatorNode : PropertyNode
    {
        [HideInInspector] public FloatVariable variable1;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public FloatVariable variable2;
        [HideInInspector] public float dataFloat2;
        protected override void Init(StoryGraph storyGraph)
        {
            variable1 = storyGraph.variablesDictionary[storyVariableNameID] as FloatVariable;
            if (isActiveVariable2)
            {
                variable2 = storyGraph.variablesDictionary[storyVariable2NameID] as FloatVariable;
            }
        }
    }
}
