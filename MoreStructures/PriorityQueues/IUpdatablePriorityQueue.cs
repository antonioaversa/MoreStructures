namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> which supports priority update and item removal.
/// </summary>
public interface IUpdatablePriorityQueue<T> : IPriorityQueue<T>
    where T : notnull
{
    /// <summary>
    /// Returns all the priorities with which <paramref name="item"/> is registered in the queue, sorted by highest
    /// to smallest.
    /// </summary>
    /// <param name="item">The item, to retrieve priorities of.</param>
    /// <returns>The sequence of <see cref="int"/> values, each being a priority.</returns>
    IEnumerable<int> GetPrioritiesOf(T item);

    /// <summary>
    /// Updates the priority of the provided <paramref name="item"/>, without changing the timestamp.
    /// </summary>
    /// <param name="item">The item, to update the priority of. Must be present in the queue.</param>
    /// <param name="newPriority">The new priority to be assigned to <paramref name="item"/>.</param>
    /// <returns>
    /// The <see cref="PrioritizedItem{T}"/> with which <paramref name="item"/> was present in the queue, before being 
    /// updated.
    /// </returns>
    PrioritizedItem<T> UpdatePriority(T item, int newPriority);

    /// <summary>
    /// Removes the first occurrence of the provided <paramref name="item"/> from the queue.
    /// </summary>
    /// <param name="item">The item to be removed.</param>
    /// <returns>
    /// The <see cref="PrioritizedItem{T}"/> with which <paramref name="item"/> was present in the queue, before being 
    /// removed, or <see langword="null"/>, if no occurrence of <paramref name="item"/> was found.
    /// </returns>
    PrioritizedItem<T>? Remove(T item);
}