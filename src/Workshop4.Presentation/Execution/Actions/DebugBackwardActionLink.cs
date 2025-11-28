using Workshop4.Presentation.Components.ExecutionActions;
using Workshop4.Presentation.Execution.DebugExecution;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Execution.Actions;

public sealed class DebugBackwardActionLink : ExecutionActionLinkBase
{
    public override void Configure(ExecutionActionsBuilder builder, IExecutionStrategy strategy)
    {
        if (strategy is DebugExecutionStrategy debugExecutionStrategy)
        {
            builder.WithFragment(DynamicRenderFragmentFactory.Create<DebugBackwardActionComponent>(new()
            {
                [nameof(DebugBackwardActionComponent.Strategy)] = debugExecutionStrategy,
            }));
        }

        Next?.Configure(builder, strategy);
    }
}
