using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Pipelines.Presentation;

public interface IPipelinePresentationManager
{
    Task OnStateChanged();
    
    Task OnExecutingNodeChangedAsync(IPipelineNode? node);

    void UpdateExecutionFrames(Stack<ExecutionFrame> frames);
}
