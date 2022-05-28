using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="ILastFirstFinder"/> implementation which just iterate over BWT and its sorted version every time.
/// Each operation has Time Complexity = O(n) and Space Complexity = O(1).
/// </summary>
/// <param name="LastBWMColumn">The last column of the Barrows-Wheeler Matrix. Corresponds to the BWT.</param>
public record NaiveLastFirstFinder(RotatedTextWithTerminator LastBWMColumn) : ILastFirstFinder
{
    /// <inheritdoc/>
    public IList<char> BWT { get; } = 
        LastBWMColumn.ToList();

    /// <inheritdoc/>
    public IList<char> SortedBWT { get; } = 
        LastBWMColumn.OrderBy(c => c, new CharOrTerminatorComparer(LastBWMColumn.Terminator)).ToList();

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation just iterates over <see cref="BWT"/> every time. 
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrence) =>
        Enumerable.Range(0, BWT.Count).Where(index => BWT[index] == charToFind).ElementAt(occurrence);

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation just iterates over <see cref="SortedBWT"/> every time.
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public int FindOccurrenceOfCharInSortedBWT(int indexOfChar) =>
        Enumerable.Range(0, indexOfChar).Count(index => SortedBWT[index] == SortedBWT[indexOfChar]);
}
