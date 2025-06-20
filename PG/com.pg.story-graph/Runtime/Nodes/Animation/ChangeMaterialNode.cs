﻿using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class ChangeMaterialNode : ActionNode
    {
        [SerializeField] private Material _material;
        [SerializeField] private int _materialIndex;
        private Renderer _renderer;

        
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _renderer);
            _renderer.materials[_materialIndex] = Instantiate(_material);
            OnTransitionToNextNode(storyGraph);
        }
    }
}