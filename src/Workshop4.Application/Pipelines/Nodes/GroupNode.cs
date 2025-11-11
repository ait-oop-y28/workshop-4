using System.Diagnostics;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Presentation;

namespace Workshop4.Application.Pipelines.Nodes;

public sealed class GroupNode : IPipelineNode
{
    private readonly HashSet<IPipelineNode> _childNodes = [];

    public bool IsEnabled { get; set; }

    public IReadOnlyCollection<IPipelineNode> ChildNodes => _childNodes;

    public void AddNode(IPipelineNode node)
    {
        _childNodes.Add(node);
    }

    public void RemoveNode(IPipelineNode node)
    {
        _childNodes.Remove(node);
    }

    public async Task<NodeExecutionResult> ExecuteAsync(
        JsonDocument input,
        IPipelinePresentationManager presentationManager)
    {
        if (IsEnabled is false)
            return new NodeExecutionResult.Success(input);
        
        foreach (IPipelineNode node in _childNodes)
        {
            NodeExecutionResult result = await node.ExecuteAsync(input, presentationManager);

            if (result is NodeExecutionResult.Success success)
            {
                input = success.Document;
            }
            else if (result is NodeExecutionResult.Failure failure)
            {
                return failure;
            }
            else
            {
                throw new UnreachableException($"Unknown result = {result}");
            }
        }

        return new NodeExecutionResult.Success(input);
    }
}
