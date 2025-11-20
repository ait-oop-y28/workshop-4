namespace Workshop4.Application.Json.Models;

public sealed record JsonArrayDocument(IReadOnlyCollection<JsonObjectDocument> Values) : JsonDocument
{
    public override void Accept(IJsonDocumentVisitor visitor)
    {
        visitor.Visit(this);
    }
}
