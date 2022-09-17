using MoreLinq;
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

    private static IEnumerable<KeyValuePair<int, T>> GetIndexedItemsInRangeEqualTo<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer, int? fromIndex, int? toIndex, bool reverse)
    {
        var length = ValidateIndexesAndGetLength(source, fromIndex, toIndex);
        fromIndex ??= 0;
        toIndex ??= length - 1;

        comparer ??= Comparer<T>.Default;

        var count = toIndex.Value! - fromIndex.Value! + 1;

        var indexes = Enumerable.Range(fromIndex.Value!, count);

        if (reverse)
        {
            source = source.Reverse();
            indexes = indexes.Select(i => count - 1 - i);
        }

        return source
            .SkipO1(fromIndex.Value!)
            .Take(count)
            .Zip(indexes)
            .Where(c => comparer.Compare(c.First, item) == 0)
            .Select(c => KeyValuePair.Create(c.Second, c.First));
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
    ///     <para id="algo">
    ///     ALGORITHM
    ///     <br/>
    ///     The algorithm linearly scans the search space from <paramref name="fromIndex"/> to 
    ///     <paramref name="toIndex"/>, one index at every iteration, collecting first occurrences into a 
    ///     <see cref="IDictionary{TKey, TValue}"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     Time Complexity = O(n), Space Complexity = O(sigma), where:
    ///     <br/>
    ///     - n is the <b>number of items</b> between <paramref name="fromIndex"/> and <paramref name="toIndex"/>.
    ///       <br/>
    ///     - For "large alphabets" scenarios (such as when the alphabet is int - 2^32 possible values, but 
    ///       <paramref name="source"/> is way smaller than that), sigma is the <b>number of distinct elements </b> of 
    ///       <paramref name="source"/>.
    ///       <br/>
    ///     - For "small alphabets" scenarios (such as when the alphabet is comprised of few symbols only), sigma is
    ///       the size of the alphabet.
    ///       <br/>
    ///     - In either scenario the worst case of the O(sigma) Space Complexity is O(n), which is when all the symbols
    ///       in <paramref name="source"/> are different from each other).
    ///     </para>
    /// </remarks>
    public IDictionary<T, int> FirstAll<T>(
        IEnumerable<T> source, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null)
        where T : notnull
    {
        var length = ValidateIndexesAndGetLength(source, fromIndex, toIndex);
        fromIndex ??= 0;
        toIndex ??= length - 1;

        var result = new Dictionary<T, int> { };

        var sourceWindow = source
            .SkipO1(fromIndex.Value!)
            .Take(toIndex.Value! - fromIndex.Value! + 1);

        var index = fromIndex.Value!;
        foreach (var item in sourceWindow)
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
