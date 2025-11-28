using Workshop4.Presentation.Components.ExecutionActions;
using Workshop4.Presentation.Execution.DebugExecution;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Execution.Actions;

public sealed class DebugForwardActionLink : ExecutionActionLinkBase
{
    public override void Configure(ExecutionActionsBuilder builder, IExecutionStrategy strategy)
    {
        if (strategy is DebugExecutionStrategy debugExecutionStrategy)
        {
            builder.WithFragment(DynamicRenderFragmentFactory.Create<DebugForwardActionComponent>(new()
            {
                [nameof(DebugForwardActionComponent.Strategy)] = debugExecutionStrategy,
            }));
        }

        Next?.Configure(builder, strategy);
    }
}
