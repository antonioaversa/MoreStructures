using MoreLinq;

namespace MoreStructures.Lists.Searching;

/// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
/// 
/// <summary>
/// An object able to search in linear time for elements in direct random access structures, such as lists and 
/// arrays, which are monodimensional and implement the <see cref="IEnumerable{T}"/> interface.
/// </summary>
/// <remarks>
///     <inheritdoc cref="ISearch"/>
///     <br/>
///     Unlike <see cref="BinarySearch"/>, this implementation doesn't make any assumption on the order of elements in
///     in the data structure.
/// </remarks>
public class LinearSearch : ISearch
{
    private static IEnumerable<KeyValuePair<int, T>> GetIndexedElementInRangeEqualTo<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer, int? fromIndex, int? toIndex, bool reverse)
    {
        comparer ??= Comparer<T>.Default;
        var indexedSource = source.Index();

        if (reverse)
            indexedSource = indexedSource.Reverse();

        return indexedSource
            .Where(e => comparer.Compare(e.Value, element) == 0)
            .Where(e => (fromIndex == null || e.Key >= fromIndex) && (toIndex == null || e.Key <= toIndex));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="fromIndex"/> to <paramref name="toIndex"/>,
    /// one index at every iteration, reducing it linearly to a single element or to an empty set.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of elements between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int First<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) => 
        GetIndexedElementInRangeEqualTo(source, element, comparer, fromIndex, toIndex, false)
            .Select(e => e.Key as int?)
            .FirstOrDefault(-1)!
            .Value;

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="toIndex"/> to <paramref name="fromIndex"/>,
    /// one index at every iteration, reducing it linearly to a single element or to an empty set.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of elements between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int Last<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) => 
        GetIndexedElementInRangeEqualTo(source, element, comparer, fromIndex, toIndex, true)
            .Select(e => e.Key as int?)
            .FirstOrDefault(-1)!
            .Value;

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="fromIndex"/> to <paramref name="toIndex"/>,
    /// one index at every iteration.
    /// <br/>
    /// It just stores the smallest and the biggest index among the ones which correspond to elements equal to 
    /// <paramref name="element"/>. So it has constant, and not linear, space requirements.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of elements between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public (int first, int last) Interval<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) => 
        GetIndexedElementInRangeEqualTo(source, element, comparer, fromIndex, toIndex, false)
            .Select(e => e.Key)
            .Aggregate((-1, -1), (acc, i) => (
                acc.Item1 < 0 ? i : Math.Min(acc.Item1, i),
                acc.Item2 < 0 ? i : Math.Max(acc.Item2, i)));

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="toIndex"/> to <paramref name="fromIndex"/>,
    /// one index at every iteration, reducing it linearly to a single element or to an empty set.
    /// <br/>
    /// It just stores the current counter of occurrences of <paramref name="element"/> in <paramref name="source"/>,
    /// not all of them. So it has constant, and not linear, space requirements.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of elements between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int Nth<T>(
        IEnumerable<T> source, T element, int occurrenceRank, IComparer<T>? comparer = null, int? fromIndex = null,
        int? toIndex = null)
    {
        if (occurrenceRank < 0)
            throw new ArgumentOutOfRangeException("Must be non-negative.", nameof(occurrenceRank));

        return GetIndexedElementInRangeEqualTo(source, element, comparer, fromIndex, toIndex, false)
            .Select(e => e.Key as int?)
            .ElementAtOrDefault(occurrenceRank) ?? -1;
    }
}
