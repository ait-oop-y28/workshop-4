namespace Workshop4.Presentation.Execution.Actions;

public abstract class ExecutionActionLinkBase : IExecutionActionLink
{
    protected IExecutionActionLink? Next { get; private set; }

    public IExecutionActionLink AddNext(IExecutionActionLink link)
    {
        if (Next is null)
        {
            Next = link;
        }
        else
        {
            Next.AddNext(link);
        }

        return this;
    }

    public abstract void Configure(ExecutionActionsBuilder builder, IExecutionStrategy strategy);
}
