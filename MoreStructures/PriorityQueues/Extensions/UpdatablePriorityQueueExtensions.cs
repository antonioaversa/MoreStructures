namespace MoreStructures.PriorityQueues.Extensions;

/// <summary>
/// Extension methods for <see cref="IUpdatablePriorityQueue{T}"/> implementations.
/// </summary>
public static class UpdatablePriorityQueueExtensions
{
    /// <summary>
    /// If the provided <paramref name="item"/> already is in the <paramref name="queue"/>, it updates its priority to
    /// the <paramref name="newPriority"/>. Otherwise, it pushes the <paramref name="item"/> in the 
    /// <paramref name="queue"/>, with the priority <paramref name="newPriority"/>.
    /// </summary>
    /// <typeparam name="T">The type of items of the <paramref name="queue"/>.</typeparam>
    /// <param name="queue">The <see cref="IUpdatablePriorityQueue{T}"/> instance to update.</param>
    /// <param name="item">The item of type <typeparamref name="T"/> to be pushed or updated.</param>
    /// <param name="newPriority">The new priority value.</param>
    /// <returns>
    /// The <see cref="PrioritizedItem{T}"/> entry before the update, or <see langword="null"/>, if there was no entry
    /// of <paramref name="item"/> before the update (and a push has been done instead).
    /// </returns>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - A <see cref="IUpdatablePriorityQueue{T}.Remove(T)"/> of the provided <paramref name="item"/> is 
    ///       attempted.
    ///       <br/>
    ///     - Then, a <see cref="IPriorityQueue{T}.Push(T, int)"/> of the same <paramref name="item"/> is made, with 
    ///       the <paramref name="newPriority"/>.
    ///       <br/>
    ///     - Finally, the removed item, if any, is returned as result.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Time Complexity is O(Tremove + Tpush) and Space Complexity is O(Sremove + Spush), where Tremove and 
    ///       Sremove are the time and space cost of <see cref="IUpdatablePriorityQueue{T}.Remove(T)"/> and Tpush and
    ///       Spush are the time and space cost of <see cref="IPriorityQueue{T}.Push(T, int)"/>, respectively.
    ///     </para>
    /// </remarks>
    public static PrioritizedItem<T>? PushOrUpdate<T>(this IUpdatablePriorityQueue<T> queue, T item, int newPriority)
        where T : notnull
    {
        var removedItem = queue.Remove(item);
        queue.Push(item, newPriority);
        return removedItem;
    }
}
