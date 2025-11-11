using System.Diagnostics.CodeAnalysis;

namespace Workshop4.Application.Json.Models;

public sealed record JsonObjectDocument(IReadOnlyCollection<JsonProperty> Properties) : JsonDocument
{
    public bool TryGetProperty(string propertyName, [NotNullWhen(true)] out JsonProperty? property)
    {
        property = Properties.SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        return property is not null;
    }
}
