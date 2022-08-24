namespace MoreStructures.XifoStructures;

/// <summary>
/// A <see cref="IXifoStructure{T}"/> based on a <see cref="Queue{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of items of the data structure, and of the underlying <see cref="Queue{T}"/>.
/// </typeparam>
public class XQueue<T> : IXifoStructure<T>
{
    private readonly Queue<T> Underlying = new();

    /// <summary>
    /// Invokes <see cref="Queue{T}.Dequeue"/> on the underlying <see cref="Queue{T}"/>.
    /// </summary>
    /// <returns>An item of type <typeparamref name="T"/>.</returns>
    public T Pop() => Underlying.Dequeue();

    /// <summary>
    /// Invokes <see cref="Queue{T}.Enqueue(T)"/> on the underlying <see cref="Queue{T}"/>.
    /// </summary>
    /// <param name="item">An item of type <typeparamref name="T"/>.</param>
    public void Push(T item) => Underlying.Enqueue(item);

    /// <summary>
    /// Invokes <see cref="Queue{T}.Count"/> on the underlying <see cref="Queue{T}"/>.
    /// </summary>
    public int Count => Underlying.Count;
}
