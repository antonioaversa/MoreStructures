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
    public string BWT { get; }

    /// <inheritdoc/>
    public string SortedBWT { get; }

    /// <summary>
    /// A function sorting the provided string into a sorted string, according to the provided 
    /// <see cref="IComparer{T}"/> of chars.
    /// </summary>
    /// <remarks>
    /// Used to calculate <see cref="SortedBWT"/> from <see cref="BWT"/>. The default uses 
    /// <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>,
    /// which uses a QuickSort with Time Complexity = O(n * log(n)) in average and O(n^2) in the worst case.
    /// </remarks>
    public Func<string, IComparer<char>, string> BWTSorter { get; init; }
        = (text, charComparer) => string.Concat(text.OrderBy(c => c, charComparer));

    /// <summary>
    /// Builds an instance of this finder, for the provided <paramref name="lastBWMColumn"/>.
    /// </summary>
    /// <param name="lastBWMColumn">The last column of the Burrows-Wheeler Matrix. Corresponds to the BWT.</param>
    public NaiveFinder(RotatedTextWithTerminator lastBWMColumn)
    {
        CharComparer = new CharOrTerminatorComparer(lastBWMColumn.Terminator);
        BWT = lastBWMColumn.RotatedText;
        SortedBWT = BWTSorter(BWT, CharComparer);
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
