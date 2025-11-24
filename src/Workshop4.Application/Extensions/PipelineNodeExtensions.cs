using Workshop4.Application.Pipelines.Iterators;
using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Extensions;

public static class PipelineNodeExtensions
{
    public static IEnumerable<IPipelineNode> Enumerate(this IPipelineNode node)
    {
        using IPipelineIterator iterator = node.GetEnumerator();

        while (iterator.MoveNext())
        {
            yield return iterator.Current;
        }
    }
}
