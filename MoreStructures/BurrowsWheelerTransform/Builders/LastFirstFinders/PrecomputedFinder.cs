using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="BinarySearchFinder"/> refinement which precalculate an hash-map of all the positions by each
/// char, for both BWT and its sorted version, which takes ~ 2 * n space.
/// </summary>
public class PrecomputedFinder : BinarySearchFinder
{
    private readonly IDictionary<char, IList<int>> _bwtOccurrenceIndexesByChar;
    private readonly IDictionary<char, IList<int>> _sbwtOccurrenceIndexesByChar;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc cref="PrecomputedFinder" path="/summary"/>
    /// </remarks>
    public PrecomputedFinder(
        RotatedTextWithTerminator lastBWMColumn,
        Func<RotatedTextWithTerminator, IComparer<char>, RotatedTextWithTerminator> bwtSorter)
        : base(lastBWMColumn, bwtSorter)
    {
        _bwtOccurrenceIndexesByChar = GetOccurrenceIndexesByChar(BWT);
        _sbwtOccurrenceIndexesByChar = GetOccurrenceIndexesByChar(SortedBWT);
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
        _bwtOccurrenceIndexesByChar = GetOccurrenceIndexesByChar(BWT);
        _sbwtOccurrenceIndexesByChar = GetOccurrenceIndexesByChar(SortedBWT);
    }

    private static IDictionary<char, IList<int>> GetOccurrenceIndexesByChar(IEnumerable<char> chars)
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
    /// This implementation úses a precomputed hash-map of all the positions by each char.
    /// Time Complexity = O(1). Space Complexity = O(1).
    /// </remarks>
    public override int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrenceRank) =>
        _bwtOccurrenceIndexesByChar.ContainsKey(charToFind) && 
        occurrenceRank >= 0 && occurrenceRank < _bwtOccurrenceIndexesByChar[charToFind].Count
        ? _bwtOccurrenceIndexesByChar[charToFind][occurrenceRank]
        : throw new ArgumentException($"Invalid {nameof(charToFind)} or {nameof(occurrenceRank)}");

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="FindIndexOfNthOccurrenceInBWT(char, int)"/>
    /// </remarks>
    public override int FindIndexOfNthOccurrenceInSortedBWT(char charToFind, int occurrenceRank) =>
        _sbwtOccurrenceIndexesByChar.ContainsKey(charToFind) &&
        occurrenceRank >= 0 && occurrenceRank < _sbwtOccurrenceIndexesByChar[charToFind].Count
        ? _sbwtOccurrenceIndexesByChar[charToFind].First() + occurrenceRank
        : throw new ArgumentException($"Invalid {nameof(charToFind)} or {nameof(occurrenceRank)}");

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para>
    ///     This implementation uses a precomputed hash-map of all the positions by each char.
    ///     </para>
    ///     <para>
    ///     It also takes advantage of the fact that <see cref="ILastFirstFinder.SortedBWT"/> is sorted, by running a 
    ///     Binary Search on it, which takes logarithmic time over the list of indexes for the char at position 
    ///     <paramref name="indexOfChar"/> in <see cref="ILastFirstFinder.SortedBWT"/>. 
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
    public override int FindOccurrenceRankOfCharInBWT(int indexOfChar)
    {
        if (indexOfChar < 0 || indexOfChar >= BWT.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        var indexesOfChar = _bwtOccurrenceIndexesByChar[BWT[indexOfChar]];
        return Lists.Searching.Search.BinarySearchFirst(
            indexesOfChar, indexOfChar, Comparer<int>.Default, 0, indexesOfChar.Count);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="FindOccurrenceRankOfCharInBWT(int)"/>
    /// </remarks>
    public override int FindOccurrenceRankOfCharInSortedBWT(int indexOfChar)
    {
        if (indexOfChar < 0 || indexOfChar >= SortedBWT.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        var indexesOfChar = _sbwtOccurrenceIndexesByChar[SortedBWT[indexOfChar]];
        return Lists.Searching.Search.BinarySearchFirst(
            indexesOfChar, indexOfChar, Comparer<int>.Default, 0, indexesOfChar.Count);
    }
}