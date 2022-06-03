namespace MoreStructures.Lists.Counting;

/// <summary>
/// An implementation of <see cref="IOccurrencesCounter"/> which uses a <see cref="Dictionary{TKey, TValue}"/> to build
/// the two-level <see cref="IDictionary{TKey, TValue}"/> of occurrences, indexed by distinct item values and index.
/// </summary>
public class DictionaryBasedOccurrencesCounter : IOccurrencesCounter
{
    /// <inheritdoc path="//*[not(self::remarks or self::typeparam)]"/>
    /// <typeparam name="T">
    /// The type of items of <paramref name="enumerable"/>. 
    /// </typeparam>
    /// <remarks>
    /// Perform counting by keeping a <see cref="IDictionary{TKey, TValue}"/> of the current number of occurrences per
    /// item encountered, by distinct value of <typeparamref name="T"/> and index, while enumerating 
    /// <paramref name="enumerable"/>.
    /// <br/>
    /// Time Complexity = O(n) and Space Complexity = O(n * sigma), where n = number of items in 
    /// <paramref name="enumerable"/> and sigma = number of the alphabet of <paramref name="enumerable"/>, i.e. the 
    /// number of distinct items of type <typeparamref name="T"/> in <paramref name="enumerable"/>.
    /// </remarks>
    public IDictionary<T, IDictionary<int, int>> Count<T>(IEnumerable<T> enumerable)
        where T: notnull
    {
        var lastIndexSetByItem = new Dictionary<T, int> { };
        var counts = new Dictionary<T, IDictionary<int, int>> { };
        var index = 0;
        foreach (var item in enumerable)
        {
            if (!counts.TryGetValue(item, out var countsOfItem))
                counts[item] = countsOfItem = new Dictionary<int, int> { };

            // Set the count for item at the current index, to be one more than the previous index (or 1)
            countsOfItem[index] = countsOfItem.ContainsKey(index - 1) ? countsOfItem[index - 1] + 1 : 1;

            // Set the counts for all indexes between index since the last update of counts[item] and the current one.
            if (!lastIndexSetByItem.TryGetValue(item, out var lastIndexSet))
                lastIndexSet = -1;

            for (var priorIndex = lastIndexSet + 1; priorIndex < index; priorIndex++)
                countsOfItem[priorIndex] = countsOfItem[index] - 1;

            lastIndexSetByItem[item] = index;

            // Set the count for all other items at the current index, to be the same as for previous index (or 0)
            foreach (var otherItem in counts.Keys)
                if (!Equals(otherItem, item))
                    counts[otherItem][index] = counts[otherItem][index - 1];

            index++;
        }

        return counts;
    }
}