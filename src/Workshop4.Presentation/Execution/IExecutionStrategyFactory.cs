using Workshop4.Application.Json.Models;

namespace Workshop4.Presentation.Execution;

public interface IExecutionStrategyFactory
{
    IExecutionStrategy Create(JsonDocument input, ExecutionContext context);
}
