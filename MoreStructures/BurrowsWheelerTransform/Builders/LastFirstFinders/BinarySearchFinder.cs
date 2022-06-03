namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A <see cref="NaiveFinder"/> refinement which iterates over <see cref="ILastFirstFinder.BWT"/> and uses 
/// binary search on <see cref="ILastFirstFinder.SortedBWT"/>, taking advantage of the fact that it is sorted.
/// </summary>
/// <remarks>
///     <para id="complexity">
///     Search over <see cref="ILastFirstFinder.BWT"/> has Time Complexity = O(n).
///     <br/>
///     Search over <see cref="ILastFirstFinder.SortedBWT"/> has Time Complexity O(log(n)).
///     <br/>
///     Space Complexity = O(1) for both search operations.
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
    public BinarySearchFinder(RotatedTextWithTerminator lastBWMColumn, BWTransform.SortStrategy bwtSorter) 
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
    public override int FindIndexOfNthOccurrenceInSortedBWT(int indexOfCharInBWT, int occurrenceRank)
    {
        if (occurrenceRank < 0)
            throw new ArgumentOutOfRangeException(nameof(occurrenceRank), $"Must be non-negative: {occurrenceRank}.");

        var index = OrderedAscListSearch.Nth(SortedBWT, BWT[indexOfCharInBWT], occurrenceRank, CharComparer);

        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(occurrenceRank), $"Invalid value: {occurrenceRank}.");
        return index;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation takes advantage of the fact that <see cref="ILastFirstFinder.SortedBWT"/> is sorted.
    /// Time Complexity = O(log(n)). Space Complexity = O(1).
    /// </remarks>
    public override int FindOccurrenceRankOfCharInSortedBWT(int indexOfCharInSortedBWT)
    { 
        if (indexOfCharInSortedBWT < 0 || indexOfCharInSortedBWT >= SortedBWT.Length)
            throw new ArgumentOutOfRangeException(
                nameof(indexOfCharInSortedBWT), $"Invalid value: {indexOfCharInSortedBWT}");

        return indexOfCharInSortedBWT - OrderedAscListSearch.First(
            SortedBWT, SortedBWT[indexOfCharInSortedBWT], CharComparer, 0, indexOfCharInSortedBWT);
    }
}
