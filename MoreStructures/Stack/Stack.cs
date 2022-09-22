namespace MoreStructures.Stack;

/// <summary>
/// Defines the interface common to all <b>Stack</b> implementations.
/// </summary>
/// <typeparam name="T">The type of items, the stack is composed of.</typeparam>
/// <remarks>
///     <para id="definitions">
///     DEFINITION
///     <br/>
///     - A Stack is a data structure storing items of a generic type <typeparamref name="T"/>.
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
