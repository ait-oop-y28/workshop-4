using System.Diagnostics;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Operation;
using Workshop4.Application.Pipelines.Presentation;

namespace Workshop4.Application.Pipelines.Nodes;

public sealed class FilterNode : IPipelineNode
{
    public FilterNode()
    {
        PropertyName = string.Empty;
        FilterOperation = new NoneOperation();
        Value = string.Empty;
    }

    public string PropertyName { get; set; }

    public IFilterOperation FilterOperation { get; set; }

    public string Value { get; set; }

    public void Accept(IPipelineNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task<NodeExecutionResult> ExecuteAsync(
        JsonDocument input,
        IPipelinePresentationManager presentationManager)
    {
        await presentationManager.OnExecutingNodeChangedAsync(this);
        await Task.Delay(TimeSpan.FromMilliseconds(500));

        if (input is JsonObjectDocument objectDocument)
        {
            NodeFilterResult result = FilterSingleNode(objectDocument);

            if (result is NodeFilterResult.Success success)
            {
                return new NodeExecutionResult.Success(success.IsApplicable
                    ? input
                    : new JsonNullDocument());
            }

            if (result is NodeFilterResult.Error error)
            {
                return new NodeExecutionResult.Failure(error.ErrorMessage);
            }

            throw new UnreachableException($"Unknown result = {result}");
        }

        if (input is JsonArrayDocument arrayDocument)
        {
            var newObjects = new List<JsonObjectDocument>();

            foreach (JsonObjectDocument obj in arrayDocument.Values)
            {
                NodeFilterResult result = FilterSingleNode(obj);

                if (result is NodeFilterResult.Success success)
                {
                    if (success.IsApplicable)
                        newObjects.Add(obj);
                }
                else if (result is NodeFilterResult.Error error)
                {
                    return new NodeExecutionResult.Failure(error.ErrorMessage);
                }
                else
                {
                    throw new UnreachableException($"Unknown result = {result}");
                }
            }

            return new NodeExecutionResult.Success(new JsonArrayDocument(newObjects));
        }

        return new NodeExecutionResult.Failure($"Invalid input for filtering operation = {input}");
    }

    private NodeFilterResult FilterSingleNode(JsonObjectDocument obj)
    {
        if (obj.TryGetProperty(PropertyName, out JsonProperty? property) is false)
        {
            return new NodeFilterResult.Error($"Object does not contain property '{PropertyName}'");
        }

        if (property.Value is not JsonValue propertyValue)
        {
            return new NodeFilterResult.Error("Object property does not contain value");
        }

        bool filterTrue =
            decimal.TryParse(propertyValue.Value, out decimal leftNumber)
            && decimal.TryParse(Value, out decimal rightNumber)
            && (FilterOperation.Execute(leftNumber, rightNumber)
                || FilterOperation.Execute(propertyValue.Value, Value));

        return new NodeFilterResult.Success(filterTrue);
    }

    private abstract record NodeFilterResult
    {
        private NodeFilterResult() { }

        public sealed record Success(bool IsApplicable) : NodeFilterResult;

        public sealed record Error(string ErrorMessage) : NodeFilterResult;
    }

    public override string ToString()
    {
        string field = string.IsNullOrWhiteSpace(PropertyName) ? "(field)" : PropertyName;
        string val = string.IsNullOrWhiteSpace(Value) ? "(value)" : Value;

        return $"Filter {field} {FilterOperation.Name} {val}";
    }
}
