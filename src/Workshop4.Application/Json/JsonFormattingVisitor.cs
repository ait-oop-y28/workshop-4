using System.Text.Json.Nodes;
using Workshop4.Application.Json.Models;
using JsonValue = Workshop4.Application.Json.Models.JsonValue;

namespace Workshop4.Application.Json;

public sealed class JsonFormattingVisitor : IJsonDocumentVisitor
{
    private readonly Stack<JsonNode?> _nodes = [];

    public JsonNode? Node => _nodes.TryPeek(out JsonNode? node) ? node : null;

    public void Visit(JsonArrayDocument document)
    {
        foreach (JsonObjectDocument value in document.Values)
        {
            value.Accept(this);
        }

        var values = new JsonNode?[document.Values.Count];

        for (int i = document.Values.Count - 1; i >= 0; i--)
        {
            values[i] = _nodes.Pop();
        }

        _nodes.Push(new JsonArray(values));
    }

    public void Visit(JsonObjectDocument document)
    {
        foreach (JsonProperty property in document.Properties)
        {
            property.Value.Accept(this);
        }

        var props = new KeyValuePair<string, JsonNode?>[document.Properties.Count];

        for (int i = document.Properties.Count - 1; i >= 0; i--)
        {
            JsonProperty property = document.Properties[i];
            JsonNode? value = _nodes.Pop();

            props[i] = new(property.Name, value);
        }

        _nodes.Push(new JsonObject(props));
    }

    public void Visit(JsonValue document)
    {
        _nodes.Push(System.Text.Json.Nodes.JsonValue.Create(document.Value));
    }

    public void Visit(JsonNullDocument document)
    {
        _nodes.Push(null);
    }
}
