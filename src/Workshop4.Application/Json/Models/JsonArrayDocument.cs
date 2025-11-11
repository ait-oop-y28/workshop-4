namespace Workshop4.Application.Json.Models;

public sealed record JsonArrayDocument(IReadOnlyCollection<JsonObjectDocument> Values) : JsonDocument;
