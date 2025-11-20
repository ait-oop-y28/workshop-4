using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Presentation;

namespace Workshop4.Application.Pipelines.Nodes;

public interface IPipelineNode
{
    void Accept(IPipelineNodeVisitor visitor);

    Task<NodeExecutionResult> ExecuteAsync(JsonDocument input, IPipelinePresentationManager presentationManager);
}
