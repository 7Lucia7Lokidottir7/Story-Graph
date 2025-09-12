using UnityEditor;
using UnityEngine;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using System;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(PlayClipAtPointNode))]
    public class PlayClipAtPointNodeEditor : StoryNodeEditor
    {
        private PlayClipAtPointNode _playClipAtPoint;
        protected override void Init()
        {
            base.Init();
            _playClipAtPoint = target as PlayClipAtPointNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            // Добавляем метку
            Label label = new Label("AudioSource Object");
            root.Add(label);

            System.Action<PlayClipAtPointNode.PositionType> action = default;

            // Создаем PropertyField для поля с enum (без дублирования)
            EnumField enumField = new EnumField("Position Type", _playClipAtPoint.positionType);
            enumField.RegisterValueChangedCallback(evt =>
            {
                _playClipAtPoint.positionType = (PlayClipAtPointNode.PositionType)evt.newValue;
                EditorUtility.SetDirty(_playClipAtPoint);
                action?.Invoke((PlayClipAtPointNode.PositionType)evt.newValue);
            });
            root.Add(enumField);


            Vector3Field positionField = new Vector3Field();
            positionField.value = _playClipAtPoint.position;
            positionField.RegisterValueChangedCallback(evt => {
                EditorUtility.SetDirty(_playClipAtPoint);
                _playClipAtPoint.position = evt.newValue;
            });
            root.Add(positionField);

            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.choices = objects;
            dropdownField.value = _playClipAtPoint.transformNameIndex;
            dropdownField.OnValueChanged += evt => {
                _playClipAtPoint.transformIndex = objects.IndexOf(evt);
                _playClipAtPoint.transformNameIndex = evt;
                EditorUtility.SetDirty(_playClipAtPoint);
            };
            root.Add(dropdownField);

            action = (PlayClipAtPointNode.PositionType positionType) =>
            {
                EditorUtility.SetDirty(_playClipAtPoint);
                switch (positionType)
                {
                    case PlayClipAtPointNode.PositionType.Object:
                        positionField.style.display = DisplayStyle.None;
                        dropdownField.style.display = DisplayStyle.Flex;
                        break;
                    case PlayClipAtPointNode.PositionType.Vector3:
                        positionField.style.display = DisplayStyle.Flex;
                        dropdownField.style.display = DisplayStyle.None;
                        break;
                }
            };

            action?.Invoke(_playClipAtPoint.positionType);
        }
    }
}