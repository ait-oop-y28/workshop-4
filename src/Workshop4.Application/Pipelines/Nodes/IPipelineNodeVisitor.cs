namespace Workshop4.Application.Pipelines.Nodes;

public interface IPipelineNodeVisitor
{
    void Visit(FilterNode node);

    void Visit(GroupNode node);

    void Visit(MappingNode node);

    void Visit(EnabledNode node);
}
