using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Iterators;

namespace Workshop4.Application.Pipelines.Nodes;

public interface IPipelineNode
{
    void Accept(IPipelineNodeVisitor visitor);

    IPipelineIterator GetEnumerator();

    IPipelineCommand? TryCreateCommand();
}
