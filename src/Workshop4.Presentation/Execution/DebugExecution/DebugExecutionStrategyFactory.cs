using Workshop4.Application.Json.Models;

namespace Workshop4.Presentation.Execution.DebugExecution;

public sealed class DebugExecutionStrategyFactory : IExecutionStrategyFactory
{
    public IExecutionStrategy Create(JsonDocument input, ExecutionContext context)
    {
        return new DebugExecutionStrategy(context.PresentationManager, input);
    }
}
