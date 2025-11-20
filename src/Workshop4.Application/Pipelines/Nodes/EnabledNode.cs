using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Presentation;

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

    public async Task<NodeExecutionResult> ExecuteAsync(
        JsonDocument input,
        IPipelinePresentationManager presentationManager)
    {
        return IsEnabled
            ? await Wrapped.ExecuteAsync(input, presentationManager)
            : new NodeExecutionResult.Success(input);
    }
}
