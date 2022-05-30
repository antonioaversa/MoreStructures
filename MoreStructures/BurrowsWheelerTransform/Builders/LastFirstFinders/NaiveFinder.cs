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

    private static int FindIndexOfNthOccurrenceInText(
        RotatedTextWithTerminator text, char charToFind, int occurrenceRank)
    {
        if (occurrenceRank < 0)
            throw new ArgumentException("Must be non-negative.", nameof(occurrenceRank));

        return Enumerable
            .Range(0, text.Length)
            .Where(index => text[index] == charToFind)
            .Cast<int?>()
            .ElementAtOrDefault(occurrenceRank)
            ?? throw new ArgumentException(
                $"Invalid {nameof(charToFind)} or {nameof(occurrenceRank)}: {charToFind}, {occurrenceRank}");
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="BWT"/> every time. 
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrenceRank) =>
        FindIndexOfNthOccurrenceInText(BWT, charToFind, occurrenceRank);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="SortedBWT"/> every time. 
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindIndexOfNthOccurrenceInSortedBWT(char charToFind, int occurrenceRank) =>
        FindIndexOfNthOccurrenceInText(SortedBWT, charToFind, occurrenceRank);

    private static int FindOccurrenceRankOfCharInText(
        RotatedTextWithTerminator text, int indexOfChar)
    {
        if (indexOfChar < 0 || indexOfChar >= text.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        return Enumerable
            .Range(0, indexOfChar)
            .Count(index => text[index] == text[indexOfChar]);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="BWT"/> every time.
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindOccurrenceRankOfCharInBWT(int indexOfChar) =>
        FindOccurrenceRankOfCharInText(BWT, indexOfChar);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="SortedBWT"/> every time.
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindOccurrenceRankOfCharInSortedBWT(int indexOfChar) => 
        FindOccurrenceRankOfCharInText(SortedBWT, indexOfChar);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// First executes <see cref="FindOccurrenceRankOfCharInBWT(int)"/>, to find the occurrence rank of the char at
    /// index <paramref name="indexOfChar"/> and then uses the last-to-first property to find the corresponding char
    /// in <see cref="SortedBWT"/> by using <see cref="FindIndexOfNthOccurrenceInSortedBWT(char, int)"/>.
    /// <br/>
    /// Time and Space Complexity depends on the implementation of <see cref="FindOccurrenceRankOfCharInBWT(int)"/> and
    /// <see cref="FindIndexOfNthOccurrenceInSortedBWT(char, int)"/>.
    /// </remarks>
    public (int indexInSortedBWT, int occurrenceRank) LastToFirst(int indexOfChar)
    {
        var occurrenceRank = FindOccurrenceRankOfCharInBWT(indexOfChar);
        return (FindIndexOfNthOccurrenceInSortedBWT(BWT[indexOfChar], occurrenceRank), occurrenceRank);
    }
}
