namespace MoreStructures.PriorityQueues;

/// <summary>
/// An item of type <typeparamref name="T"/> with a priority and a "push timestamp" assigned to it.
/// </summary>
/// <typeparam name="T">The type of the <paramref name="Item"/>.</typeparam>
/// <param name="Item">The item.</param>
/// <param name="Priority">The <see cref="int"/> defining the priority of the <paramref name="Item"/>.</param>
/// <param name="PushTimestamp">
/// A strictly monotonic increasing <see cref="int"/>, uniquely identifying an insertion of the <paramref name="Item"/>
/// in the queue via <see cref="IPriorityQueue{T}.Push(T, int)"/>. To be considered as offsets of
/// <paramref name="PushTimestampEra"/>.
/// </param>
/// <param name="PushTimestampEra">
/// The base for <paramref name="PushTimestamp"/>. 
/// <br/>
/// Same or equivalent <paramref name="PushTimestampEra"/> and same <paramref name="PushTimestamp"/> correspond to same
/// actual timestamps.
/// <br/>
/// Timestamps in different eras can never be equal (i.e. there is strict order of timestamps between eras).
/// </param>
public record struct PrioritizedItem<T>(T Item, int Priority, int PushTimestamp, PushTimestampEra PushTimestampEra) 
    : IComparable<PrioritizedItem<T>>
{
    /// <inheritdoc cref="PrioritizedItem{T}"/>
    public PrioritizedItem(T Item, int Priority, int PushTimestamp) :
        this(Item, Priority, PushTimestamp, new(0))
    { }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Comparison is made by <see cref="Priority"/> first: bigger <see cref="Priority"/> determines bigger instance.
    /// <br/>
    /// If the two instances have the same <see cref="Priority"/>, comparison is made by <see cref="PushTimestampEra"/>
    /// values: smaller <see cref="PushTimestampEra"/> determines higher priority instance.
    /// <br/>
    /// If the two instances have the same <see cref="PushTimestampEra"/>, comparison is made by 
    /// <see cref="PushTimestamp"/>: smaller <see cref="PushTimestamp"/> determines higher priority instance.
    /// <br/>
    /// If <see cref="Priority"/>, <see cref="PushTimestampEra"/> and <see cref="PushTimestamp"/> are the same, 
    /// returned value is 0.
    /// </remarks>
    public int CompareTo(PrioritizedItem<T> other)
    {
        var priorityDifference = Priority - other.Priority;
        if (priorityDifference != 0)
            return priorityDifference;
        var eraDifference = -PushTimestampEra.Era + other.PushTimestampEra.Era;
        if (eraDifference != 0)
            return eraDifference;
        return -PushTimestamp + other.PushTimestamp;
    }
}
