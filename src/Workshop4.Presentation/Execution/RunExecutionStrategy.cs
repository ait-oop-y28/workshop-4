using Workshop4.Application.Extensions;
using Workshop4.Application.Json;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Nodes;
using Workshop4.Application.Pipelines.Presentation;
using Workshop4.Presentation.Models;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Execution;

public sealed class RunExecutionStrategy : IExecutionStrategy
{
    private readonly IPipelinePresentationManager _presentationManager;

    private JsonDocument _workingDocument;

    public RunExecutionStrategy(IPipelinePresentationManager presentationManager, JsonDocument workingDocument)
    {
        _presentationManager = presentationManager;
        _workingDocument = workingDocument;
    }

    public async Task StartAsync()
    {
        AppState.Instance.ExecutionState = ExecutionState.Run;
        _presentationManager.ClearExecutionFrames();

        AppState.Instance.BackupNavigationStack();
        await _presentationManager.OnStateChaned();

        try
        {
            AppState.Instance.OutputText = JsonDocumentFormatter.FormatDocument(_workingDocument);

            IEnumerable<IPipelineNode> nodes = AppState.Instance.RootGroup.Enumerate();

            foreach (IPipelineNode node in nodes)
            {
                await _presentationManager.OnExecutingNodeChangedAsync(node);

                IPipelineCommand? command = node.TryCreateCommand();

                if (command is null)
                    continue;

                PipelineCommandExecutionResult nodeResult = command.Execute(_workingDocument);

                await Task.Delay(TimeSpan.FromMilliseconds(500));

                if (nodeResult is PipelineCommandExecutionResult.Success success)
                {
                    _presentationManager.AddExecutionFrame(new ExecutionFrame(node, _workingDocument));
                    _workingDocument = success.Document;

                    await _presentationManager.OnStateChaned();
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

            AppState.Instance.OutputText = JsonDocumentFormatter.FormatDocument(_workingDocument);
        }
        finally
        {
            AppState.Instance.ExecutionState = ExecutionState.None;
            await _presentationManager.OnExecutingNodeChangedAsync(null);

            AppState.Instance.RestoreNavigationStack();
            await _presentationManager.OnStateChaned();
        }
    }
}
