using Workshop4.Application.Pipelines.Models;
using Workshop4.Application.Pipelines.Nodes;
using Workshop4.Application.Pipelines.Operation;

namespace Workshop4.Presentation.Services;

public static class MockFactory
{
    public static readonly string MockInput = """
    [
      {
        "id": 1,
        "name": "Alice",
        "age": 30,
        "height": 170
      },
      {
        "id": 2,
        "name": "Bob",
        "age": 24,
        "height": 180
      }
    ]
    """;

    public static GroupNode CreatePipeline()
    {
        var pipeline = new GroupNode();

        var mapNode = new MappingNode();

        mapNode.AddProjection(new MappingNodeProjection
        {
            SourceFieldName = "id",
            TargetFieldName = "userId",
        });

        mapNode.AddProjection(new MappingNodeProjection
        {
            SourceFieldName = "name",
            TargetFieldName = "displayName",
        });

        mapNode.AddProjection(new MappingNodeProjection
        {
            TargetFieldName = "height",
        });

        var filterNode = new FilterNode
        {
            PropertyName = "age",
            FilterOperation = new GreaterThanOperation(),
            Value = "25",
        };

        var groupNode = new GroupNode { Name = "Users" };

        pipeline.AddNode(new EnabledNode(filterNode));
        pipeline.AddNode(new EnabledNode(mapNode));
        pipeline.AddNode(new EnabledNode(groupNode));

        return pipeline;
    }
}
