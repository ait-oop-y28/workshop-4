using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Pipelines.Presentation;

public interface IPipelinePresentationManager
{
    void OnExecutingNodeChanged(IPipelineNode node);
}
