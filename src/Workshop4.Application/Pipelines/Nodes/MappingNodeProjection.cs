using System.Diagnostics.CodeAnalysis;
using Workshop4.Application.Json.Models;

namespace Workshop4.Application.Pipelines.Nodes;

public sealed record MappingNodeProjection
{
    public string SourceFieldName { get; set; } = string.Empty;

    public string TargetFieldName { get; set; } = string.Empty;

    public bool TryProjectProperty(JsonObjectDocument obj, [NotNullWhen(true)] out JsonProperty? property)
    {
        if (obj.TryGetProperty(SourceFieldName, out JsonProperty? jsonProperty) is false)
        {
            property = null;
            return false;
        }

        property = new JsonProperty(TargetFieldName, jsonProperty.Value);
        return true;
    }
}
