using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class BaseMaterialNode : ActionNode
    {
        [Tooltip("Here the index of the material inside the MeshRenderer or SkinnedMeshRenderer is selected.")]
        [SerializeField] protected int _materialIndex;
        protected Renderer _renderer;
    }
}