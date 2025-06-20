﻿using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AddVector2Node : Vector2ModificatorNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isActiveVariable2)
            {
                variable1.vector2Value += variable2.vector2Value;
            }
            else if (!isActiveVariable2)
            {
                variable1.vector2Value += dataVector2;
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
