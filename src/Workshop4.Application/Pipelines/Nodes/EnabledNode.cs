using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Iterators;

namespace Workshop4.Application.Pipelines.Nodes;

public sealed class EnabledNode : IPipelineNode
{
    public EnabledNode(IPipelineNode wrapped)
    {
        Wrapped = wrapped;
        IsEnabled = true;
    }

    public override string ToString()
        => Wrapped.ToString() ?? string.Empty;

    public bool IsEnabled { get; set; }

    public IPipelineNode Wrapped { get; }

    public void Accept(IPipelineNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public IPipelineIterator GetEnumerator()
    {
        return IsEnabled
            ? Wrapped.GetEnumerator()
            : new NoOpIterator();
    }

    public IPipelineCommand? TryCreateCommand()
    {
        return IsEnabled
            ? Wrapped.TryCreateCommand()
            : null;
    }
}
