using System.Collections;
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

        protected override bool useUpdate => true;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _renderer);
            _material = _renderer.materials[_materialIndex];
        }
        protected override void OnStart(StoryGraph storyGraph)
        {

            _startColor = _material.color;  // Сохраняем начальный цвет

            if (!useLerp)
            {
                _material.color = colorValue;
                TransitionToNextNodes(storyGraph);
            }
        }

        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            float time = Time.time;
            while (true)
            {
                if (useLerp)
                {
                    float elapsedTime = Time.time - time;

                    if (elapsedTime < duration)
                    {
                        // Lerp от _startColor к colorValue
                        _material.color = Color.Lerp(_startColor, colorValue, elapsedTime / duration);
                    }
                    else
                    {
                        // Устанавливаем окончательный цвет и переходим к следующему узлу
                        _material.color = colorValue;
                        TransitionToNextNodes(storyGraph);
                    }
                }
                yield return null;
            }
        }
    }
}
