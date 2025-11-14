namespace Workshop4.Application.Pipelines.Operation;

public sealed class EqualsOperation : IFilterOperation
{
    public string Name => "=";

    public bool Execute<T>(T left, T right)
        where T : IEquatable<T>, IComparable<T>
    {
        return left.Equals(right);
    }
}
