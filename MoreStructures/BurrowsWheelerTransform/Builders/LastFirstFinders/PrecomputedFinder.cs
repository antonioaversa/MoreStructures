using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="BinarySearchFinder"/> refinement which precalculate an hash-map of all the positions by each
/// char, for both BWT and its sorted version, which takes ~ 2 * n space.
/// </summary>
public class PrecomputedFinder : BinarySearchFinder
{
    private readonly IDictionary<char, IList<int>> _bwtOccurrencesByChar;
    private readonly IDictionary<char, IList<int>> _sbwtOccurrencesByChar;

    /// <summary>
    /// Builds a <see cref="PrecomputedFinder"/>.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="PrecomputedFinder" path="/summary"/>
    /// </remarks>
    /// <param name="lastBWMColumn">The last column of the Burrows-Wheeler Matrix. Corresponds to the BWT.</param>
    public PrecomputedFinder(RotatedTextWithTerminator lastBWMColumn)
        :base(lastBWMColumn)
    {
        _bwtOccurrencesByChar = GetOccurrencesByChar(BWT);
        _sbwtOccurrencesByChar = GetOccurrencesByChar(SortedBWT);
    }

    private static IDictionary<char, IList<int>> GetOccurrencesByChar(IEnumerable<char> chars)
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
    public override int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrence) =>
        _bwtOccurrencesByChar.ContainsKey(charToFind) && 
        occurrence >= 0 && occurrence < _bwtOccurrencesByChar[charToFind].Count
        ? _bwtOccurrencesByChar[charToFind][occurrence]
        : throw new ArgumentException($"Invalid {nameof(charToFind)} or {nameof(occurrence)}");

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
    public override int FindOccurrenceOfCharInSortedBWT(int indexOfChar)
    {
        if (indexOfChar < 0 || indexOfChar >= SortedBWT.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        var indexesOfChar = _sbwtOccurrencesByChar[SortedBWT[indexOfChar]];
        return BinarySearchWithRepetitions(indexesOfChar, indexOfChar, Comparer<int>.Default, 0, indexesOfChar.Count);
    }
}