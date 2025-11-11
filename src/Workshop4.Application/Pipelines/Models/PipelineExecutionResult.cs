using Workshop4.Application.Json.Models;

namespace Workshop4.Application.Pipelines.Models;

public abstract record PipelineExecutionResult
{
    private PipelineExecutionResult() { }

    public sealed record Success(JsonDocument Document) : PipelineExecutionResult;

    public sealed record Failure(string ErrorMessage) : PipelineExecutionResult;
}
