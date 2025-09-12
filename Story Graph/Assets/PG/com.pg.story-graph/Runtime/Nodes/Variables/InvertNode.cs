using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class InvertNode : PropertyNode
    {
        [HideInInspector] public StoryVariable scenarioVariable;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            switch (scenarioVariable)
            {
                case FloatVariable:
                    FloatVariable floatVariable = scenarioVariable as FloatVariable;
                    floatVariable.floatValue *= -1;
                    break;
                case IntVariable:
                    IntVariable intVariable = scenarioVariable as IntVariable;
                    intVariable.intValue *= -1;
                    break;
                case BoolVariable:
                    BoolVariable boolVariable = scenarioVariable as BoolVariable;
                    boolVariable.boolValue = !boolVariable.boolValue;
                    break;
                case Vector2Variable:
                    Vector2Variable vector2Variable = scenarioVariable as Vector2Variable;
                    vector2Variable.vector2Value *= -1;
                    break;
                case Vector3Variable:
                    Vector3Variable vector3Variable = scenarioVariable as Vector3Variable;
                    vector3Variable.vector3Value *= -1;
                    break;
            }
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
