using System.Collections.Generic;

namespace MoreStructures.Lists.Searching;

/// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
/// 
/// <summary>
/// An object able to search in logarithmic time for items in direct random access structures, such as lists and 
/// arrays, which are monodimensional, implement the <see cref="IEnumerable{T}"/> interface and are sorted in ascending 
/// order according to the provided comparer (which is the property enabling the search to be carried out in O(log(n))
/// time.
/// </summary>
/// <remarks>
///     <inheritdoc cref="ISearch"/>
///     <br/>
///     The sorting order assumed by this search can be reversed by simply inverting the comparer implementation.
/// </remarks>
public class BinarySearch : ISearch
{
    private static int Search<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer, int? fromIndex, int? toIndex, bool first)
    {
        comparer ??= Comparer<T>.Default;

        // Optimization for strings, due to LINQ ElementAt(index) being O(n) for strings.
        Func<int, int> compareWith =
            (source is string str) && item is char charToFind && comparer is IComparer<char> charComparer
            ? i => charComparer.Compare(str[i], charToFind)
            : i => comparer.Compare(source.ElementAt(i), item);

        var length = source is string str1 ? str1.Length : source.Count();
        var start = fromIndex ?? 0;
        var end = toIndex ?? length - 1;
        var result = -1;
        while (start <= end)
        {
            var middle = (start + end) / 2;
            var comparison = compareWith(middle);
            if (comparison < 0)
            {
                start = middle + 1;
            }
            else if (comparison == 0)
            {
                result = middle;

                if (first)
                    end = middle - 1;
                else
                    start = middle + 1;
            }
            else
            {
                end = middle - 1;
            }
        }

        return result;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm split in half the search space at every iteration, reducing it exponentially to a single item
    /// or to an empty set.
    /// <br/>
    /// Time Complexity = O(log(n)), Space Complexity = O(1), where n = number of items between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int First<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) =>
        Search(source, item, comparer, fromIndex, toIndex, true);

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm split in half the search space at every iteration, reducing it exponentially to a single item
    /// or to an empty set.
    /// <br/>
    /// Time Complexity = O(log(n)), Space Complexity = O(1), where n = number of items between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int Last<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) =>
        Search(source, item, comparer, fromIndex, toIndex, false);

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm peforms two successive binary search operations: the first to find the lower extreme of the 
    /// interval and the second to find the higher extreme of the interval. Each binary search runs in logarithmic 
    /// time.
    /// <br/>
    /// Time Complexity = O(log(n)), Space Complexity = O(1), where n = number of items between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public (int first, int last) Interval<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null)
    {
        var first = First(source, item, comparer, fromIndex, toIndex);
        if (first < 0)
            return (-1, -1);

        var last = Last(source, item, comparer, first, toIndex);
        return (first, last);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The algorithm first performs a binary search to find the index i of the 1st item. Then it checks whether 
    /// the n-th occurrence of the item exists at index i + n, taking advantage of the fact that 
    /// <paramref name="source"/> is sorted. The first step takes logarithmic time, whereas the second step takes
    /// constant time and space to execute.
    /// <br/>
    /// Time Complexity = O(log(n)), Space Complexity = O(1), where n = number of items between 
    /// <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    /// </remarks>
    public int Nth<T>(
        IEnumerable<T> source, T item, int occurrenceRank, IComparer<T>? comparer = null, int? fromIndex = null, 
        int? toIndex = null)
    {
        if (occurrenceRank < 0)
            throw new ArgumentOutOfRangeException(nameof(occurrenceRank), "Must be a non-negative value.");

        comparer ??= Comparer<T>.Default;

        var first = First(source, item, comparer, fromIndex, toIndex);
        if (first < 0)
            return -1;

        var index = first + occurrenceRank;

        // Optimization for strings, due to LINQ ElementAt(index) being O(n) for strings.
        if (source is string str)
        {
            if (index < str.Length && Equals(str[index], item))
                return index;
            return -1;
        }

        if (comparer.Compare(source.ElementAtOrDefault(index), item) == 0)
            return index;
        return -1;
    }
}
