namespace MoreStructures.Queues;

/// <summary>
/// Defines the interface common to all <b>Queues</b> implementations for items of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items, the queue is composed of.</typeparam>
/// <remarks>
///     <para id="definitions">
///     DEFINITION
///     <br/>
///     - A Queue is a data structure storing items of a generic type <typeparamref name="T"/> in a FIFO (First In 
///       First Out) fashion.
///       <br/>
///     - Items are inserted at the back (or end) of the queue and extracted from the front (or begin) of the queue.
///       <br/>
///     - Like stacks, they are specialized data structures for which general random access is not a priority. 
///       <br/>
///     - As for stack, O(1) non amortized cost of insertion and extraction can be provided in some implementations.
///       <br/>
///     - Arrays and array lists can be used as a backing structure for a stack, and still have O(1) amortized
///       cost within array boundaries and O(1) amortized cost to remove array boundaries constraints.
///       <br/>
///     - Items are in a total order relationship and duplicates are supported.
///     </para>
/// </remarks>
public interface IQueue<T>
{
    /// <summary>
    /// The number of items currently in the queue.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Returns the item at the front of the queue, without popping it out from the queue.
    /// </summary>
    /// <returns>The item of type <typeparamref name="T"/> currently at the front of the queue.</returns>
    T Peek();

    /// <summary>
    /// Push the provided <paramref name="item"/> at the back of the queue.
    /// </summary>
    /// <param name="item">The item of type <typeparamref name="T"/> to be pushed.</param>
    /// <remarks>
    /// All other items in the queue keep their position.
    /// </remarks>
    void Enqueue(T item);

    /// <summary>
    /// Pops the item out from the front of the queue and returns it as a result.
    /// </summary>
    /// <returns>The item of type <typeparamref name="T"/> which was at the front of the queue.</returns>
    /// <remarks>
    /// The item which was on second position at the front of the queue gets promoted to first position after the item
    /// at the front is popped out. The position of all the items is shifted of -1.
    /// </remarks>
    T Dequeue();
}
