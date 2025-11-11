using Workshop4.Application.Json.Models;

namespace Workshop4.Application.Pipelines.Models;

public abstract record NodeExecutionResult
{
    private NodeExecutionResult() { }

    public sealed record Success(JsonDocument Document) : NodeExecutionResult;

    public sealed record Failure(string ErrorMessage) : NodeExecutionResult;
}
