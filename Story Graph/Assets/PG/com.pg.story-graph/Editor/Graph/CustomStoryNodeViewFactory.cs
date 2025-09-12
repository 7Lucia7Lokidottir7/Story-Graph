using System.Collections.Generic;
using System;
using PG.StorySystem.Nodes;
using PG.StorySystem;
using System.Linq;

internal static class CustomStoryNodeViewFactory
{
    public static Dictionary<Type, Type> _nodeViewMappings = new();

    static CustomStoryNodeViewFactory()
    {
        RegisterCustomNodeViews();
    }

    internal static void RegisterCustomNodeViews()
    {
        _nodeViewMappings = new Dictionary<Type, Type>();

        // Получаем все типы из всех сборок (с осторожностью к исключениям)
        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                try { return assembly.GetTypes(); }
                catch { return new Type[0]; }
            })
            .ToArray();

        // Находим типы, у которых CustomNodeViewAttribute задан напрямую (без наследования)
        var nodeViewTypes = allTypes
            .Where(t => t.GetCustomAttributes(typeof(CustomNodeViewAttribute), false).Any())
            .Select(t => new
            {
                NodeViewType = t,
                Attribute = (CustomNodeViewAttribute)t.GetCustomAttributes(typeof(CustomNodeViewAttribute), false).First()
            })
            // Сортируем так, чтобы сначала обрабатывались более конкретные (у которых глубина наследования NodeType больше)
            .OrderByDescending(x => GetInheritanceDepth(x.Attribute.NodeType))
            .ToList();

        foreach (var item in nodeViewTypes)
        {
            var baseNodeType = item.Attribute.NodeType;

            // Регистрируем этот NodeView для всех классов-нод, которые наследуются от baseNodeType,
            // но только если для них ещё не назначен более специфичный мэппинг.
            var derivedTypes = allTypes
                .Where(t => baseNodeType.IsAssignableFrom(t) &&
                            !t.IsAbstract &&
                            typeof(StoryNode).IsAssignableFrom(t));

            foreach (var nodeType in derivedTypes)
            {
                // Если для ноды уже назначен мэппинг, то пропускаем (это и есть защита от затирания)
                if (!_nodeViewMappings.ContainsKey(nodeType))
                {
                    _nodeViewMappings[nodeType] = item.NodeViewType;
                }
            }
        }
    }

    // Метод для вычисления "глубины" наследования относительно Object (чем больше – тем конкретнее)
    private static int GetInheritanceDepth(Type type)
    {
        int depth = 0;
        while (type != null)
        {
            depth++;
            type = type.BaseType;
        }
        return depth;
    }

    public static StoryNodeView CreateNodeView(StoryNode node)
    {
        if (node == null) return null;

        var nodeType = node.GetType();

        if (_nodeViewMappings.TryGetValue(nodeType, out var nodeViewType))
        {
            return CreateNodeViewInstance(nodeViewType, node);
        }

        // Если нет прямого сопоставления, ищем через базовые типы (для абстрактных классов)
        var baseType = nodeType.BaseType;
        while (baseType != null)
        {
            if (baseType.IsAbstract && _nodeViewMappings.TryGetValue(baseType, out nodeViewType))
            {
                // Кэшируем результат для ускорения последующих вызовов
                _nodeViewMappings[nodeType] = nodeViewType;
                return CreateNodeViewInstance(nodeViewType, node);
            }
            baseType = baseType.BaseType;
        }

        // Если отображение не найдено – возвращаем базовый NodeView
        return new StoryNodeView(node);
    }

    // Создание экземпляра NodeView через рефлексию
    private static StoryNodeView CreateNodeViewInstance(Type nodeViewType, StoryNode node)
    {
        var constructor = nodeViewType.GetConstructor(new[] { typeof(StoryNode) });
        if (constructor != null)
        {
            return (StoryNodeView)constructor.Invoke(new object[] { node });
        }
        throw new MissingMethodException($"Constructor with parameter {nameof(StoryNode)} not found for {nodeViewType.Name}");
    }
}