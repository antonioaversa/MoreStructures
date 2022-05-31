using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Lists.Counting;

/// <summary>
/// An object able to count the number of occurrences of each item of a <see cref="IEnumerable{T}"/>. Also known as 
/// Count Array. 
/// </summary>
public interface IOccurrencesCounter
{
    /// <summary>
    /// For each item E[i] of <paramref name="enumerable"/> E at index i, counts the total number of items equal 
    /// to E[i] in E[0..(i-1)] (extremes included). The output value O[i] is called here the "occurrence rank" of E[i].
    /// </summary>
    /// <typeparam name="T">
    /// The type of items of <paramref name="enumerable"/>. Required to be a non-nullable type (value or reference).
    /// </typeparam>
    /// <param name="enumerable"></param>
    /// <returns>
    /// A new, lazy evaluated <see cref="IEnumerable{T}"/> of <see cref="int"/>, of length n, where n is the length of
    /// <paramref name="enumerable"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// var counter = ...
    /// var enumerable = new List&lt;int&gt; { 1, 4, 2, 1, 3, 4, 2, 2 }
    /// var occurrenceRanks = counter.Count(enumerable) 
    /// // Result = { 0, 0, 0, 1, 0, 1, 1, 2 }
    /// </code>
    /// </example>
    public IEnumerable<int> Count<T>(IEnumerable<T> enumerable) 
        where T: notnull;
}
