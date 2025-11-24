using Workshop4.Application.Pipelines.Commands;
using Workshop4.Application.Pipelines.Iterators;

namespace Workshop4.Application.Pipelines.Nodes;

public sealed class MappingNode : IPipelineNode
{
    private readonly List<MappingNodeProjection> _projections = [];

    public IReadOnlyCollection<MappingNodeProjection> Projections => _projections;

    public void AddProjection(MappingNodeProjection projection)
    {
        _projections.Add(projection);
    }

    public void RemoveProjection(MappingNodeProjection projection)
    {
        _projections.Remove(projection);
    }

    public void Accept(IPipelineNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public IPipelineIterator GetEnumerator()
    {
        return new SingleNodeIterator(this);
    }

    public IPipelineCommand TryCreateCommand()
    {
        return new MapPipelineCommand(_projections);
    }

    public override string ToString()
    {
        if (Projections.Count == 0)
            return "Map (no projections)";

        IEnumerable<string> pairs = Projections
            .Select(projection => $"{projection.SourceFieldName}->{projection.TargetFieldName}");

        var joined = string.Join(", ", pairs);
        return $"Map {joined}";
    }
}
