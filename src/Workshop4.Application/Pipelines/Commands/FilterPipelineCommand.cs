using System.Diagnostics;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Operation;

namespace Workshop4.Application.Pipelines.Commands;

public sealed class FilterPipelineCommand : IPipelineCommand
{
    private readonly string _propertyName;
    private readonly IFilterOperation _filterOperation;
    private readonly string _value;

    public FilterPipelineCommand(string propertyName, IFilterOperation filterOperation, string value)
    {
        _propertyName = propertyName;
        _filterOperation = filterOperation;
        _value = value;
    }

    public PipelineCommandExecutionResult Execute(JsonDocument input)
    {
        if (input is JsonObjectDocument objectDocument)
        {
            NodeFilterResult result = FilterSingleNode(objectDocument);

            if (result is NodeFilterResult.Success success)
            {
                return new PipelineCommandExecutionResult.Success(success.IsApplicable
                    ? input
                    : new JsonNullDocument());
            }

            if (result is NodeFilterResult.Error error)
            {
                return new PipelineCommandExecutionResult.Failure(error.ErrorMessage);
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
                    return new PipelineCommandExecutionResult.Failure(error.ErrorMessage);
                }
                else
                {
                    throw new UnreachableException($"Unknown result = {result}");
                }
            }

            return new PipelineCommandExecutionResult.Success(new JsonArrayDocument(newObjects));
        }

        return new PipelineCommandExecutionResult.Failure($"Invalid input for filtering operation = {input}");
    }

    private NodeFilterResult FilterSingleNode(JsonObjectDocument obj)
    {
        if (obj.TryGetProperty(_propertyName, out JsonProperty? property) is false)
        {
            return new NodeFilterResult.Error($"Object does not contain property '{_propertyName}'");
        }

        if (property.Value is not JsonValue propertyValue)
        {
            return new NodeFilterResult.Error("Object property does not contain value");
        }

        bool filterTrue =
            decimal.TryParse(propertyValue.Value, out decimal leftNumber)
            && decimal.TryParse(_value, out decimal rightNumber)
            && (_filterOperation.Execute(leftNumber, rightNumber)
                || _filterOperation.Execute(propertyValue.Value, _value));

        return new NodeFilterResult.Success(filterTrue);
    }

    private abstract record NodeFilterResult
    {
        private NodeFilterResult() { }

        public sealed record Success(bool IsApplicable) : NodeFilterResult;

        public sealed record Error(string ErrorMessage) : NodeFilterResult;
    }
}
