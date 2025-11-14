namespace Workshop4.Application.Pipelines.Operation;

public sealed class GreaterThanOperation : IFilterOperation
{
    public string Name => ">";

    public bool Execute<T>(T left, T right)
        where T : IEquatable<T>, IComparable<T>
    {
        return left.CompareTo(right) > 0;
    }
}
