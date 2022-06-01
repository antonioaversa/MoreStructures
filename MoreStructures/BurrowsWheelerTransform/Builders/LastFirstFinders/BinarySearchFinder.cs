namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="NaiveFinder"/> refinement which iterates over <see cref="ILastFirstFinder.BWT"/> and uses 
/// binary search on <see cref="ILastFirstFinder.SortedBWT"/>.
/// </summary>
/// <remarks>
///     <para id="complexity">
///         <para>
///         Search over <see cref="ILastFirstFinder.BWT"/> has Time Complexity = O(n).
///         </para>
///         <para>
///         Search over <see cref="ILastFirstFinder.SortedBWT"/> has Time Complexity O(log(n)).
///         </para>
///         <para>
///         Space Complexity = O(1) for both search operations.
///         </para>
///     </para>
/// </remarks>
public class BinarySearchFinder : NaiveFinder
{
    /// <summary>
    /// The <see cref="Lists.Searching.ISearch"/> implementation to be used when searching for items in lists sorted 
    /// in ascending order.
    /// </summary>
    protected static Lists.Searching.ISearch OrderedAscListSearch { get; } = new Lists.Searching.BinarySearch();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc cref="BinarySearchFinder" path="/summary"/>
    /// </remarks>
    public BinarySearchFinder(
        RotatedTextWithTerminator lastBWMColumn,
        Func<RotatedTextWithTerminator, IComparer<char>, RotatedTextWithTerminator> bwtSorter) 
        : base(lastBWMColumn, bwtSorter)
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc cref="BinarySearchFinder" path="/summary"/>
    /// </remarks>
    public BinarySearchFinder(
        RotatedTextWithTerminator lastBWMColumn, 
        RotatedTextWithTerminator firstBWMColumn) 
        : base(lastBWMColumn, firstBWMColumn)
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation takes advantage of the fact that <see cref="ILastFirstFinder.SortedBWT"/> is sorted.
    /// Time Complexity = O(log(n)). Space Complexity = O(1).
    /// </remarks>
    public override int FindIndexOfNthOccurrenceInSortedBWT(char charToFind, int occurrenceRank)
    {
        if (occurrenceRank < 0)
            throw new ArgumentException("Must be non-negative.", nameof(occurrenceRank));

        var index = OrderedAscListSearch.Nth(SortedBWT, charToFind, occurrenceRank, CharComparer);

        if (index < 0)
            throw new ArgumentException($"Invalid {nameof(occurrenceRank)}: {occurrenceRank}");
        return index;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation takes advantage of the fact that <see cref="ILastFirstFinder.SortedBWT"/> is sorted.
    /// Time Complexity = O(log(n)). Space Complexity = O(1).
    /// </remarks>
    public override int FindOccurrenceRankOfCharInSortedBWT(int indexOfChar)
    { 
        if (indexOfChar < 0 || indexOfChar >= SortedBWT.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        return indexOfChar - OrderedAscListSearch.First(SortedBWT, SortedBWT[indexOfChar], CharComparer, 0, indexOfChar);
    }
}
