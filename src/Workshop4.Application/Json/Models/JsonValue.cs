namespace Workshop4.Application.Json.Models;

public sealed record JsonValue(string Value) : JsonDocument
{
    public override void Accept(IJsonDocumentVisitor visitor)
    {
        visitor.Visit(this);
    }
}
