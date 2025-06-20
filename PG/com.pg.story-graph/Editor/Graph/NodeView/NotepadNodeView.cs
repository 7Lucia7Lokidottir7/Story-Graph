
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{

    [CustomNodeView(typeof(NotepadNode))]
    public class NotepadNodeView : StoryNodeView
    {
        public NotepadNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            SetupClasses();
        }
        protected override void CreateInputPorts()
        {
        }
        protected override void CreateVariableInputNode()
        {
        }
        protected override void CreateOutputPorts()
        {
        }
    }
}