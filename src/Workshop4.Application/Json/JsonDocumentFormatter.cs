using Workshop4.Application.Json.Models;

namespace Workshop4.Application.Json;

public static class JsonDocumentFormatter
{
    public static string FormatDocument(JsonDocument doc)
    {
        var visitor = new JsonFormattingVisitor();
        doc.Accept(visitor);

        return visitor.Node?.ToString() ?? "null";
    }
}
