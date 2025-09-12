using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(SetMaterialDataNode), true)]
    public class SetMaterialDataNodeEditor : StoryNodeEditor
    {
        private SetMaterialDataNode _setMaterialDataNode;
        protected override void Init()
        {
            base.Init();
            _setMaterialDataNode = target as SetMaterialDataNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Object"));
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.choices = objects;
            dropdownField.value = _setMaterialDataNode.objectNameID;
            dropdownField.OnValueChanged += evt => {
                _setMaterialDataNode.objectNameID = evt;
                EditorUtility.SetDirty(_setMaterialDataNode);
            };
            root.Add(dropdownField);


            root.Add(new Label("Parameter Type"));
            EnumField parameterType = new EnumField(_setMaterialDataNode.parameterType);
            root.Add(parameterType);


            FloatField floatField = new FloatField();
            floatField.value = _setMaterialDataNode.floatValue;
            floatField.RegisterValueChangedCallback(c =>
            {
                _setMaterialDataNode.floatValue = c.newValue;
                EditorUtility.SetDirty(_setMaterialDataNode);
            });
            root.Add(floatField);

            ColorField colorField = new ColorField();
            colorField.value = _setMaterialDataNode.colorValue;
            colorField.hdr = true;
            colorField.RegisterValueChangedCallback(c =>
            {
                _setMaterialDataNode.colorValue = c.newValue;
                EditorUtility.SetDirty(_setMaterialDataNode);
            });
            root.Add(colorField);

            Vector2Field vector2Field = new Vector2Field();
            vector2Field.value = _setMaterialDataNode.vector2Value;
            vector2Field.RegisterValueChangedCallback(c =>
            {
                _setMaterialDataNode.vector2Value = c.newValue;
                EditorUtility.SetDirty(_setMaterialDataNode);
            });
            root.Add(vector2Field);

            Vector3Field vector3Field = new Vector3Field();
            vector3Field.value = _setMaterialDataNode.vector2Value;
            vector3Field.RegisterValueChangedCallback(c =>
            {
                _setMaterialDataNode.vector3Value = c.newValue;
                EditorUtility.SetDirty(_setMaterialDataNode);
            });
            root.Add(vector3Field);

            Vector4Field vector4Field = new Vector4Field();
            vector4Field.value = _setMaterialDataNode.vector2Value;
            vector4Field.RegisterValueChangedCallback(c => {
                _setMaterialDataNode.vector4Value = c.newValue;
                EditorUtility.SetDirty(_setMaterialDataNode);
            });
            root.Add(vector4Field);

            System.Action updateParameter = () => {
                switch (_setMaterialDataNode.parameterType)
                {
                    case SetMaterialDataNode.ParameterType.Float:
                        floatField.style.display = DisplayStyle.Flex;
                        colorField.style.display = DisplayStyle.None;
                        vector2Field.style.display = DisplayStyle.None;
                        vector3Field.style.display = DisplayStyle.None;
                        vector4Field.style.display = DisplayStyle.None;
                        break;
                    case SetMaterialDataNode.ParameterType.Color:
                        floatField.style.display = DisplayStyle.None;
                        colorField.style.display = DisplayStyle.Flex;
                        vector2Field.style.display = DisplayStyle.None;
                        vector3Field.style.display = DisplayStyle.None;
                        vector4Field.style.display = DisplayStyle.None;
                        break;
                    case SetMaterialDataNode.ParameterType.Vector2:
                        floatField.style.display = DisplayStyle.None;
                        colorField.style.display = DisplayStyle.None;
                        vector2Field.style.display = DisplayStyle.Flex;
                        vector3Field.style.display = DisplayStyle.None;
                        vector4Field.style.display = DisplayStyle.None;
                        break;
                    case SetMaterialDataNode.ParameterType.Vector3:
                        floatField.style.display = DisplayStyle.None;
                        colorField.style.display = DisplayStyle.None;
                        vector2Field.style.display = DisplayStyle.None;
                        vector3Field.style.display = DisplayStyle.Flex;
                        vector4Field.style.display = DisplayStyle.None;
                        break;
                    case SetMaterialDataNode.ParameterType.Vector4:
                        floatField.style.display = DisplayStyle.None;
                        colorField.style.display = DisplayStyle.None;
                        vector2Field.style.display = DisplayStyle.None;
                        vector3Field.style.display = DisplayStyle.None;
                        vector4Field.style.display = DisplayStyle.Flex;
                        break;
                }
            };

            parameterType.value = _setMaterialDataNode.parameterType;
            parameterType.RegisterValueChangedCallback(c =>
            {
                _setMaterialDataNode.parameterType = (SetMaterialDataNode.ParameterType)c.newValue;
                updateParameter.Invoke();
                EditorUtility.SetDirty(_setMaterialDataNode);
            });

            updateParameter.Invoke();

            Toggle useLerp = new Toggle("Use Lerp");

            FloatField duration = new FloatField("Duration");
            duration.value = _setMaterialDataNode.duration;
            duration.RegisterValueChangedCallback(c => {
                _setMaterialDataNode.duration = c.newValue;
                EditorUtility.SetDirty(_setMaterialDataNode);
            });

            useLerp.value = _setMaterialDataNode.useLerp;
            useLerp.RegisterValueChangedCallback((c) =>
            {
                _setMaterialDataNode.useLerp = c.newValue;
                duration.style.display = c.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                EditorUtility.SetDirty(_setMaterialDataNode);

            });


            duration.value = _setMaterialDataNode.duration;
            duration.style.display = _setMaterialDataNode.useLerp ? DisplayStyle.Flex : DisplayStyle.None;


            root.Add(useLerp);
            root.Add(duration);
        }
    }
}