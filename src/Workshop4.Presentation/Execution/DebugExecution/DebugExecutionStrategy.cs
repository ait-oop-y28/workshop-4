using System.Diagnostics.CodeAnalysis;
using Workshop4.Application.Json;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Presentation;
using Workshop4.Presentation.Execution.Actions;
using Workshop4.Presentation.Models;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Execution.DebugExecution;

public sealed class DebugExecutionStrategy : IExecutionStrategy
{
    private readonly IPipelinePresentationManager _presentationManager;
    private readonly JsonDocument _workingDocument;

    private readonly Stack<ExecutionIteratorSnapshot> _iteratorSnapshots;

    [NotNull]
    private PipelineExecutionIterator? _iterator;

    public DebugExecutionStrategy(IPipelinePresentationManager presentationManager, JsonDocument workingDocument)
    {
        _presentationManager = presentationManager;
        _workingDocument = workingDocument;
        _iterator = null!;
        _iteratorSnapshots = [];
    }

    public bool StepBackDisabled => _iteratorSnapshots.Count is 0;

    public IExecutionActionLink ActionLink { get; } = new DebugBackwardActionLink()
        .AddNext(new DebugForwardActionLink())
        .AddNext(new DebugPassActionLink());

    public async Task StartAsync()
    {
        AppState.Instance.ExecutionState = ExecutionState.Debug;
        _presentationManager.UpdateExecutionFrames([]);

        AppState.Instance.BackupNavigationStack();
        await _presentationManager.OnStateChanged();

        AppState.Instance.OutputText = JsonDocumentFormatter.FormatDocument(_workingDocument);

        _iterator = new PipelineExecutionIterator(
            _workingDocument,
            AppState.Instance.RootGroup,
            _presentationManager);
    }

    public async Task StepForwardAsync()
    {
        _iteratorSnapshots.Push(_iterator.CreateSnapshot());

        if (await _iterator.MoveNextAsync() is false)
        {
            await FinishExecutionAsync();
            return;
        }

        PipelineCommandExecutionResult nodeResult = _iterator.Current;

        if (nodeResult is PipelineCommandExecutionResult.Success)
        {
            await _presentationManager.OnStateChanged();
        }
        else if (nodeResult is PipelineCommandExecutionResult.Failure fail)
        {
            AppState.Instance.OutputText = $"ERROR: {fail.ErrorMessage}";
            await FinishExecutionAsync();
        }
        else
        {
            AppState.Instance.OutputText = "Unknown result";
            await FinishExecutionAsync();
        }
    }

    public async Task StepBackwardAsync()
    {
        if (_iteratorSnapshots.TryPop(out ExecutionIteratorSnapshot? snapshot) is false)
            return;

        await _iterator.RestoreSnapshotAsync(snapshot);
    }

    public async Task PassAsync()
    {
        try
        {
            while (await _iterator.MoveNextAsync())
            {
                PipelineCommandExecutionResult nodeResult = _iterator.Current;

                if (nodeResult is PipelineCommandExecutionResult.Success)
                {
                    await _presentationManager.OnStateChanged();
                }
                else if (nodeResult is PipelineCommandExecutionResult.Failure fail)
                {
                    AppState.Instance.OutputText = $"ERROR: {fail.ErrorMessage}";
                    return;
                }
                else
                {
                    AppState.Instance.OutputText = "Unknown result";
                    return;
                }
            }
        }
        finally
        {
            AppState.Instance.ExecutionState = ExecutionState.None;
            await _presentationManager.OnExecutingNodeChangedAsync(null);

            AppState.Instance.RestoreNavigationStack();
            await _presentationManager.OnStateChanged();
        }
    }

    private async Task FinishExecutionAsync()
    {
        AppState.Instance.ExecutionState = ExecutionState.None;
        await _presentationManager.OnExecutingNodeChangedAsync(null);

        AppState.Instance.RestoreNavigationStack();
        await _presentationManager.OnStateChanged();
    }
}
