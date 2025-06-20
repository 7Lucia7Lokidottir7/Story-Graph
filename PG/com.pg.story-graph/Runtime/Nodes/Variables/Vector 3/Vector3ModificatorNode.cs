using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class Vector3ModificatorNode : PropertyNode
    {
        [HideInInspector] public Vector3Variable variable1;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public Vector3Variable variable2;
        [HideInInspector] public Vector3 dataVector2;
        protected override void Init(StoryGraph storyGraph)
        {
            variable1 = storyGraph.variablesDictionary[storyVariableNameID] as Vector3Variable;
            if (isActiveVariable2)
            {
                variable2 = storyGraph.variablesDictionary[storyVariable2NameID] as Vector3Variable;
            }
        }
    }
}
