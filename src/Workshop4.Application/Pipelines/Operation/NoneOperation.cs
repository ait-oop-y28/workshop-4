namespace Workshop4.Application.Pipelines.Operation;

public sealed class NoneOperation : IFilterOperation
{
    public string Name => "None";

    public bool Execute<T>(T left, T right)
        where T : IEquatable<T>, IComparable<T>
    {
        return true;
    }
}
