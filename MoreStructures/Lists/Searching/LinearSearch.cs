﻿using MoreLinq;
using MoreStructures.Utilities;

namespace MoreStructures.Lists.Searching;

/// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
/// 
/// <summary>
/// An object able to search in linear time for items in direct random access structures, such as lists and 
/// arrays, which are monodimensional and implement the <see cref="IEnumerable{T}"/> interface.
/// </summary>
/// <remarks>
///     <inheritdoc cref="ISearch"/>
///     <br/>
///     Unlike <see cref="BinarySearch"/>, this implementation doesn't make any assumption on the order of items in
///     in the data structure.
/// </remarks>
public class LinearSearch : ISearch
{
    private static IEnumerable<KeyValuePair<int, T>> GetIndexedItemsInRangeEqualTo<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer, int? fromIndex, int? toIndex, bool reverse)
    {
        comparer ??= Comparer<T>.Default;
        var indexedSource = source.Index();

        if (reverse)
            indexedSource = indexedSource.Reverse();

        return indexedSource
            .Where(e => comparer.Compare(e.Value, item) == 0)
            .Where(e => (fromIndex == null || e.Key >= fromIndex) && (toIndex == null || e.Key <= toIndex));
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation does not make any assunption on <paramref name="source"/> being sorted.
    /// </summary>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="fromIndex"/> to <paramref name="toIndex"/>,
    /// one index at every iteration, reducing it linearly to a single item or to an empty set.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of items between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int First<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) => 
        GetIndexedItemsInRangeEqualTo(source, item, comparer, fromIndex, toIndex, false)
            .Select(e => e.Key as int?)
            .FirstOrDefault(-1)!
            .Value;

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation does not make any assunption on <paramref name="source"/> being sorted.
    /// </summary>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="fromIndex"/> to <paramref name="toIndex"/>,
    /// one index at every iteration, collecting first occurrences into a <see cref="IDictionary{TKey, TValue}"/>.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(sigma), where:
    /// <para id = "params">
    /// - n is the <b>number of items</b> between <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// <br/>
    /// - sigma is the <b>number of distinct elements </b> of <paramref name="source"/>, for "large alphabets" 
    /// scenarios (such as when the alphabet is int - 2^32 possible values, but <paramref name="source"/> is way 
    /// smaller than that), or the size of the alphabet for "small alphabets" scenarios (such as when the alphabet is 
    /// comprised of few symbols only). In either scenario the worst case of a O(sigma) is O(n).
    /// </para>
    /// </remarks>
    public IDictionary<T, int> FirstAll<T>(
        IEnumerable<T> source, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null)
        where T : notnull
    {
        var start = fromIndex ?? 0;
        var end = toIndex ?? source.CountO1();

        var result = new Dictionary<T, int> { };
        var index = start;
        foreach (var item in source.Where((el, i) => i >= start && i <= end))
        {
            if (!result.TryGetValue(item, out var value))
                result[item] = index;
            index++;
        }

        return result;
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation does not make any assunption on <paramref name="source"/> being sorted.
    /// </summary>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="toIndex"/> to <paramref name="fromIndex"/>,
    /// one index at every iteration, reducing it linearly to a single item or to an empty set.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of items between <paramref name="fromIndex"/> 
    /// and <paramref name="toIndex"/>.
    /// </remarks>
    public int Last<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) => 
        GetIndexedItemsInRangeEqualTo(source, item, comparer, fromIndex, toIndex, true)
            .Select(e => e.Key as int?)
            .FirstOrDefault(-1)!
            .Value;

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation does not make any assunption on <paramref name="source"/> being sorted.
    /// </summary>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="fromIndex"/> to <paramref name="toIndex"/>,
    /// one index at every iteration.
    /// <br/>
    /// It just stores the smallest and the biggest index among the ones which correspond to items equal to 
    /// <paramref name="item"/>. So it has constant, and not linear, space requirements.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of items between <paramref name="fromIndex"/> 
    /// and <paramref name="toIndex"/>.
    /// </remarks>
    public (int first, int last) Interval<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) => 
        GetIndexedItemsInRangeEqualTo(source, item, comparer, fromIndex, toIndex, false)
            .Select(e => e.Key)
            .Aggregate((-1, -1), (acc, i) => (
                acc.Item1 < 0 ? i : Math.Min(acc.Item1, i),
                acc.Item2 < 0 ? i : Math.Max(acc.Item2, i)));

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation does not make any assunption on <paramref name="source"/> being sorted.
    /// </summary>
    /// <remarks>
    /// The algorithm linearly scans the search space from <paramref name="toIndex"/> to <paramref name="fromIndex"/>,
    /// one index at every iteration, reducing it linearly to a single item or to an empty set.
    /// <br/>
    /// It just stores the current counter of occurrences of <paramref name="item"/> in <paramref name="source"/>,
    /// not all of them. So it has constant, and not linear, space requirements.
    /// <br/>
    /// Time Complexity = O(n), Space Complexity = O(1), where n = number of items between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int Nth<T>(
        IEnumerable<T> source, T item, int occurrenceRank, IComparer<T>? comparer = null, int? fromIndex = null,
        int? toIndex = null)
    {
        if (occurrenceRank < 0)
            throw new ArgumentOutOfRangeException(nameof(occurrenceRank), "Must be non-negative.");

        return GetIndexedItemsInRangeEqualTo(source, item, comparer, fromIndex, toIndex, false)
            .Select(e => e.Key as int?)
            .ElementAtOrDefault(occurrenceRank) ?? -1;
    }
}
