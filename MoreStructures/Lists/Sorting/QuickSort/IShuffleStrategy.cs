namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// An algorithm shuffling the specified window of the provided list.
/// </summary>
public interface IShuffleStrategy
{
    /// <summary>
    /// Shuffles the items of <paramref name="list"/> between index <paramref name="start"/> and 
    /// <paramref name="end"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items of the <paramref name="list"/>. It doesn't necessarily have to support 
    /// <see cref="IComparable{T}"/>, since items are never compared to each other, only swapped.
    /// </typeparam>
    /// <param name="list">The list whose window to be shuffled.</param>
    /// <param name="start">The left index of the window of <paramref name="list"/>, included.</param>
    /// <param name="end">The right index of the window of <paramref name="list"/>, included.</param>
    void Shuffle<T>(IList<T> list, int start, int end);
}
