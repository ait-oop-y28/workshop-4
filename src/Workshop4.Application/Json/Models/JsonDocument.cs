namespace Workshop4.Application.Json.Models;

public abstract record JsonDocument
{
    public abstract void Accept(IJsonDocumentVisitor visitor);
}
