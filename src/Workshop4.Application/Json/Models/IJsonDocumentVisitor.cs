namespace Workshop4.Application.Json.Models;

public interface IJsonDocumentVisitor
{
    void Visit(JsonArrayDocument document);

    void Visit(JsonNullDocument document);

    void Visit(JsonObjectDocument document);

    void Visit(JsonValue document);
}
