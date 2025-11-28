using Microsoft.AspNetCore.Components;
using Workshop4.Application.Pipelines.Nodes;
using Workshop4.Presentation.Components.NodeActions;
using Workshop4.Presentation.Components.Nodes;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Visitors;

public sealed class DynamicNodeVisitor : IPipelineNodeVisitor
{
    private readonly EventCallback<GroupNode> _onGroupOpened;
    private readonly List<RenderFragment> _actions;
    private readonly IPipelineNode? _currentNode;

    public DynamicNodeVisitor(EventCallback<GroupNode> onGroupOpened, IPipelineNode? currentNode)
    {
        _onGroupOpened = onGroupOpened;
        _currentNode = currentNode;
        _actions = [];
    }

    public RenderFragment? NodeComponent { get; private set; }

    public IReadOnlyCollection<RenderFragment> Actions => _actions;

    public bool IsCurrentNode { get; private set; }

    public void Visit(FilterNode node)
    {
        NodeComponent = DynamicRenderFragmentFactory.Create<FilterNodeComponent>(new()
        {
            [nameof(FilterNodeComponent.Node)] = node,
        });

        IsCurrentNode |= ReferenceEquals(node, _currentNode);
    }

    public void Visit(MappingNode node)
    {
        NodeComponent = DynamicRenderFragmentFactory.Create<MapNodeComponent>(new()
        {
            [nameof(MapNodeComponent.Node)] = node,
        });

        IsCurrentNode |= ReferenceEquals(node, _currentNode);
    }

    public void Visit(GroupNode node)
    {
        NodeComponent = DynamicRenderFragmentFactory.Create<GroupNodeComponent>(new()
        {
            [nameof(GroupNodeComponent.Node)] = node,
            [nameof(GroupNodeComponent.OnEdit)] = _onGroupOpened,
        });

        IsCurrentNode |= ReferenceEquals(node, _currentNode);
    }

    public void Visit(EnabledNode node)
    {
        _actions.Add(DynamicRenderFragmentFactory.Create<EnabledNodeActionComponent>(new()
        {
            [nameof(EnabledNodeActionComponent.Node)] = node,
        }));

        IsCurrentNode |= ReferenceEquals(node, _currentNode);
        node.Wrapped.Accept(this);
    }
}
