using System.Collections.Generic;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class LogicalOperatorNode : ConditionNode
    {
        [HideInInspector] 
        public int conditionID = -1;
        [HideInInspector] 
        public int condition2ID = -1;
        protected ConditionNode condition;
        protected ConditionNode condition2;

        public override void InitializeCondition()
        {
            condition = storyGraph.GetNodeByID(conditionID, _groupNode) as ConditionNode;
            condition2 = storyGraph.GetNodeByID(condition2ID, _groupNode) as ConditionNode;

            condition.storyGraph = storyGraph;
            condition.InitializeCondition();
            condition2.storyGraph = storyGraph;
            condition2.InitializeCondition();
        }
        public override void OnDublicateChildren(Dictionary<int, int> idMapping, StoryNode originalNode)
        {
            LogicalOperatorNode logicalOperatorNode = originalNode as LogicalOperatorNode;
            if (idMapping.TryGetValue(logicalOperatorNode.conditionID, out int newConditionChildId))
            {
                conditionID = newConditionChildId; // Добавляем новую связь к скопированным нодам
            }
            if (idMapping.TryGetValue(logicalOperatorNode.condition2ID, out int newConditionChild2Id))
            {
                condition2ID = newConditionChild2Id; // Добавляем новую связь к скопированным нодам
            }
        }
    }
}
