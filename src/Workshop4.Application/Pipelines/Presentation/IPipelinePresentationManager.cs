using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Pipelines.Presentation;

public interface IPipelinePresentationManager
{
    Task OnStateChaned();
    
    Task OnExecutingNodeChangedAsync(IPipelineNode? node);

    void ClearExecutionFrames();
    
    void AddExecutionFrame(ExecutionFrame frame);
}
