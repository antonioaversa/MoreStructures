namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;


/// <summary>
/// A <see cref="BinarySearchFinder"/> refinement which precalculate an hash-map of all the positions by each
/// char, for both BWT and its sorted version, which takes ~ 2 * n space and makes searches in .
/// </summary>
public class PrecomputedFinder : BinarySearchFinder
{
    /// <summary>
    /// The <see cref="Lists.Searching.ISearch"/> implementation to be used when searching for items in lists not 
    /// sorted in any order.
    /// </summary>
    protected static Lists.Searching.ISearch UnorderedListSearch { get; } = new Lists.Searching.LinearSearch();

    private readonly IDictionary<char, IList<int>> _bwtOccurrenceIndexesOfChar;
    private readonly IDictionary<char, IList<int>> _sbwtOccurrenceIndexesOfChar;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc cref="PrecomputedFinder" path="/summary"/>
    /// </remarks>
    public PrecomputedFinder(RotatedTextWithTerminator lastBWMColumn, BWTransform.SortStrategy bwtSorter)
        : base(lastBWMColumn, bwtSorter)
    {
        _bwtOccurrenceIndexesOfChar = GetOccurrenceIndexesOfAllCharsIn(BWT);
        _sbwtOccurrenceIndexesOfChar = GetOccurrenceIndexesOfAllCharsIn(SortedBWT);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc cref="PrecomputedFinder" path="/summary"/>
    /// </remarks>
    public PrecomputedFinder(
        RotatedTextWithTerminator lastBWMColumn,
        RotatedTextWithTerminator firstBWMColumn)
        : base(lastBWMColumn, firstBWMColumn)
    {
        _bwtOccurrenceIndexesOfChar = GetOccurrenceIndexesOfAllCharsIn(BWT);
        _sbwtOccurrenceIndexesOfChar = GetOccurrenceIndexesOfAllCharsIn(SortedBWT);
    }

    private static IDictionary<char, IList<int>> GetOccurrenceIndexesOfAllCharsIn(IEnumerable<char> chars)
    {
        var result = new Dictionary<char, IList<int>>() { };
        var index = 0;
        foreach (var c in chars)
        {
            if (!result.ContainsKey(c))
                result[c] = new List<int> { };
            result[c].Add(index);
            index++;
        }
        return result;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation uses a precomputed hash-map of all the positions by each char.
    /// Time Complexity = O(1). Space Complexity = O(1).
    /// </remarks>
    public override int FindIndexOfNthOccurrenceInBWT(int indexOfCharInBWT, int occurrenceRank)
    {
        if (indexOfCharInBWT < 0 || indexOfCharInBWT >= BWT.Length)
            throw new ArgumentOutOfRangeException(nameof(indexOfCharInBWT), $"Invalid {nameof(indexOfCharInBWT)}.");

        var charToFind = BWT[indexOfCharInBWT];

        return _bwtOccurrenceIndexesOfChar[charToFind][occurrenceRank];
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="FindIndexOfNthOccurrenceInBWT(int, int)"/>
    /// </remarks>
    public override int FindIndexOfNthOccurrenceInSortedBWT(int indexOfCharInBWT, int occurrenceRank)
    {
        if (indexOfCharInBWT < 0 || indexOfCharInBWT >= BWT.Length)
            throw new ArgumentOutOfRangeException(nameof(indexOfCharInBWT), $"Invalid value: {indexOfCharInBWT}.");

        var charToFind = BWT[indexOfCharInBWT];

        if (occurrenceRank < 0 || occurrenceRank >= _sbwtOccurrenceIndexesOfChar[charToFind].Count)
            throw new ArgumentOutOfRangeException(nameof(occurrenceRank), $"Invalid value: {occurrenceRank}.");

        return _sbwtOccurrenceIndexesOfChar[charToFind].First() + occurrenceRank;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para>
    ///     This implementation uses a precomputed hash-map of all the positions by each char.
    ///     </para>
    ///     <para>
    ///     However, unlike <see cref="ILastFirstFinder.SortedBWT"/>, <see cref="ILastFirstFinder.BWT"/> is not sorted,
    ///     so the precomputed list storing all the indexes where the char of <see cref="ILastFirstFinder.BWT"/> at 
    ///     index <paramref name="indexOfCharInBWT"/> appears can be accessed in O(1) but has to be iterated over 
    ///     linearly. 
    ///     <br/>
    ///     Such a list has in average n / sigma elements, where sigma is the number of distinct chars in the text. 
    ///     If sigma is constant, the Time Complexity is O(n). 
    ///     <br/>
    ///     Space Complexity is always O(1), since O(n * sigma) space has already been allocated to host the result of
    ///     counts and first occurrences precomputation.
    ///     </para>
    /// </remarks>
    public override int FindOccurrenceRankOfCharInBWT(int indexOfCharInBWT)
    {
        if (indexOfCharInBWT < 0 || indexOfCharInBWT >= BWT.Length)
            throw new ArgumentOutOfRangeException(nameof(indexOfCharInBWT), $"Invalid value: {indexOfCharInBWT}");

        var indexesOfChar = _bwtOccurrenceIndexesOfChar[BWT[indexOfCharInBWT]];
        return UnorderedListSearch.First(indexesOfChar, indexOfCharInBWT);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para>
    ///     This implementation uses a precomputed hash-map of all the positions by each char.
    ///     </para>
    ///     <para>
    ///     It also takes advantage of the fact that <see cref="ILastFirstFinder.SortedBWT"/> is sorted, by running a 
    ///     Binary Search on it, which takes logarithmic time over the list of indexes for the char at position 
    ///     <paramref name="indexOfCharInSortedBWT"/> in <see cref="ILastFirstFinder.BWT"/>. 
    ///     <br/>
    ///     Such list has average size = n / sigma, where n = number of chars in 
    ///     <see cref="ILastFirstFinder.SortedBWT"/> and sigma = size of the alphabet of 
    ///     <see cref="ILastFirstFinder.SortedBWT"/>.
    ///     <br/>
    ///     If sigma is constant, the list has a size O(n).
    ///     </para>
    ///     <para>
    ///     Therefore, Time Complexity = O(log(n)) and Space Complexity = O(1).
    ///     </para>
    /// </remarks>
    public override int FindOccurrenceRankOfCharInSortedBWT(int indexOfCharInSortedBWT)
    {
        if (indexOfCharInSortedBWT < 0 || indexOfCharInSortedBWT >= SortedBWT.Length)
            throw new ArgumentOutOfRangeException(
                nameof(indexOfCharInSortedBWT), $"Invalid value: {indexOfCharInSortedBWT}");

        var indexesOfChar = _sbwtOccurrenceIndexesOfChar[SortedBWT[indexOfCharInSortedBWT]];
        return OrderedAscListSearch.First(indexesOfChar, indexOfCharInSortedBWT);
    }
}