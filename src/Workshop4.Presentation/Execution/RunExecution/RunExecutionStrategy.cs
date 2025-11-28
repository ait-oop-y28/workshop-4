using Workshop4.Application.Json;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Presentation;
using Workshop4.Presentation.Execution.Actions;
using Workshop4.Presentation.Models;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Execution.RunExecution;

public sealed class RunExecutionStrategy : IExecutionStrategy
{
    private readonly IPipelinePresentationManager _presentationManager;
    private readonly JsonDocument _workingDocument;

    public RunExecutionStrategy(IPipelinePresentationManager presentationManager, JsonDocument workingDocument)
    {
        _presentationManager = presentationManager;
        _workingDocument = workingDocument;
    }

    public IExecutionActionLink ActionLink => new NoopActionLink();

    public async Task StartAsync()
    {
        AppState.Instance.ExecutionState = ExecutionState.Run;
        _presentationManager.UpdateExecutionFrames([]);

        AppState.Instance.BackupNavigationStack();
        await _presentationManager.OnStateChanged();

        try
        {
            AppState.Instance.OutputText = JsonDocumentFormatter.FormatDocument(_workingDocument);

            var resultIterator = new PipelineExecutionIterator(
                _workingDocument,
                AppState.Instance.RootGroup,
                _presentationManager);

            while (await resultIterator.MoveNextAsync())
            {
                PipelineCommandExecutionResult nodeResult = resultIterator.Current;

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
}
