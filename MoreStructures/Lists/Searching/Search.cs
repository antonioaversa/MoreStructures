using System.Collections.Generic;

namespace MoreStructures.Lists.Searching;

/// <summary>
/// Searching methods for all direct random access structures, such as lists and arrays.
/// </summary>
/// <remarks>
/// In .NET, random access structures usually have an indexer defined, which takes as an index the address of the 
/// element in the data structure (usually an integer). Most random access structures (but not all) inherit 
/// <see cref="IList{T}"/> and define <see cref="IList{T}.this[int]"/>. 
/// <br/>
/// A notable exception is <see cref="string"/>, which has an indexer (<see cref="string.this[int]"/>), but does not
/// implement <see cref="IList{T}"/>. For <see cref="string"/>, specific optimizations are done, to ensure random
/// access in O(1) time.
/// <br/>
/// </remarks>
public static class Search
{
    private static int BinarySearch<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer, int? fromIndex, int? toIndex, bool first)
    {
        comparer ??= Comparer<T>.Default;

        // Optimization for strings, due to LINQ ElementAt(index) being O(n) for strings.
        Func<int, int> compareWith =
            (source is string str) && element is char charToFind && comparer is IComparer<char> charComparer
            ? i => charComparer.Compare(str[i], charToFind)
            : i => comparer.Compare(source.ElementAt(i), element);

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

    /// <summary>
    /// Find the index of the first element in the sub-sequence of elements of <paramref name="source"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="element"/>, assuming that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements of <paramref name="source"/>. Must be comparable.
    /// If it is <see cref="string"/>, random access is done in O(1) via <see cref="string.this[int]"/>.
    /// Otherwise, random access is done via the generic LINQ method 
    /// <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, Index)"/>, which is O(n) or O(1) dependending
    /// on the <see cref="IEnumerable{T}"/> concretion.
    /// </typeparam>
    /// <param name="source">The enumerable where to search for <paramref name="element"/>.</param>
    /// <param name="element">The element to search for.</param>
    /// <param name="comparer">
    /// The comparer to be used when performing the search.
    /// If not specified, <see cref="Comparer{T}.Default"/> is used.
    /// </param>
    /// <param name="fromIndex">
    /// The first index, marking the begin of the sub-sequence of <paramref name="source"/> where to search.
    /// If not specified, 0 is used.
    /// </param>
    /// <param name="toIndex">
    /// The last index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// If not specified, <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> is called on 
    /// <paramref name="source"/> to calculate the count of elements, and count - 1 is used.
    /// If <paramref name="source"/> is a <see cref="string"/>, however, <see cref="string.Length"/> is used 
    /// instead.
    /// </param>
    /// <returns>The first index of <paramref name="element"/> in <paramref name="source"/>.</returns>
    public static int BinarySearchFirst<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) => 
        BinarySearch(source, element, comparer, fromIndex, toIndex, true);

    /// <inheritdoc cref="BinarySearchFirst{T}(IEnumerable{T}, T, IComparer{T}?, int?, int?)" 
    ///     path="//*[not(self::summary or self::returns)]"/>
    ///     
    /// <summary>
    /// Find the index of the last element in the sub-sequence of elements of <paramref name="source"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="element"/>, assuming that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
    /// <returns>
    /// The first and last index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// </returns>
    public static int BinarySearchLast<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null) =>
        BinarySearch(source, element, comparer, fromIndex, toIndex, false);

    /// <inheritdoc cref="BinarySearchFirst{T}(IEnumerable{T}, T, IComparer{T}?, int?, int?)" 
    ///     path="//*[not(self::summary or self::returns)]"/>
    ///     
    /// <summary>
    /// Find the indexes of the first and last elements in the sub-sequence of elements of 
    /// <paramref name="source"/> from <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which 
    /// is equal to <paramref name="element"/>, assuming that <paramref name="source"/> is sorted in ascending 
    /// order.
    /// </summary>
    /// <returns>
    /// The first and last index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// </returns>
    public static (int first, int last) BinarySearchInterval<T>(
        IEnumerable<T> source, T element, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null)
    {
        var first = BinarySearchFirst(source, element, comparer, fromIndex, toIndex);
        if (first < 0)
            return (-1, -1);

        var last = BinarySearchLast(source, element, comparer, first, toIndex);
        return (first, last);
    }

    /// <inheritdoc cref="BinarySearchFirst{T}(IEnumerable{T}, T, IComparer{T}?, int?, int?)" 
    ///     path="//*[not(self::summary or self::returns)]"/>
    ///     
    /// <summary>
    /// Find the index of the n-th occurence (0-based) of the element in the sub-sequence of elements of 
    /// <paramref name="source"/> from <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is 
    /// equal to <paramref name="element"/>, assuming that <paramref name="source"/> is sorted in ascending order.
    /// </summary>
    /// <returns>
    /// The n-th index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// </returns>
    public static int BinarySearchNth<T>(
        IEnumerable<T> source, T element, int occurrenceRank, IComparer<T>? comparer = null, int? fromIndex = null, 
        int? toIndex = null)
    {
        if (occurrenceRank < 0)
            throw new ArgumentOutOfRangeException(nameof(occurrenceRank), "Must be a non-negative value.");

        comparer ??= Comparer<T>.Default;

        var first = BinarySearchFirst(source, element, comparer, fromIndex, toIndex);
        if (first < 0)
            return -1;

        var index = first + occurrenceRank;

        // Optimization for strings, due to LINQ ElementAt(index) being O(n) for strings.
        if (source is string str)
        {
            if (index < str.Length && Equals(str[index], element))
                return index;
            return -1;
        }

        if (comparer.Compare(source.ElementAtOrDefault(index), element) == 0)
            return index;
        return -1;
    }
}
