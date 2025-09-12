using System;
using System.Linq;
using PG.StorySystem.Nodes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CustomNodeViewAttribute : Attribute
{
    public Type NodeType { get; }

    public CustomNodeViewAttribute(Type nodeType)
    {
        if (!typeof(StoryNode).IsAssignableFrom(nodeType))
        {
            throw new ArgumentException($"Type {nodeType.Name} must inherit from {nameof(StoryNode)}");
        }
        NodeType = nodeType;
    }
}
