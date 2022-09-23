namespace MoreStructures.Stacks;

/// <summary>
/// Defines the interface common to all <b>Stack</b> implementations  for items of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items, the stack is composed of.</typeparam>
/// <remarks>
///     <para id="definitions">
///     DEFINITION
///     <br/>
///     - A Stack is a data structure storing items of a generic type <typeparamref name="T"/> in a LIFO (Last In First
///       Out) fashion.
///       <br/>
///     - Items are both inserted and extracted at the top (or begin) of the stack.
///       <br/>
///     - Unlike array lists, general random access doesn't come as a easy, performant operation. 
///       <br/>
///     - Because data insertion and extraction at a single specific spot is priviledged over any other position in the
///       data structure, O(1) non amortized cost of insertion and extraction can be provided in some implementations.
///       <br/>
///     - However, arrays and array lists can be used as a backing structure for a stack, and still have O(1) amortized
///       cost within array boundaries and O(1) amortized cost to remove array boundaries constraints.
///       <br/>
///     - Items are in a total order relationship and duplicates are supported.
///     </para>
/// </remarks>
public interface IStack<T>
{
    /// <summary>
    /// The number of items currently in the stack.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Returns the item on top of the stack, without popping it out from the stack.
    /// </summary>
    /// <returns>The item of type <typeparamref name="T"/> currently on top of the stack.</returns>
    T Peek();

    /// <summary>
    /// Push the provided <paramref name="item"/> onto the top of the stack.
    /// </summary>
    /// <param name="item">The item of type <typeparamref name="T"/> to be pushed.</param>
    /// <remarks>
    /// The item which was on top of the stack before this push is pushed down onto second position.
    /// </remarks>
    void Push(T item);

    /// <summary>
    /// Pops the item out from the top of the stack and returns it as a result.
    /// </summary>
    /// <returns>The item of type <typeparamref name="T"/> which was on top of the stack.</returns>
    /// <remarks>
    /// The item which was on second position at the top of the stack goes on the top after the item on top is popped 
    /// out.
    /// </remarks>
    T Pop();
}
