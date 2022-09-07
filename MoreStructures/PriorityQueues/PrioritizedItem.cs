namespace MoreStructures.PriorityQueues;

/// <summary>
/// An item of type <typeparamref name="T"/> with a priority and a "push timestamp" assigned to it.
/// </summary>
/// <typeparam name="T">The type of the <paramref name="Item"/>.</typeparam>
/// <param name="Item">The item.</param>
/// <param name="Priority">The <see cref="int"/> defining the priority of the <paramref name="Item"/>.</param>
/// <param name="PushTimestamp">
/// A strictly monotonic increasing <see cref="int"/>, uniquely identifying an insertion of the <paramref name="Item"/>
/// in the queue via <see cref="IPriorityQueue{T}.Push(T, int)"/>.
/// </param>
public record struct PrioritizedItem<T>(T Item, int Priority, int PushTimestamp) : IComparable<PrioritizedItem<T>>
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Comparison is made by <see cref="Priority"/> first: bigger <see cref="Priority"/> determines bigger instance.
    /// <br/>
    /// If the two instances have the same <see cref="Priority"/>, comparison is made by <see cref="PushTimestamp"/>:
    /// smaller <see cref="PushTimestamp"/> determines bigger instance.
    /// <br/>
    /// If both <see cref="Priority"/> and <see cref="PushTimestamp"/> are the same, returned value is 0.
    /// </remarks>
    public int CompareTo(PrioritizedItem<T> other)
    {
        var priorityDifference = Priority - other.Priority;
        if (priorityDifference != 0)
            return priorityDifference;
        return -PushTimestamp + other.PushTimestamp;
    }
}
