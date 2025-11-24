using Workshop4.Application.Json.Models;

namespace Workshop4.Application.Pipelines.Commands;

public abstract record PipelineCommandExecutionResult
{
    private PipelineCommandExecutionResult() { }

    public sealed record Success(JsonDocument Document) : PipelineCommandExecutionResult;

    public sealed record Failure(string ErrorMessage) : PipelineCommandExecutionResult;
}
