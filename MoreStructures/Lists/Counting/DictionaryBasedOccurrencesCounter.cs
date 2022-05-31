namespace MoreStructures.Lists.Counting;

/// <summary>
/// An implementation of <see cref="IOccurrencesCounter"/> which uses a <see cref="Dictionary{TKey, TValue}"/> to build
/// the <see cref="IEnumerable{T}"/> of occurrences in linear time.
/// </summary>
public class DictionaryBasedOccurrencesCounter : IOccurrencesCounter
{
    /// <inheritdoc path="//*[not(self::remarks or self::typeparam)]"/>
    /// <typeparam name="T">
    /// The type of items of <paramref name="enumerable"/>. 
    /// </typeparam>
    /// <remarks>
    /// Perform counting by keeping a <see cref="IDictionary{TKey, TValue}"/> of the current number of occurrences per
    /// item encountered while enumerating <paramref name="enumerable"/>.
    /// </remarks>
    public IEnumerable<int> Count<T>(IEnumerable<T> enumerable)
        where T: notnull
    {
        var occurrenceRanks = new Dictionary<T, int> { };
        foreach (var item in enumerable)
        {
            if (!occurrenceRanks.TryGetValue(item, out var occurrenceRankOfItem))
                occurrenceRankOfItem = 0;

            occurrenceRanks[item] = occurrenceRankOfItem + 1;
            yield return occurrenceRankOfItem;
        }
    }
}