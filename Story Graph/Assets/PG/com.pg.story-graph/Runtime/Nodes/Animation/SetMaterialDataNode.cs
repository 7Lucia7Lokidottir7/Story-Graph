using System.Collections;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetMaterialDataNode : BaseMaterialNode
    {
        private Material _material;

        [SerializeField] private string _parameter;
        public enum ParameterType { Float, Color, Vector2, Vector3, Vector4 }
        [HideInInspector] public ParameterType parameterType;

        [HideInInspector] public float floatValue;
        [HideInInspector][ColorUsage(true, true)] public Color colorValue;
        [HideInInspector] public Vector2 vector2Value;
        [HideInInspector] public Vector3 vector3Value;
        [HideInInspector] public Vector4 vector4Value;

        [HideInInspector] public bool useLerp;
        [HideInInspector] public float duration;


        // Переменные для начальных значений
        private float _startFloat;
        private Color _startColor;
        private Vector2 _startVector2;
        private Vector3 _startVector3;
        private Vector4 _startVector4;

        protected override bool useUpdate => true;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _renderer);
            _material = _renderer.materials[_materialIndex];
        }
        protected override void OnStart(StoryGraph storyGraph)
        {

            if (!useLerp)
            {
                SetMaterialParameter();
                TransitionToNextNodes(storyGraph);
            }
            else
            {
                // Сохраняем начальные значения
                switch (parameterType)
                {
                    case ParameterType.Float:
                        _startFloat = _material.GetFloat(_parameter);
                        break;
                    case ParameterType.Color:
                        _startColor = _material.GetColor(_parameter);
                        break;
                    case ParameterType.Vector2:
                        _startVector2 = _material.GetVector(_parameter);
                        break;
                    case ParameterType.Vector3:
                        _startVector3 = _material.GetVector(_parameter);
                        break;
                    case ParameterType.Vector4:
                        _startVector4 = _material.GetVector(_parameter);
                        break;
                }
            }
        }

        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            float startTime = Time.time;
            while (true)
            {
                if (useLerp)
                {
                    float elapsedTime = Time.time - startTime;

                    if (elapsedTime < duration)
                    {
                        float lerpFactor = elapsedTime / duration;

                        // Lerp от начального значения к целевому значению
                        switch (parameterType)
                        {
                            case ParameterType.Float:
                                _material.SetFloat(_parameter, Mathf.Lerp(_startFloat, floatValue, lerpFactor));
                                break;
                            case ParameterType.Color:
                                _material.SetColor(_parameter, Color.Lerp(_startColor, colorValue, lerpFactor));
                                break;
                            case ParameterType.Vector2:
                                _material.SetVector(_parameter, Vector2.Lerp(_startVector2, vector2Value, lerpFactor));
                                break;
                            case ParameterType.Vector3:
                                _material.SetVector(_parameter, Vector3.Lerp(_startVector3, vector3Value, lerpFactor));
                                break;
                            case ParameterType.Vector4:
                                _material.SetVector(_parameter, Vector4.Lerp(_startVector4, vector4Value, lerpFactor));
                                break;
                        }
                    }
                    else
                    {
                        // Устанавливаем окончательное значение и переходим к следующему узлу
                        SetMaterialParameter();
                        TransitionToNextNodes(storyGraph);
                    }
                }
                yield return null;
            }
        }

        private void SetMaterialParameter()
        {
            switch (parameterType)
            {
                case ParameterType.Float:
                    _material.SetFloat(_parameter, floatValue);
                    break;
                case ParameterType.Color:
                    _material.SetColor(_parameter, colorValue);
                    break;
                case ParameterType.Vector2:
                    _material.SetVector(_parameter, vector2Value);
                    break;
                case ParameterType.Vector3:
                    _material.SetVector(_parameter, vector3Value);
                    break;
                case ParameterType.Vector4:
                    _material.SetVector(_parameter, vector4Value);
                    break;
            }
        }
    }
}
