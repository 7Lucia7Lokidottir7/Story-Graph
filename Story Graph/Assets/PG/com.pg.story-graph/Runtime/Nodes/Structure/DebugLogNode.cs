using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class DebugLogNode : StructureNode
    {

        [TextArea(3,10)][SerializeField] protected string _textLog = "DEBUG TEST";


        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            Debug.Log(_textLog);
            TransitionToNextNodes(storyGraph); // Переход на следующий узел только после выполнения действий
        }


        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
    }

}