using System.Collections;
using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Pipelines.Iterators;

public sealed class GroupNodeIterator : IPipelineIterator
{
    private readonly Queue<IPipelineNode> _returnQueue;
    private readonly Stack<IPipelineIterator> _iteratorStack;

    private IPipelineNode? _current;

    public GroupNodeIterator(GroupNode node)
    {
        _returnQueue = [];
        _iteratorStack = [];

        _returnQueue.Enqueue(node);

        IEnumerable<IPipelineIterator> iterators = node.ChildNodes
            .Select(x => x.GetEnumerator())
            .Reverse();

        foreach (IPipelineIterator iterator in iterators)
        {
            _iteratorStack.Push(iterator);
        }
    }

    private GroupNodeIterator(
        IPipelineNode? current,
        IEnumerable<IPipelineNode> returnQueue,
        IEnumerable<IPipelineIterator> iteratorStack)
    {
        _current = current;
        _returnQueue = new Queue<IPipelineNode>(returnQueue);
        _iteratorStack = new Stack<IPipelineIterator>(iteratorStack);
    }

    object IEnumerator.Current => Current;

    public IPipelineIterator Clone()
    {
        return new GroupNodeIterator(
            _current,
            _returnQueue,
            _iteratorStack.Select(iterator => iterator.Clone()).Reverse());
    }

    public IPipelineNode Current
        => _current ?? throw new InvalidOperationException("No current node");

    public bool MoveNext()
    {
        IPipelineNode? next;

        while (_returnQueue.TryDequeue(out next) is false
               && _iteratorStack.TryPeek(out IPipelineIterator? iterator))
        {
            if (iterator.MoveNext())
            {
                _returnQueue.Enqueue(iterator.Current);
            }
            else
            {
                _iteratorStack.Pop();
            }
        }

        _current = next;
        return next is not null;
    }

    public void Reset()
    {
        throw new InvalidOperationException();
    }

    public void Dispose() { }
}
