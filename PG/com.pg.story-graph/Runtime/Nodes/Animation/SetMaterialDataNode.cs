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

        private float _startTime;

        // Переменные для начальных значений
        private float _startFloat;
        private Color _startColor;
        private Vector2 _startVector2;
        private Vector3 _startVector3;
        private Vector4 _startVector4;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _renderer);
            _material = _renderer.materials[_materialIndex];

            _startTime = Time.time;

            if (!useLerp)
            {
                SetMaterialParameter();
                OnTransitionToNextNode(storyGraph);
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

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (useLerp)
            {
                float elapsedTime = Time.time - _startTime;

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
                    OnTransitionToNextNode(storyGraph);
                }
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
