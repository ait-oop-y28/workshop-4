using Workshop4.Presentation.Components.ExecutionActions;
using Workshop4.Presentation.Execution.DebugExecution;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Execution.Actions;

public sealed class DebugPassActionLink : ExecutionActionLinkBase
{
    public override void Configure(ExecutionActionsBuilder builder, IExecutionStrategy strategy)
    {
        if (strategy is DebugExecutionStrategy debugExecutionStrategy)
        {
            builder.WithFragment(DynamicRenderFragmentFactory.Create<DebugPassActionComponent>(new()
            {
                [nameof(DebugPassActionComponent.Strategy)] = debugExecutionStrategy,
            }));
        }

        Next?.Configure(builder, strategy);
    }
}
