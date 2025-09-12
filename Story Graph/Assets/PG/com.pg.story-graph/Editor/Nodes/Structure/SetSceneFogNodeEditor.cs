using UnityEngine;
using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(SetSceneFogNode))]
    public class SetSceneFogNodeEditor : StoryNodeEditor
    {
        private SetSceneFogNode _setSceneFogNode;

        protected override void Init()
        {
            base.Init();
            _setSceneFogNode = target as SetSceneFogNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            System.Action updateDensityVisibility = default;
            // ������������� ��������� ������ ������
            Toggle changeMode = new Toggle("Change Fog Mode");
            changeMode.value = _setSceneFogNode.changeFogMode;
            root.Add(changeMode);

            // ���� ������ ������ ������
            EnumField fogMode = new EnumField("Fog Mode", (FogMode)_setSceneFogNode.fogMode);
            fogMode.value = _setSceneFogNode.fogMode;
            fogMode.style.display = _setSceneFogNode.changeFogMode ? DisplayStyle.Flex : DisplayStyle.None;
            fogMode.RegisterValueChangedCallback(evt =>
            {
                _setSceneFogNode.fogMode = (FogMode)evt.newValue;
                updateDensityVisibility?.Invoke();
            });
            root.Add(fogMode);

            // ������������� ��������� �����
            Toggle changeColor = new Toggle("Change Color");
            changeColor.value = _setSceneFogNode.changeColor;
            root.Add(changeColor);

            // ���� ������ ����� ������
            ColorField colorField = new ColorField("Fog Color");
            colorField.value = _setSceneFogNode.fogColor;
            colorField.style.display = _setSceneFogNode.changeColor ? DisplayStyle.Flex : DisplayStyle.None;
            colorField.RegisterValueChangedCallback(evt =>
            {
                _setSceneFogNode.fogColor = evt.newValue;
            });
            root.Add(colorField);

            // ������������� ��������� ���������
            Toggle changeDensity = new Toggle("Change Density");
            changeDensity.value = _setSceneFogNode.changeDensity;
            root.Add(changeDensity);

            // ���� ����� ��������� ������
            FloatField densityField = new FloatField("Fog Density");
            densityField.value = _setSceneFogNode.fogDensity;
            root.Add(densityField);

            // ���� ����� ��������� ��������� ��� ��������� ������
            FloatField fogStartDistanceField = new FloatField("Fog Start Distance");
            fogStartDistanceField.value = _setSceneFogNode.fogStartDistance;
            root.Add(fogStartDistanceField);

            // ���� ����� �������� ��������� ��� ��������� ������
            FloatField fogEndDistanceField = new FloatField("Fog End Distance");
            fogEndDistanceField.value = _setSceneFogNode.fogEndDistance;
            root.Add(fogEndDistanceField);

            // ����� ��� ���������� ��������� ���������
            updateDensityVisibility += delegate
            {
                if (_setSceneFogNode.changeFogMode && _setSceneFogNode.changeDensity)
                {
                    switch (_setSceneFogNode.fogMode)
                    {
                        case FogMode.Linear:
                            fogStartDistanceField.style.display = DisplayStyle.Flex;
                            fogEndDistanceField.style.display = DisplayStyle.Flex;
                            densityField.style.display = DisplayStyle.None;
                            break;
                        case FogMode.Exponential:
                        case FogMode.ExponentialSquared:
                            fogStartDistanceField.style.display = DisplayStyle.None;
                            fogEndDistanceField.style.display = DisplayStyle.None;
                            densityField.style.display = DisplayStyle.Flex;
                            break;
                    }
                }
                else
                {
                    fogStartDistanceField.style.display = DisplayStyle.None;
                    fogEndDistanceField.style.display = DisplayStyle.None;
                    densityField.style.display = DisplayStyle.None;
                }
            };

            // ����������� ������������ �������
            changeMode.RegisterValueChangedCallback(evt =>
            {
                _setSceneFogNode.changeFogMode = evt.newValue;
                fogMode.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                updateDensityVisibility?.Invoke();
            });

            changeColor.RegisterValueChangedCallback(evt =>
            {
                _setSceneFogNode.changeColor = evt.newValue;
                colorField.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            });

            changeDensity.RegisterValueChangedCallback(evt =>
            {
                _setSceneFogNode.changeDensity = evt.newValue;
                updateDensityVisibility?.Invoke();
            });

            densityField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue > 1f)
                {
                    densityField.value = 1f;
                }
                if (evt.newValue < 0f)
                {
                    densityField.value = 0f;
                }
                _setSceneFogNode.fogDensity = evt.newValue;
            });

            fogStartDistanceField.RegisterValueChangedCallback(evt =>
            {
                _setSceneFogNode.fogStartDistance = evt.newValue;
            });

            fogEndDistanceField.RegisterValueChangedCallback(evt =>
            {
                _setSceneFogNode.fogEndDistance = evt.newValue;
            });

            // ������������� ��������� ��������� ����� �������� ���� ���������
            updateDensityVisibility?.Invoke();
        }


    }
}
