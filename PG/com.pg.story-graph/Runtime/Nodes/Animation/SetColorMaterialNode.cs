using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetColorMaterialNode : BaseMaterialNode
    {
        private Material _material;
        private Color _startColor;

        [ColorUsage(true, true)] public Color colorValue;
        [HideInInspector] public bool useLerp;
        [HideInInspector] public float duration;

        private float _startTime;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _renderer);
            _material = _renderer.materials[_materialIndex];

            _startColor = _material.color;  // Сохраняем начальный цвет
            _startTime = Time.time;

            if (!useLerp)
            {
                _material.color = colorValue;
                OnTransitionToNextNode(storyGraph);
            }
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (useLerp)
            {
                float elapsedTime = Time.time - _startTime;

                if (elapsedTime < duration)
                {
                    // Lerp от _startColor к colorValue
                    _material.color = Color.Lerp(_startColor, colorValue, elapsedTime / duration);
                }
                else
                {
                    // Устанавливаем окончательный цвет и переходим к следующему узлу
                    _material.color = colorValue;
                    OnTransitionToNextNode(storyGraph);
                }
            }
        }
    }
}
