namespace MoreStructures.Utilities;

/// <summary>
/// Extensions for <see cref="LinkedList{T}"/>.
/// </summary>
public static class LinkedListExtensions
{
    /// <summary>
    /// Generates an <see cref="IEnumerable{T}"/> of the <see cref="LinkedListNode{T}"/> of the provided 
    /// <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects contained by <paramref name="list"/>.</typeparam>
    /// <param name="list">The list, to iterate over.</param>
    /// <returns>A sequence of <see cref="LinkedListNode{T}"/>.</returns>
    public static IEnumerable<LinkedListNode<T>> AsNodes<T>(this LinkedList<T> list)
    {
        for (var node = list.First; node != null; node = node.Next)
            yield return node;
    }
}
