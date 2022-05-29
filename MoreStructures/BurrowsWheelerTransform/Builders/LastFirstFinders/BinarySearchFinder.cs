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
    /// <inheritdoc/>
    public BinarySearchFinder(RotatedTextWithTerminator lastBWMColumn) : base(lastBWMColumn)
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This implementation takes advantage of the fact that <see cref="ILastFirstFinder.SortedBWT"/> is sorted.
    /// Time Complexity = O(log(n)). Space Complexity = O(1).
    /// </remarks>
    public override int FindOccurrenceOfCharInSortedBWT(int indexOfChar)
    { if (indexOfChar < 0 || indexOfChar >= SortedBWT.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        return indexOfChar - BinarySearchWithRepetitions(
            SortedBWT, SortedBWT[indexOfChar], CharComparer, 0, indexOfChar);
    }

    /// <summary>
    /// Find the index of the first element in the sub-sequence of elements of <paramref name="enumerable"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="element"/>, assuming that <paramref name="enumerable"/> is sorted in ascending order.
    /// </summary>
    /// <param name="enumerable">The enumerable where to search for <paramref name="element"/>.</param>
    /// <param name="element">The element to search for.</param>
    /// <param name="comparer">The comparer to be used when performing the search.</param>
    /// <param name="fromIndex">
    /// The first index, marking the begin of the sub-sequence of <paramref name="enumerable"/> where to search.
    /// </param>
    /// <param name="toIndex">
    /// The last index, marking the end of the sub-sequence of <paramref name="enumerable"/> where to search.
    /// </param>
    /// <returns>The index of <paramref name="element"/> in <paramref name="enumerable"/>.</returns>
    protected static int BinarySearchWithRepetitions<T>(
        IEnumerable<T> enumerable, T element, IComparer<T> comparer, int fromIndex, int toIndex)
    {
        // Optimization for strings
        if (enumerable is IEnumerable<char> chars && 
            element is char charToFind && 
            comparer is IComparer<char> charComparer)
        {
            var str = string.Concat(chars);
            return BinarySearchWithRepetitionsInText(str, charToFind, charComparer, fromIndex, toIndex);
        }

        var start = fromIndex;
        var end = toIndex;
        while (!Equals(enumerable.ElementAt(start), element))
        {
            var middle = (start + end) / 2;
            if (comparer.Compare(enumerable.ElementAt(middle), element) >= 0)
                end = middle;
            else
                start = middle + 1;
        }

        return start;
    }

    /// <summary>
    /// Find the index of the first char in the substring of <paramref name="text"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="charToFind"/>, assuming that <paramref name="text"/> is sorted in ascending order.
    /// </summary>
    /// <param name="text">The string where to search for <paramref name="charToFind"/>.</param>
    /// <param name="charToFind">The char to search for.</param>
    /// <param name="comparer">The comparer to be used when performing the search.</param>
    /// <param name="fromIndex">
    /// The first index, marking the begin of the substring of <paramref name="text"/> where to search.
    /// </param>
    /// <param name="toIndex">
    /// The last index, marking the end of the substring of <paramref name="text"/> where to search.
    /// </param>
    /// <returns>The index of <paramref name="charToFind"/> in <paramref name="text"/>.</returns>
    private static int BinarySearchWithRepetitionsInText(
        string text, char charToFind, IComparer<char> comparer, int fromIndex, int toIndex)
    {
        var start = fromIndex;
        var end = toIndex;
        while (!Equals(text[start], charToFind))
        {
            var middle = (start + end) / 2;
            if (comparer.Compare(text[middle], charToFind) >= 0)
                end = middle;
            else
                start = middle + 1;
        }

        return start;
    }
}
