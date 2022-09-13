namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> which supports efficient peek of the item stored in the queue with the k-highest
/// priority.
/// </summary>
/// <remarks>
/// <see cref="PeekKth(int)"/> can be implemented in a general way by copying the entire data structure and then
/// performing k <see cref="IPriorityQueue{T}.Pop"/>, followed by a single <see cref="IPriorityQueue{T}.Peek"/>.
/// <br/>
/// This approach is however expensive both in time and space, having O(n) Time and Space Complexity for all known 
/// implementation of <see cref="IPriorityQueue{T}"/>..
/// <br/>
/// Implementing this interface can take advantage of the properties of the underlying data structure implementing the
/// priority queue, and providing better-than-linear performance.
/// </remarks>
public interface IPeekKthPriorityQueue<T> : IPriorityQueue<T>
{
    /// <summary>
    /// Retrieves the item of the queue with highest priority, without extracting any of the items in the queue.
    /// </summary>
    /// <param name="k">The non-negative priority rank: 0 means highest priority, 1 second highest, etc.</param>
    /// <returns>The <see cref="PrioritizedItem{T}"/> with k-th highest priority if any. Null otherwise.</returns>
    /// <remarks>
    /// <see cref="PeekKth(int)"/> with <c>k == 0</c> is equivalent to <see cref="IPriorityQueue{T}.Peek"/>.
    /// </remarks>
    PrioritizedItem<T>? PeekKth(int k);
}