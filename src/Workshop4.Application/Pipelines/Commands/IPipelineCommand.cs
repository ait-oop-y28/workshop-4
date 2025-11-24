using Workshop4.Application.Json.Models;

namespace Workshop4.Application.Pipelines.Commands;

public interface IPipelineCommand
{
    PipelineCommandExecutionResult Execute(JsonDocument input);
}
