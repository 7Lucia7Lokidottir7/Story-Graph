using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(BaseInteractionWithObjectsNode), true)]
    internal class BaseInteractionWithObjectsNodeEditor : StoryNodeEditor
    {
        private BaseInteractionWithObjectsNode _baseInteractionWithObjectsNode;
        private VisualElement _objectsContainer;

        [SerializeField] private VisualTreeAsset _treeAsset;

        protected override void Init()
        {
            base.Init();
            _baseInteractionWithObjectsNode = target as BaseInteractionWithObjectsNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            Button addButton = new Button { text = "+" };
            addButton.clicked += AddObject;
            root.Add(addButton);

            _objectsContainer = new VisualElement();
            root.Add(_objectsContainer);

            RenderObjectList();
        }

        void AddObject()
        {
            _baseInteractionWithObjectsNode.objectNamesID.Add("");
            RenderObjectList();
        }

        void RenderObjectList()
        {
            _objectsContainer.Clear();  // Очищаем контейнер перед перерисовкой
            if (_baseInteractionWithObjectsNode.objectNamesID.Count > 0)
            {
                for (int i = 0; i < _baseInteractionWithObjectsNode.objectNamesID.Count; i++)
                {
                    int index = i; // Локальная копия индекса для использования в лямбде



                    VisualElement objectElement = new VisualElement();
                    _treeAsset.CloneTree(objectElement);


                    _objectsContainer.Add(objectElement);

                    SearchableDropdownField dropdownField = objectElement.Q<SearchableDropdownField>();
                    dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
                    dropdownField.choices = objects;
                    dropdownField.value = _baseInteractionWithObjectsNode.objectNamesID[index];
                    dropdownField.OnValueChanged += evt =>
                    {
                        _baseInteractionWithObjectsNode.objectNamesID[index] = evt;
                    };
                    Button removeButton = objectElement.Q<Button>();
                    removeButton.clicked += () => RemoveObject(index);

                }
            }
        }

        void RemoveObject(int index)
        {
            if (index >= 0 && index < _baseInteractionWithObjectsNode.objectNamesID.Count)
            {
                _baseInteractionWithObjectsNode.objectNamesID.RemoveAt(index);
                RenderObjectList(); // Перерисовываем список только после удаления
            }
        }
    }
}
