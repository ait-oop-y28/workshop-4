using Workshop4.Application.Json.Models;

namespace Workshop4.Presentation.Execution.RunExecution;

public sealed class RunExecutionStrategyFactory : IExecutionStrategyFactory
{
    public IExecutionStrategy Create(JsonDocument input, ExecutionContext context)
    {
        return new RunExecutionStrategy(
            context.PresentationManager,
            input);
    }
}
