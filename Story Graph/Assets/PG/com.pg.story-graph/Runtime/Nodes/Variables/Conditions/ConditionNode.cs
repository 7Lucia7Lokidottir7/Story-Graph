using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.Nodes
{
    public abstract class ConditionNode : PropertyNode
    {
        public virtual bool GetResult()
        {
            return false;
        }
        public virtual void InitializeCondition()
        {
        }
        public override Color colorNode => Color.lawnGreen;
    }
}
