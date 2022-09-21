using System.Diagnostics.CodeAnalysis;

namespace MoreStructures.Stack;

/// <summary>
/// A <see cref="IStack{T}"/> implementation based on a singly-linked linked list of items.
/// </summary>
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
    /// First, calls <see cref="Peek"/> to retrieve the item at the top of the stack.
    /// <br/>
    /// Then it updates the reference to the head of the linked list, to point to its "next".
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Pop()
    {
        var head = Peek();
        Head = Head?.Next;
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
