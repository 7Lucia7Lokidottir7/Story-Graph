namespace PG.StorySystem
{
    using Nodes;
    using System.Linq;
    using UnityEditor.Graphs;

    [CustomNodeView(typeof(PropertyNode))]
    public class PropertyNodeView : StoryNodeView
    {
        public PropertyNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            SetupClasses();
            PropertyNode propertyNode = node as PropertyNode;
            updateNode += (node) =>
            {
                UpdateTitle(propertyNode, StoryGraphEditorWindow.storyGraph);
            };
            UpdateTitle(propertyNode, StoryGraphEditorWindow.storyGraph);
        }
        void UpdateTitle(PropertyNode propertyNode, StoryGraph graph)
        {
            title = propertyNode.GetName();

            // простая проверка и поиск по string
            if (graph != null && !string.IsNullOrEmpty(propertyNode.storyVariableNameID))
            {
                StoryVariable found = graph.variables
                                 .FirstOrDefault(v => v.variableName == propertyNode.storyVariableNameID) as StoryVariable;
                if (found != null)
                    title += $" ({found.variableName})";

                //Debug.Log(found);
            }
            //Debug.Log(propertyNode.scenarioVariableNameID);
        }
    }
}
