using UnityEngine;

namespace PG.StorySystem
{
    using PG.StorySystem.Nodes;
    using UnityEditor.Experimental.GraphView;

    internal static class PortFactory
    {
        public static Port CreateVerticalPort(Node node, string portName, Direction direction, Port.Capacity capacity, Color color)
        {
            var port = node.InstantiatePort(Orientation.Vertical, direction, capacity, typeof(StoryNode));
            port.portName = portName;
            port.portColor = color;
            return port;
        }
        public static Port CreateHorizontalPort(Node node, string portName, Direction direction, Port.Capacity capacity, Color color)
        {
            var port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(PropertyNode));
            port.portName = portName;
            port.portColor = color;
            return port;
        }
    }
}
