namespace MoreStructures.Queues;

/// <summary>
/// A <see cref="IQueue{T}"/> implementation based on a singly-linked linked list of items.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IQueue{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - The advantages and disadvantages of using a linked list over an array-based implementation are the same 
///       as the ones described in <see cref="Stacks.LinkedListStack{T}"/>.
///     </para>
/// </remarks>
public class LinkedListQueue<T> : IQueue<T>
{
    private sealed record Node(T Value, Node? Next)
    {
        public Node? Next { get; set; } = Next;
    }

    private Node? Head { get; set; } = null;
    private Node? Last { get; set; } = null;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Stored and updated in constant time at each <see cref="Enqueue(T)"/> and <see cref="Dequeue"/>.
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
            throw new InvalidOperationException($"Cannot {nameof(Peek)} on an empty queue.");

        return Head.Value;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// First, it retrieves the item referenced by the front of the underlying linked list.
    /// <br/>
    /// Then it updates the reference to the head of the linked list, to point to its "next".
    /// The reference to the last item is also updated, if the head is set to point to null.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Dequeue()
    {
        if (Head == null)
            throw new InvalidOperationException($"Can't {nameof(Dequeue)} on an empty queue.");

        var head = Head.Value;
        Head = Head.Next;
        Count--;
        if (Head == null)
            Last = null;
        return head;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Creates a new node wrapping the provided <paramref name="item"/>.
    /// <br/>
    /// Then, it appends it to the back of the linked list of items, updating the reference to the last item of the 
    /// list.
    /// <br/>
    /// It also updates the head, if the list was previously empty, to point to the newly created item.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public void Enqueue(T item)
    {
        var node = new Node(item, Head);
        if (Last == null)
        {
            Last = Head = node;
        }
        else
        {
            Last.Next = node;
            Last = node;
        }
        Count++;
    }
}
