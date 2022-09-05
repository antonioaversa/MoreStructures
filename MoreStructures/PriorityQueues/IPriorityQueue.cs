namespace MoreStructures.PriorityQueues;

/// <summary>
/// Defines the interface common to all <b>Priority Queue</b> implementations.
/// </summary>
/// <typeparam name="T">The type of items, the queue is composed of.</typeparam>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     - A Priority Queue is a data structure storing items of a generic type <typeparamref name="T"/>, together with
///       their <b>priority</b>.
///       <br/>
///     - Duplicates, both for items and their priorities, are supported.
///       <br/>
///     - Priorities are defined as <see cref="int"/> values: the bigger the value, the higher is the priority of the
///       item.
///       <br/>
///     - The core operations of any Priority Queue are <b>insertion</b> of an item with related priority, 
///       <b>extraction</b> of the item with the highest priority and testing for empty.
///       <br/>
///     - Other operations are provided, such as <b>total count</b> of items and <b>peeking</b> (retrieval without
///       extraction).
///       <br/>
///     - More advanced operations, requiring auxiliary data structures for their implementation, are not specified
///       by this interface: examples are <b>priority update</b> and <b>removal</b> of an item.
///       <br/>
///     - Popular implementations make both insertion and extraction sub-linear operations (i.e. faster than O(n)).
///     </para>
/// </remarks>
public interface IPriorityQueue<T> : IEnumerable<T>
{
    /// <summary>
    /// Inserts the provided <paramref name="item"/> into the queue, with the provided <paramref name="priority"/>.
    /// </summary>
    /// <param name="item">The item to be inserted into the queue.</param>
    /// <param name="priority">The priority to be assigned to the <paramref name="item"/>.</param>
    void Push(T item, int priority);

    /// <summary>
    /// Extracts the item of the queue with highest priority.
    /// </summary>
    PrioritizedItem<T> Pop();

    /// <summary>
    /// The number of items currently in the queue.
    /// </summary>
    /// <value>
    /// A non-negative value.
    /// </value>
    int Count { get; }

    /// <summary>
    /// Retrieves the item of the queue with highest priority, without extracting it.
    /// </summary>
    PrioritizedItem<T> Peek();
}
