namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> which supports priority update and item removal.
/// </summary>
public interface IUpdatablePriorityQueue<T> : IPriorityQueue<T>
    where T : notnull
{
    /// <summary>
    /// Returns the highest priority of the provided <paramref name="item"/>, if any. Null otherwise.
    /// </summary>
    /// <param name="item">The item, to look for in the queue.</param>
    /// <returns>The highest priority value or <see langword="null"/>.</returns>
    int? GetHighestPriorityOf(T item);

    /// <summary>
    /// Returns all the priorities with which <paramref name="item"/> is registered in the queue.
    /// </summary>
    /// <param name="item">The item, to retrieve priorities of.</param>
    /// <returns>A <see cref="IPriorityQueue{T}"/> of <see cref="int"/> values, each being a priority.</returns>
    IPriorityQueue<int> GetPrioritiesOf(T item);

    /// <summary>
    /// Updates the priority of the provided <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item, to update the priority of. Must be present in the queue.</param>
    /// <param name="newPriority">The new priority to be assigned to <paramref name="item"/>.</param>
    /// <returns>The old priority of the <paramref name="item"/>.</returns>
    int UpdatePriority(T item, int newPriority);

    /// <summary>
    /// Removes the first occurrence of the provided <paramref name="item"/> from the queue.
    /// </summary>
    /// <param name="item">The item to be removed.</param>
    /// <returns>
    /// The priority with which <paramref name="item"/> was present in the queue, before being removed, or 
    /// <see langword="null"/>, if no occurrence of <paramref name="item"/> was found.
    /// </returns>
    ItemAndPriority<T>? Remove(T item);
}