using System.Diagnostics.CodeAnalysis;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Pipelines.Commands;

public sealed class MapPipelineCommand : IPipelineCommand
{
    private readonly IReadOnlyCollection<MappingNodeProjection> _projections;

    public MapPipelineCommand(IReadOnlyCollection<MappingNodeProjection> projections)
    {
        _projections = projections;
    }

    public PipelineCommandExecutionResult Execute(JsonDocument input)
    {
        if (input is JsonObjectDocument obj)
        {
            return TryCreateProjectedObject(obj, out JsonObjectDocument? projectedObject)
                ? new PipelineCommandExecutionResult.Success(projectedObject)
                : new PipelineCommandExecutionResult.Failure("Failed to map object");
        }

        if (input is JsonArrayDocument arr)
        {
            var mappedObjects = new List<JsonObjectDocument>();

            foreach (JsonObjectDocument value in arr.Values)
            {
                if (TryCreateProjectedObject(value, out JsonObjectDocument? projectedObject))
                {
                    mappedObjects.Add(projectedObject);
                }
                else
                {
                    return new PipelineCommandExecutionResult.Failure("Failed to map object");
                }
            }

            return new PipelineCommandExecutionResult.Success(new JsonArrayDocument(mappedObjects));
        }

        return new PipelineCommandExecutionResult.Failure($"Invalid input for mapping operation = {input}");
    }

    private bool TryCreateProjectedObject(
        JsonObjectDocument obj,
        [NotNullWhen(true)] out JsonObjectDocument? projectedObject)
    {
        var properties = new List<JsonProperty>();

        foreach (MappingNodeProjection projection in _projections)
        {
            if (projection.TryProjectProperty(obj, out JsonProperty? property))
            {
                properties.Add(property);
            }
            else
            {
                projectedObject = null;
                return false;
            }
        }

        projectedObject = new JsonObjectDocument(properties);
        return true;
    }
}
