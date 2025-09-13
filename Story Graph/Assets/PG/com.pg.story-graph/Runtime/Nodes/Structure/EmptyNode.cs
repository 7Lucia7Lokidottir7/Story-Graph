﻿using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class EmptyNode : StoryNode
    {
        public override Color colorNode => Color.gray1;
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
    }
}
