using MoreStructures.Utilities;
using System.Linq;
using System.Text;

namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="ILastFirstFinder"/> implementation which just iterate over BWT and its sorted version every time.
/// Each operation has Time Complexity = O(n) and Space Complexity = O(1).
/// </summary>
public class NaiveFinder : ILastFirstFinder
{
    /// <inheritdoc/>
    public IComparer<char> CharComparer { get; }

    /// <inheritdoc/>
    public RotatedTextWithTerminator BWT { get; }

    /// <inheritdoc/>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// Builds an instance of this finder, for the provided <paramref name="lastBWMColumn"/>, using 
    /// <paramref name="bwtSorter"/> to calculate <see cref="SortedBWT"/> from it.
    /// </summary>
    /// <param name="lastBWMColumn">The last column of the Burrows-Wheeler Matrix. Corresponds to the BWT.</param>
    /// <param name="bwtSorter">
    /// A function sorting the provided<see cref="RotatedTextWithTerminator"/> into a sorted
    /// <see cref="RotatedTextWithTerminator"/>, according to the provided <see cref="IComparer{T}"/> of chars.
    /// </param>
    public NaiveFinder(
        RotatedTextWithTerminator lastBWMColumn, 
        Func<RotatedTextWithTerminator, IComparer<char>, RotatedTextWithTerminator> bwtSorter)
    {
        CharComparer = CharOrTerminatorComparer.Build(lastBWMColumn.Terminator);
        BWT = lastBWMColumn;
        SortedBWT = bwtSorter(BWT, CharComparer);
    }

    /// <summary>
    /// Builds an instance of this finder, for the provided <paramref name="lastBWMColumn"/> and 
    /// <paramref name="firstBWMColumn"/>. Because both columns of the BWM are provided, no sorting happens.
    /// </summary>
    /// <param name="lastBWMColumn">
    /// The last column of the Burrows-Wheeler Matrix. Corresponds to the BWT.
    /// </param>
    /// <param name="firstBWMColumn">
    /// The first column of the Burrows-Wheeler Matrix. Corresponds to the Sorted BWT.
    /// </param>
    public NaiveFinder(RotatedTextWithTerminator lastBWMColumn, RotatedTextWithTerminator firstBWMColumn)
    {
        CharComparer = CharOrTerminatorComparer.Build(lastBWMColumn.Terminator);
        BWT = lastBWMColumn;
        SortedBWT = firstBWMColumn;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="BWT"/> every time. 
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrence) =>
        Enumerable
            .Range(0, BWT.Length)
            .Where(index => BWT[index] == charToFind)
            .Cast<int?>()
            .ElementAtOrDefault(occurrence)
            ?? throw new ArgumentException(
                $"Invalid {nameof(charToFind)} or {nameof(occurrence)}: {charToFind}, {occurrence}");

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="SortedBWT"/> every time.
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindOccurrenceOfCharInSortedBWT(int indexOfChar)
    {
        if (indexOfChar < 0 || indexOfChar >= SortedBWT.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        return Enumerable
            .Range(0, indexOfChar)
            .Count(index => SortedBWT[index] == SortedBWT[indexOfChar]);
    }
}
