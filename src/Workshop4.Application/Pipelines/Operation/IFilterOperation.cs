namespace Workshop4.Application.Pipelines.Operation;

public interface IFilterOperation
{
    string Name { get; }

    bool Execute<T>(T left, T right)
        where T : IEquatable<T>, IComparable<T>;
}
