namespace Workshop4.Presentation.Execution.Actions;

public sealed class NoopActionLink : ExecutionActionLinkBase
{
    public override void Configure(ExecutionActionsBuilder builder, IExecutionStrategy strategy)
    {
        Next?.Configure(builder, strategy);
    }
}
