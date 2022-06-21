namespace MoreStructures.Lists.Counting;

/// <summary>
/// An object able to count the number of occurrences of each item of a <see cref="IEnumerable{T}"/>, also known as 
/// Count Array. 
/// </summary>
public interface IOccurrencesCounter
{
    /// <summary>
    /// For each item t of type <typeparamref name="T"/> in <paramref name="enumerable"/> and for each index i of E, 
    /// counts the total number of items equal to t in E[0..i] (extremes included).
    /// </summary>
    /// <typeparam name="T">
    /// The type of items of <paramref name="enumerable"/>. Required to be a non-nullable type (value or reference).
    /// </typeparam>
    /// <param name="enumerable">The enumerable, to count the items of.</param>
    /// <returns>
    /// A new, lazy evaluated two-levels dictionary of <see cref="int"/>, the 1st-level one having as many items as 
    /// the number of distinct chars in <paramref name="enumerable"/>, the 2nd-level one having as many items as the 
    /// length of <paramref name="enumerable"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// var counter = ...
    /// var enumerable = new List&lt;int&gt; { 1, 4, 2, 1, 3, 4, 2, 2 }
    /// var counts = counter.Count(enumerable) 
    /// // Result = 
    /// // { 
    /// //      [1, 0] = 1, [2, 0] = 0, [3, 0] = 0,  [4, 0] = 0, // Counts by char in E[0..0]
    /// //      [1, 1] = 1, [2, 1] = 0, [3, 1] = 0,  [4, 1] = 1, // Counts by char in E[0..1]
    /// //      [1, 2] = 1, [2, 2] = 1, [3, 2] = 0,  [4, 2] = 1, // Counts by char in E[0..2]
    /// //      ...
    /// //      [1, 7] = 2, [2, 7] = 2, [3, 7] = 1,  [4, 7] = 2, // Counts by char in E[0..7]
    /// // }
    /// </code>
    /// </example>
    public IDictionary<T, IDictionary<int, int>> Count<T>(IEnumerable<T> enumerable) 
        where T: notnull;
}
