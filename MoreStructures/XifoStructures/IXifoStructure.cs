namespace MoreStructures.XifoStructures;

/// <summary>
/// An Abstract Data Type modelling a generic XIFO structure: either a LIFO (such as a <see cref="XStack{T}"/>), or a 
/// FIFO (such as a <see cref="XQueue{T}"/>).
/// </summary>
/// <typeparam name="T">The type of items of the data structure.</typeparam>
/// <remarks>
/// This interface is implemented by adapters for standard data structures present in the framework, such as 
/// <see cref="XStack{T}"/> and <see cref="XQueue{T}"/>.
/// <br/>
/// The advantage of using this interface is that the order of insertion and extraction of the items of the structure
/// can be changed in the code of the client using the data structure without changing the code itself.
/// </remarks>
public interface IXifoStructure<T>
{
    /// <summary>
    /// Inserts the provided <paramref name="item"/> into this data structure.
    /// </summary>
    /// <param name="item">The item to be inserted.</param>
    /// <remarks>
    /// Insertion is done in a way to minimize the cost of the operation.
    /// <br/>
    /// For example insertion into a stack is done on top of the stack, since prepending is a O(1) operation.
    /// <br/>
    /// Insertion into a queue is done at the end of the queue instead, since appending is the O(1) way of inserting.
    /// </remarks>
    void Push(T item);

    /// <summary>
    /// Extracts the first available item from this data structure.
    /// </summary>
    /// <returns>An item of the data structure, of type <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// Extraction is done in a way to minimize the cost of the operation.
    /// <br/>
    /// For example extraction from a stack is done from the top of the stack, since it can be accessed in O(1) time.
    /// <br/>
    /// Extraction from a queue is done at the beginning of the queue, since it can be modified in O(1) time.
    /// </remarks>
    T Pop();

    /// <summary>
    /// Returns the number of items in this data structure.
    /// </summary>
    /// <value>
    /// A non-negative value.
    /// </value>
    int Count { get; }
}
