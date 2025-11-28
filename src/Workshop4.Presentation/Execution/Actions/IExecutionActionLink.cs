namespace Workshop4.Presentation.Execution.Actions;

public interface IExecutionActionLink
{
    IExecutionActionLink AddNext(IExecutionActionLink link);

    void Configure(ExecutionActionsBuilder builder, IExecutionStrategy strategy);
}
