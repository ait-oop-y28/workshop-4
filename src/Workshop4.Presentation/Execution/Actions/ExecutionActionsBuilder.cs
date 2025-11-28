using Microsoft.AspNetCore.Components;

namespace Workshop4.Presentation.Execution.Actions;

public sealed class ExecutionActionsBuilder
{
    private readonly List<RenderFragment> _fragments = [];

    public IReadOnlyCollection<RenderFragment> Fragments => _fragments;

    public ExecutionActionsBuilder WithFragment(RenderFragment fragment)
    {
        _fragments.Add(fragment);
        return this;
    }
}
