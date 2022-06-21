namespace MoreStructures.Lists.Searching;

/// <summary>
/// An object able to search for items in direct random access structures, such as lists and arrays, which are 
/// monodimensional and implement the <see cref="IEnumerable{T}"/> interface.
/// </summary>
/// <remarks>
/// In .NET, random access structures usually have an indexer defined, which takes as an index the address of the 
/// item in the data structure (usually an integer). Most random access structures (but not all) inherit 
/// <see cref="IList{T}"/> and define <see cref="IList{T}.this[int]"/>. 
/// <br/>
/// A notable exception is <see cref="string"/>, which has an indexer (<see cref="string.this[int]"/>), but does not
/// implement <see cref="IList{T}"/>. For <see cref="string"/>, specific optimizations are done, to ensure random
/// access in O(1) time.
/// <br/>
/// </remarks>
public interface ISearch
{
    /// <summary>
    /// Find the index of the first item in the sub-sequence of items of <paramref name="source"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="item"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items of <paramref name="source"/>. Must be comparable.
    /// If it is <see cref="string"/>, random access is done in O(1) via <see cref="string.this[int]"/>.
    /// Otherwise, random access is done via the generic LINQ method 
    /// <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, Index)"/>, which is O(n) or O(1) dependending
    /// on the <see cref="IEnumerable{T}"/> concretion.
    /// </typeparam>
    /// <param name="source">The enumerable where to search for <paramref name="item"/>.</param>
    /// <param name="item">The item to search for.</param>
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
    /// <paramref name="source"/> to calculate the count of items, and count - 1 is used.
    /// If <paramref name="source"/> is a <see cref="string"/>, however, <see cref="string.Length"/> is used 
    /// instead.
    /// </param>
    /// <returns>The first index of <paramref name="item"/> in <paramref name="source"/>.</returns>
    int First<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null);

    /// <summary>
    /// Find the indexes of the first occurrence of each item in the sub-sequence of items of <paramref name="source"/>
    /// from <paramref name="fromIndex"/> to <paramref name="toIndex"/> included.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items of <paramref name="source"/>. Must be comparable and non-null, since it is used as key to 
    /// index first occurrence indexes in the output <see cref="IDictionary{TKey, TValue}"/>.
    /// <br/>
    /// If it is <see cref="string"/>, random access is done in O(1) via <see cref="string.this[int]"/>.
    /// Otherwise, random access is done via the generic LINQ method 
    /// <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, Index)"/>, which is O(n) or O(1) dependending
    /// on the <see cref="IEnumerable{T}"/> concretion.
    /// </typeparam>
    /// <param name="source">The enumerable.</param>
    /// <param name="comparer">
    /// The comparer to be used when performing the search, to tell apart different items.
    /// If not specified, <see cref="Comparer{T}.Default"/> is used.
    /// </param>
    /// <param name="fromIndex">
    /// The first index, marking the begin of the sub-sequence of <paramref name="source"/> where to search.
    /// If not specified, 0 is used.
    /// </param>
    /// <param name="toIndex">
    /// The last index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// If not specified, <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> is called on 
    /// <paramref name="source"/> to calculate the count of items, and count - 1 is used.
    /// If <paramref name="source"/> is a <see cref="string"/>, however, <see cref="string.Length"/> is used 
    /// instead.
    /// </param>
    /// <returns>
    /// A <see cref="IDictionary{TKey, TValue}"/> containing the 0-based index first occurrence of each item, indexed 
    /// by the item itself.
    /// </returns>
    IDictionary<T, int> FirstAll<T>(
        IEnumerable<T> source, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null)
        where T : notnull;

    /// <inheritdoc cref="First{T}(IEnumerable{T}, T, IComparer{T}?, int?, int?)" 
    ///     path="//*[not(self::summary or self::returns)]"/>
    ///     
    /// <summary>
    /// Find the index of the last item in the sub-sequence of items of <paramref name="source"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="item"/>.
    /// </summary>
    /// <returns>
    /// The first and last index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// </returns>
    int Last<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null);

    /// <inheritdoc cref="First{T}(IEnumerable{T}, T, IComparer{T}?, int?, int?)" 
    ///     path="//*[not(self::summary or self::returns)]"/>
    ///     
    /// <summary>
    /// Find the indexes of the first and last items in the sub-sequence of items of <paramref name="source"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="item"/>.
    /// </summary>
    /// <returns>
    /// The first and last index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// </returns>
    (int first, int last) Interval<T>(
        IEnumerable<T> source, T item, IComparer<T>? comparer = null, int? fromIndex = null, int? toIndex = null);

    /// <inheritdoc cref="First{T}(IEnumerable{T}, T, IComparer{T}?, int?, int?)" 
    ///     path="//*[not(self::summary or self::returns)]"/>
    ///     
    /// <summary>
    /// Find the index of the n-th occurence (0-based) of the item in the sub-sequence of items of 
    /// <paramref name="source"/> from <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is 
    /// equal to <paramref name="item"/>.
    /// </summary>
    /// <returns>
    /// The n-th index, marking the end of the sub-sequence of <paramref name="source"/> where to search.
    /// </returns>
    int Nth<T>(
        IEnumerable<T> source, T item, int occurrenceRank, IComparer<T>? comparer = null, int? fromIndex = null,
        int? toIndex = null);
}
