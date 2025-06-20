using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class Vector2ModificatorNode : PropertyNode
    {

        [HideInInspector] public Vector2Variable variable1;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public Vector2Variable variable2;
        [HideInInspector] public Vector2 dataVector2;
        protected override void Init(StoryGraph storyGraph)
        {
            variable1 = storyGraph.variablesDictionary[storyVariableNameID] as Vector2Variable;
            if (isActiveVariable2)
            {
                variable2 = storyGraph.variablesDictionary[storyVariable2NameID] as Vector2Variable;
            }
        }
    }
}
