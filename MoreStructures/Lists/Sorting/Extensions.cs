namespace MoreStructures.Lists.Sorting;

/// <summary>
/// Extensions methods for <see cref="IList{T}"/>, in the context of list sorting.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Verifies that the provided <paramref name="list"/> is sorted.
    /// </summary>
    /// <typeparam name="T">The type of items of <paramref name="list"/>.</typeparam>
    /// <param name="list">The <see cref="IList{T}"/> to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="list"/> is sorted, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    /// Zips the list with itself, shifted one item forward.
    /// <br/>
    /// Then checks whether each of the couples of the zip has a <see cref="ValueTuple{T, U}.Item1"/> non-bigger than 
    /// <see cref="ValueTuple{T, U}.Item2"/>.
    /// <br/>
    /// Stops at the first encountered.
    /// <br/>
    /// Time Complexity is O(n), when fully enumerated and in the worst case. Space Complexity is O(1).
    /// </remarks>
    public static bool IsSorted<T>(this IList<T> list)
        where T : IComparable<T> => 
        list.Zip(list.Skip(1)).All(c => c.First.CompareTo(c.Second) <= 0);

    /// <inheritdoc cref="IsSorted{T}(IList{T})" path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// Verifies that the provided <paramref name="list"/> is sorted, using the provided <paramref name="comparer"/>.
    /// </summary>
    /// <param name="list">The <see cref="IList{T}"/> to check.</param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> to be used to comparer <typeparamref name="T"/> instances.
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="IsSorted{T}(IList{T})"/>
    /// </remarks>
    public static bool IsSorted<T>(this IList<T> list, IComparer<T> comparer) =>
        list.Zip(list.Skip(1)).All(c => comparer.Compare(c.First, c.Second) <= 0);
}
