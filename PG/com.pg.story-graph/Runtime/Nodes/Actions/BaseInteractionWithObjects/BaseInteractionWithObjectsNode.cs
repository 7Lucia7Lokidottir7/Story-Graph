using System.Collections.Generic;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class BaseInteractionWithObjectsNode : StoryNode
    {
        public List<string> objectNamesID = new List<string>();
        protected List<Transform> _objects = new List<Transform>();
        protected override void Init(StoryGraph storyGraph)
        {
            _objects.Clear();
            for (int i = 0; i < objectNamesID.Count; i++)
            {
                storyGraph.GetObject(objectNamesID[i], out Transform gameObject);
                _objects.Add(gameObject.transform);
            }
        }
    }
}