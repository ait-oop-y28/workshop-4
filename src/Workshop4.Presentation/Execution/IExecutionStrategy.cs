using Workshop4.Presentation.Execution.Actions;

namespace Workshop4.Presentation.Execution;

public interface IExecutionStrategy
{
    IExecutionActionLink ActionLink { get; }
    
    Task StartAsync();
}
