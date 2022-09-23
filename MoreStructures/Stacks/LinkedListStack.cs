namespace MoreStructures.Stacks;

/// <summary>
/// A <see cref="IStack{T}"/> implementation based on a singly-linked linked list of items.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IStack{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to an implementation based on an array list, it has the advantage of not requiring contiguous 
///       memory, for the backing structure storing the items to be allocated.
///       <br/>
///     - Items of the linked list are allocated on the heap on demand, one by one, each on requiring only the space
///       to store the value of the item (whose size depends on the definition of the type <typeparamref name="T"/>),
///       and the reference to the next node in the list (whose size depends on the bit parallelism of the 
///       architecture: typically either 32 or 64 bits).
///       <br/>
///     - That minimizes the risk of <see cref="OutOfMemoryException"/>, due to memory fragmentation, when dealing with
///       large queues.
///       <br/>
///     - The downside is the number of object allocations, which is constant on an array-based implementation, and as 
///       high as the number of items when using a linked list. 
///       <br/>
///     - This introduces more stress on the Garbage Collector and also uses way more memory than an array-based 
///       implementation, since each object on the heap has a space overhead required for its memory management.
///       <br/>
///     - Moreover the non-contiguous memory layout requires as many "next" references as the number of items in the
///       queue, which can be higher than the amount of memory used for the items: e.g. a queue of n <see cref="int"/>
///       requires n * 4 bytes for the actual data and double that amount on a 64 bit architecture to store next 
///       references.
///     </para>
/// </remarks>
public class LinkedListStack<T> : IStack<T>
{
    private sealed record Node(T Value, Node? Next);

    private Node? Head { get; set; } = null;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Stored and updated in constant time at each <see cref="Push(T)"/> and <see cref="Pop"/>.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count { get; private set; } = 0;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks the front of the underlying linked list.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Peek()
    {
        if (Head == null)
            throw new InvalidOperationException($"Can't {nameof(Peek)} on an empty stack.");

        return Head.Value;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// First, it retrieves the item referenced by the head of the underlying linked list.
    /// <br/>
    /// Then, it updates the reference to the head of the linked list, to point to its "next".
    /// <br/>
    /// Raises an <see cref="InvalidOperationException"/> if the linked list is empty.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Pop()
    {
        if (Head == null)
            throw new InvalidOperationException($"Can't {nameof(Pop)} on an empty stack.");

        var head = Head.Value;
        Head = Head.Next;
        Count--;
        return head;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Creates a new node wrapping the provided <paramref name="item"/> and sets it as new head of the linked list of
    /// items, making it point to the previous head.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public void Push(T item)
    {
        Head = new Node(item, Head);
        Count++;
    }
}
