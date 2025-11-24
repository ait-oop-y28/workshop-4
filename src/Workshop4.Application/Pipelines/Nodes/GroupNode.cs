using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Iterators;

namespace Workshop4.Application.Pipelines.Nodes;

public sealed class GroupNode : IPipelineNode
{
    private readonly List<IPipelineNode> _childNodes = [];

    public string Name { get; set; } = string.Empty;

    public IReadOnlyCollection<IPipelineNode> ChildNodes => _childNodes;

    public void AddNode(IPipelineNode node)
    {
        _childNodes.Add(node);
    }

    public void RemoveNode(IPipelineNode node)
    {
        _childNodes.Remove(node);
    }

    public void Accept(IPipelineNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public IPipelineIterator GetEnumerator()
    {
        return new GroupNodeIterator(this);
    }

    public IPipelineCommand? TryCreateCommand()
    {
        return null;
    }

    public override string ToString()
    {
        string name = string.IsNullOrWhiteSpace(Name) ? "Group" : Name;
        return $"Group '{name}'";
    }
}
