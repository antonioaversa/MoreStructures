namespace MoreStructures.XifoStructures;

/// <summary>
/// A <see cref="IXifoStructure{T}"/> based on a <see cref="Stack{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of items of the data structure, and of the underlying <see cref="Stack{T}"/>.
/// </typeparam>
public class XStack<T> : IXifoStructure<T>
{
    private readonly Stack<T> Underlying = new();

    /// <summary>
    /// Invokes <see cref="Stack{T}.Pop"/> on the underlying <see cref="Stack{T}"/>.
    /// </summary>
    /// <returns>An item of type <typeparamref name="T"/>.</returns>
    public T Pop() => Underlying.Pop();

    /// <summary>
    /// Invokes <see cref="Stack{T}.Push(T)"/> on the underlying <see cref="Stack{T}"/>.
    /// </summary>
    /// <param name="item">An item of type <typeparamref name="T"/>.</param>
    public void Push(T item) => Underlying.Push(item);

    /// <summary>
    /// Invokes <see cref="Stack{T}.Count"/> on the underlying <see cref="Stack{T}"/>.
    /// </summary>
    public int Count => Underlying.Count;
}
