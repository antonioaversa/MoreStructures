using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="ILastFirstFinder"/> implementation which just iterates over <see cref="BWT"/> and its sorted version 
/// <see cref="SortedBWT"/> every time.
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
    public NaiveFinder(RotatedTextWithTerminator lastBWMColumn, BWTransform.SortStrategy bwtSorter)
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
    /// The last column of the Burrows-Wheeler Matrix. Corresponds to the <see cref="BWT"/>.
    /// </param>
    /// <param name="firstBWMColumn">
    /// The first column of the Burrows-Wheeler Matrix. Corresponds to the <see cref="SortedBWT"/>.
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
            throw new ArgumentOutOfRangeException(nameof(occurrenceRank), $"Must be non-negative: {occurrenceRank}.");

        return Enumerable
            .Range(0, text.Length)
            .Where(index => text[index] == charToFind)
            .Cast<int?>()
            .ElementAtOrDefault(occurrenceRank)
            ?? throw new ArgumentOutOfRangeException(
                $"Invalid {nameof(charToFind)} or {nameof(occurrenceRank)}: {charToFind}, {occurrenceRank}");
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="BWT"/> every time. 
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindIndexOfNthOccurrenceInBWT(int indexOfCharInBWT, int occurrenceRank) =>
        FindIndexOfNthOccurrenceInText(BWT, BWT[indexOfCharInBWT], occurrenceRank);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="SortedBWT"/> every time. 
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindIndexOfNthOccurrenceInSortedBWT(int indexOfCharInBWT, int occurrenceRank) =>
        FindIndexOfNthOccurrenceInText(SortedBWT, BWT[indexOfCharInBWT], occurrenceRank);

    private static int FindOccurrenceRankOfCharInText(
        RotatedTextWithTerminator text, int indexOfCharInText)
    {
        if (indexOfCharInText < 0 || indexOfCharInText >= text.Length)
            throw new ArgumentOutOfRangeException(nameof(indexOfCharInText), $"Invalid value: {indexOfCharInText}");

        return Enumerable
            .Range(0, indexOfCharInText)
            .Count(index => text[index] == text[indexOfCharInText]);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="BWT"/> every time.
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindOccurrenceRankOfCharInBWT(int indexOfCharInBWT) =>
        FindOccurrenceRankOfCharInText(BWT, indexOfCharInBWT);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation just iterates over <see cref="SortedBWT"/> every time.
    /// Time Complexity = O(n). Space Complexity = O(1).
    /// </remarks>
    public virtual int FindOccurrenceRankOfCharInSortedBWT(int indexOfCharInSortedBWT) => 
        FindOccurrenceRankOfCharInText(SortedBWT, indexOfCharInSortedBWT);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// First executes <see cref="FindOccurrenceRankOfCharInBWT(int)"/>, to find the occurrence rank of the char at
    /// index <paramref name="indexOfCharInBWT"/> and then uses the last-to-first property to find the corresponding 
    /// char in <see cref="SortedBWT"/> by using <see cref="FindIndexOfNthOccurrenceInSortedBWT(int, int)"/>.
    /// <br/>
    /// Time and Space Complexity depends on the implementation of <see cref="FindOccurrenceRankOfCharInBWT(int)"/> and
    /// <see cref="FindIndexOfNthOccurrenceInSortedBWT(int, int)"/>.
    /// </remarks>
    public virtual (int indexInSortedBWT, int occurrenceRank) LastToFirst(int indexOfCharInBWT)
    {
        var occurrenceRank = FindOccurrenceRankOfCharInBWT(indexOfCharInBWT);
        return (FindIndexOfNthOccurrenceInSortedBWT(indexOfCharInBWT, occurrenceRank), occurrenceRank);
    }
}
