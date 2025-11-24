using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Iterators;
using Workshop4.Application.Pipelines.Operation;

namespace Workshop4.Application.Pipelines.Nodes;

public sealed class FilterNode : IPipelineNode
{
    public FilterNode()
    {
        PropertyName = string.Empty;
        FilterOperation = new NoneOperation();
        Value = string.Empty;
    }

    public string PropertyName { get; set; }

    public IFilterOperation FilterOperation { get; set; }

    public string Value { get; set; }

    public void Accept(IPipelineNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public IPipelineIterator GetEnumerator()
    {
        return new SingleNodeIterator(this);
    }

    public IPipelineCommand TryCreateCommand()
    {
        return new FilterPipelineCommand(PropertyName, FilterOperation, Value);
    }

    public override string ToString()
    {
        string field = string.IsNullOrWhiteSpace(PropertyName) ? "(field)" : PropertyName;
        string val = string.IsNullOrWhiteSpace(Value) ? "(value)" : Value;

        return $"Filter {field} {FilterOperation.Name} {val}";
    }
}
