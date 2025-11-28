using Workshop4.Application.Json;
using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Iterators;
using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Nodes;
using Workshop4.Application.Pipelines.Presentation;
using Workshop4.Presentation.Services;

namespace Workshop4.Presentation.Execution;

public sealed class PipelineExecutionIterator : IAsyncEnumerator<PipelineCommandExecutionResult>
{
    private readonly IPipelinePresentationManager _presentationManager;

    private JsonDocument _workingDocument;

    private IPipelineIterator _iterator;
    private Stack<ExecutionFrame> _executionHistory;

    private PipelineCommandExecutionResult? _current;

    public PipelineExecutionIterator(
        JsonDocument workingDocument,
        IPipelineNode startNode,
        IPipelinePresentationManager presentationManager)
    {
        _workingDocument = workingDocument;
        _presentationManager = presentationManager;
        _iterator = startNode.GetEnumerator();

        _executionHistory = [];
    }

    public PipelineCommandExecutionResult Current
        => _current ?? throw new InvalidOperationException("Current is not specified");

    public async ValueTask<bool> MoveNextAsync()
    {
        while (_iterator.MoveNext())
        {
            IPipelineNode node = _iterator.Current;
            await _presentationManager.OnExecutingNodeChangedAsync(node);

            IPipelineCommand? command = node.TryCreateCommand();

            if (command is null)
                continue;

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            _executionHistory.Push(new ExecutionFrame(node, _workingDocument));
            _presentationManager.UpdateExecutionFrames(_executionHistory);

            _current = command.Execute(_workingDocument);

            if (_current is PipelineCommandExecutionResult.Success success)
            {
                _workingDocument = success.Document;
                AppState.Instance.OutputText = JsonDocumentFormatter.FormatDocument(_workingDocument);
            }

            return true;
        }

        return false;
    }

    public ValueTask DisposeAsync()
    {
        _iterator.Dispose();
        return ValueTask.CompletedTask;
    }

    public ExecutionIteratorSnapshot CreateSnapshot()
    {
        return new ExecutionIteratorSnapshot(
            _iterator.Clone(),
            ExecutionFrames: _executionHistory.ToArray(),
            _workingDocument);
    }

    public async Task RestoreSnapshotAsync(ExecutionIteratorSnapshot snapshot)
    {
        _iterator = snapshot.Iterator;
        _executionHistory = new Stack<ExecutionFrame>(snapshot.ExecutionFrames);
        _workingDocument = snapshot.WorkingDocument;

        _executionHistory.TryPeek(out ExecutionFrame? executionFrame);
        await _presentationManager.OnExecutingNodeChangedAsync(executionFrame?.Node);

        AppState.Instance.OutputText = JsonDocumentFormatter.FormatDocument(_workingDocument);
        _presentationManager.UpdateExecutionFrames(_executionHistory);

        await _presentationManager.OnStateChanged();
    }
}
