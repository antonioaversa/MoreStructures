using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="ILastFirstFinder"/> implementation which precalculate an hash-map of all the positions by each
/// char, for both BWT and its sorted version, which takes ~ 2 * n space. Every call then takes constant amortized
/// time. Time Complexity = O(1) and Space Complexity = O(n).
/// </summary>
public class PrecomputedDictionaryLastFirstFinder : ILastFirstFinder
{
    private readonly IDictionary<char, IList<int>> _bwtOccurrencesByChar;
    private readonly IDictionary<char, IList<int>> _sbwtOccurrencesByChar;

    /// <inheritdoc/>
    public IList<char> BWT { get; }

    /// <inheritdoc/>
    public IList<char> SortedBWT { get; }

    /// <summary>
    /// Builds a <see cref="PrecomputedDictionaryLastFirstFinder"/>.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="PrecomputedDictionaryLastFirstFinder" path="/summary"/>
    /// </remarks>
    /// <param name="lastBWMColumn">The last column of the Barrows-Wheeler Matrix. Corresponds to the BWT.</param>
    public PrecomputedDictionaryLastFirstFinder(RotatedTextWithTerminator lastBWMColumn)
    {
        BWT = lastBWMColumn.ToList();
        SortedBWT = lastBWMColumn.OrderBy(c => c, new CharOrTerminatorComparer(lastBWMColumn.Terminator)).ToList();
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

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation úses a precomputed hash-map of all the positions by each char.
    /// Time Complexity = O(1). Space Complexity = O(1).
    /// </remarks>
    public int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrence) =>
        _bwtOccurrencesByChar[charToFind][occurrence];

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation úses a precomputed hash-map of all the positions by each char.
    /// Time Complexity = O(1). Space Complexity = O(1).
    /// </remarks>
    public int FindOccurrenceOfCharInSortedBWT(int indexOfChar) =>
        _sbwtOccurrencesByChar[SortedBWT[indexOfChar]].IndexOf(indexOfChar);
}