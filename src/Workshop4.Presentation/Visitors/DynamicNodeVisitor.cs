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

    public DynamicNodeVisitor(EventCallback<GroupNode> onGroupOpened)
    {
        _onGroupOpened = onGroupOpened;
        _actions = [];
    }

    public RenderFragment? NodeComponent { get; private set; }

    public IReadOnlyCollection<RenderFragment> Actions => _actions;

    public void Visit(FilterNode node)
    {
        NodeComponent = DynamicRenderFragmentFactory.Create<FilterNodeComponent>(new()
        {
            [nameof(FilterNodeComponent.Node)] = node,
        });
    }

    public void Visit(MappingNode node)
    {
        NodeComponent = DynamicRenderFragmentFactory.Create<MapNodeComponent>(new()
        {
            [nameof(MapNodeComponent.Node)] = node,
        });
    }

    public void Visit(GroupNode node)
    {
        NodeComponent = DynamicRenderFragmentFactory.Create<GroupNodeComponent>(new()
        {
            [nameof(GroupNodeComponent.Node)] = node,
            [nameof(GroupNodeComponent.OnEdit)] = _onGroupOpened,
        });
    }

    public void Visit(EnabledNode node)
    {
        _actions.Add(DynamicRenderFragmentFactory.Create<EnabledNodeActionComponent>(new()
        {
            [nameof(EnabledNodeActionComponent.Node)] = node,
        }));

        node.Wrapped.Accept(this);
    }
}
