namespace Workshop4.Application.Json.Models;

public sealed record JsonNullDocument : JsonDocument
{
    public override void Accept(IJsonDocumentVisitor visitor)
    {
        visitor.Visit(this);
    }
}
