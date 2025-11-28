using System.Collections;
using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Pipelines.Iterators;

public sealed class SingleNodeIterator : IPipelineIterator
{
    private bool _isFirstIteration = true;

    public SingleNodeIterator(IPipelineNode current)
    {
        Current = current;
    }

    object IEnumerator.Current => Current;

    public IPipelineIterator Clone()
        => new SingleNodeIterator(Current) { _isFirstIteration = _isFirstIteration };

    public IPipelineNode Current { get; }

    public bool MoveNext()
    {
        if (_isFirstIteration is false)
            return false;

        _isFirstIteration = false;
        return true;
    }

    public void Reset() { }

    public void Dispose() { }
}
