using System.Diagnostics;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Nodes;
using Workshop4.Application.Pipelines.Presentation;

namespace Workshop4.Application.Pipelines;

public sealed class PipelineDefinition
{
    private readonly List<IPipelineNode> _nodes = [];

    public IReadOnlyCollection<IPipelineNode> Nodes => _nodes;

    public void AddNode(IPipelineNode node)
    {
        _nodes.Add(node);
    }

    public void RemoveNode(IPipelineNode node)
    {
        _nodes.Remove(node);
    }

    public async Task<PipelineExecutionResult> ExecuteAsync(
        JsonDocument document,
        IPipelinePresentationManager presentationManager)
    {
        foreach (IPipelineNode node in Nodes)
        {
            NodeExecutionResult result = await node.ExecuteAsync(document, presentationManager);

            if (result is NodeExecutionResult.Success success)
            {
                document = success.Document;
            }
            else if (result is NodeExecutionResult.Failure failure)
            {
                return new PipelineExecutionResult.Failure(failure.ErrorMessage);
            }
            else
            {
                throw new UnreachableException($"Unknown result = {result}");
            }
        }

        return new PipelineExecutionResult.Success(document);
    }
}
