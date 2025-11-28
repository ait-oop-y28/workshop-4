using System.Collections;
using Workshop4.Application.Pipelines.Nodes;

namespace Workshop4.Application.Pipelines.Iterators;

public sealed class NoOpIterator : IPipelineIterator
{
    object IEnumerator.Current => Current;
    
    public IPipelineIterator Clone() => this;

    public IPipelineNode Current => throw new InvalidOperationException();

    public bool MoveNext() => false;

    public void Reset() => throw new InvalidOperationException();

    public void Dispose() { }
}
