using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using MoreStructures.Lists.Counting;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A <see cref="NarrowingIntervalMatcher"/> refinement which precalculate the count of occurrences of each item at 
/// index i in BWT[0..(i - 1)], and the index of first occurrence of each char in SortedBWT, and later uses them
/// to perform in constant time interval narrowing operations within the top-level loop of chars to match.
/// </summary>
/// <remarks>
/// 
/// </remarks>
public class CountBasedNarrowingIntervalMatcher : NarrowingIntervalMatcher
{
    /// <summary>
    /// The <see cref="IOccurrencesCounter"/> implementation to be used when counting the number of occurrences of 
    /// each of the items of an enumerable.
    /// </summary>
    protected static IOccurrencesCounter OccurrencesCounter { get; } = new DictionaryBasedOccurrencesCounter();

    private readonly IDictionary<char, int> _sbwtFirstOccurrences;
    private readonly IDictionary<char, IDictionary<int, int>> _bwtCounts;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This specific implementation also precalculates two dictionaries: the counts of <see cref="IMatcher.BWT"/> 
    /// and the first occurrence of each of the chars of <see cref="IMatcher.SortedBWT"/>. These two data structures
    /// makes single char matching a linear operation.
    /// </remarks>
    public CountBasedNarrowingIntervalMatcher(RotatedTextWithTerminator bwt, BWTransform.SortStrategy bwtSorter)
        : base(bwt, bwtSorter)
    {
        _bwtCounts = OccurrencesCounter.Count(BWT);
        _sbwtFirstOccurrences = Calculate1stOccurrences(SortedBWT);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc cref="CountBasedNarrowingIntervalMatcher(RotatedTextWithTerminator, BWTransform.SortStrategy)"/>
    /// </remarks>
    public CountBasedNarrowingIntervalMatcher(RotatedTextWithTerminator bwt, RotatedTextWithTerminator sbwt)
        : base(bwt, sbwt)
    {
        _bwtCounts = OccurrencesCounter.Count(BWT);
        _sbwtFirstOccurrences = Calculate1stOccurrences(SortedBWT);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Unlike <see cref="NarrowingIntervalMatcher"/>, this implementation of <see cref="IMatcher"/> doesn't make 
    /// explicit calls to <see cref="ILastFirstFinder.LastToFirst(int)"/>. Instead it solely uses its precomputed 
    /// structures and uses the last-first property implicitely when narrowing the current interval via such strucutes
    /// in <see cref="NarrowInterval(char, ILastFirstFinder, int, int)"/>.
    /// <br/>
    /// Because of that, it doesn't need an optimized <see cref="ILastFirstFinder"/>, and in particular one which does
    /// precomputation (such as the <see cref="PrecomputedFinder"/> used by <see cref="NarrowingIntervalMatcher"/>), 
    /// and can just instantiate a <see cref="NaiveFinder"/> instead.
    /// </remarks>
    protected override ILastFirstFinder BuildLastFirstFinder() => new NaiveFinder(BWT, SortedBWT);

    private static IDictionary<T, int> Calculate1stOccurrences<T>(IEnumerable<T> enumerable)
        where T: notnull
    {
        var result = new Dictionary<T, int> { };
        var index = 0;
        foreach (var item in enumerable)
        {
            if (!result.TryGetValue(item, out var value))
                result[item] = index;
            index++;
        }

        return result;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Narrowing is performed in three sub-steps (compared to the five in <see cref="NarrowingIntervalMatcher"/>):
    /// <br/>
    /// 1. The new start index is calculated as the 1st occurrence in <see cref="IMatcher.SortedBWT"/> of the current 
    /// char + the count of such char in <see cref="IMatcher.BWT"/> up to the current start index excluded (i.e. the 
    /// number of occurrences of the char up to the index before the current start index);
    /// <br/>
    /// 2. The new end index is calculated as the 1st occurrence in <see cref="IMatcher.SortedBWT"/> of the current
    /// char + the count of such char in <see cref="IMatcher.BWT"/> up to the current end index included, short of one 
    /// (i.e. the number of occurrences of the char up to the current end index - 1);
    /// <br/>
    /// 3. the narrowed interval in Sorted BWT is returned.
    /// <br/>
    /// </remarks>
    protected override (bool success, int narrowedStartIndex, int narrowedEndIndex) NarrowInterval(
        char currentChar, ILastFirstFinder finder, int startIndex, int endIndex)
    {
        if (!_sbwtFirstOccurrences.TryGetValue(currentChar, out var currentChar1stOccurrence))
            return (false, startIndex, endIndex);
        var currentCharCounts = _bwtCounts[currentChar];
        var narrowedStartIndex = currentChar1stOccurrence + (startIndex >= 1 ? currentCharCounts[startIndex - 1] : 0);
        var narrowedEndIndex = currentChar1stOccurrence + currentCharCounts[endIndex] - 1;
        return (true, narrowedStartIndex, narrowedEndIndex);
    }
}
