using Unity.VisualScripting;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class BoolConditionNode : ConditionNode
    {
        [HideInInspector] public string boolVariableNameID;
        public BoolVariable variable1 => storyGraph.variablesDictionary[storyVariableNameID] as BoolVariable;

        [HideInInspector] public bool isActiveVariable2;
        [HideInInspector] public string boolVariable2NameID;
        public BoolVariable variable2 => storyGraph.variablesDictionary[storyVariable2NameID] as BoolVariable;
        [HideInInspector] public bool data2;
        public override bool GetResult()
        {
            bool result = false;
            //Debug.Log($"Var1: {variable1.intValue} \n data: {data2}");
            if (isActiveVariable2)
            {
                result = variable1.boolValue == variable2.boolValue;
            }
            else
            {
                result = variable1.boolValue == data2;
            }
            return result;
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
