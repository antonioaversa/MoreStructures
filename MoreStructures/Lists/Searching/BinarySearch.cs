using MoreStructures.Utilities;

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
    private static int ValidateIndexesAndGetLength<T>(IEnumerable<T> source, int? fromIndex, int? toIndex)
    {
        var length = source.CountO1();
        if (fromIndex != null && (fromIndex < 0 || fromIndex > length - 1))
            throw new ArgumentOutOfRangeException(
                nameof(fromIndex), $"Must be within the range of valid indexes for {source}.");
        if (toIndex != null && (toIndex < 0 || toIndex > length - 1))
            throw new ArgumentOutOfRangeException(
                nameof(toIndex), $"Must be within the range of valid indexes for {source}.");
        return length;
    }

    private static int Search<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer, int? fromIndex, int? toIndex, bool first)
    {
        int length = ValidateIndexesAndGetLength(source, fromIndex, toIndex);

        comparer ??= Comparer<T>.Default;

        var start = fromIndex ?? 0;
        var end = toIndex ?? length - 1;
        var result = -1;
        while (start <= end)
        {
            var middle = start + (end - start) / 2;
            var comparison = comparer.Compare(source.ElementAtO1(middle), item);
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

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation assumes that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
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

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation assumes that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc/>
    ///     <br/>
    ///     The size of the output and the Space Complexity is O(sigma), where:
    ///     <inheritdoc cref="LinearSearch.FirstAll{T}(IEnumerable{T}, IComparer{T}?, int?, int?)" 
    ///         path="/remarks/para[@id='params']"/>
    ///     <br/>
    ///     A binary search for the next different element gives an overall O(sigma * log(sigma)) Time Complexity.
    /// </remarks>
    public IDictionary<T, int> FirstAll<T>(
        IEnumerable<T> source, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null)
        where T : notnull
    {
        var length = ValidateIndexesAndGetLength(source, fromIndex, toIndex);

        comparer ??= Comparer<T>.Default;

        var start = fromIndex ?? 0;
        var end = toIndex ?? length;

        var result = new Dictionary<T, int> { };

        int currentIndex = start;
        while (currentIndex >= 0 && currentIndex < end && currentIndex < length)
        {
            var currentItem = source.ElementAtO1(currentIndex);
            if (!result.TryGetValue(currentItem, out var _))
                result.Add(currentItem, currentIndex);

            currentIndex = Last(source, currentItem, comparer, currentIndex, toIndex) + 1;
        }

        return result;
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation assumes that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
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

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation assumes that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
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

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     This specific implementation assumes that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
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

        if (comparer.Compare(source.ElementAtO1OrDefault(index), item) == 0)
            return index;
        return -1;
    }
}
