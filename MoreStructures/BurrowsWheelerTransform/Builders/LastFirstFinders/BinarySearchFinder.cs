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
    {
        if (indexOfChar < 0 || indexOfChar >= SortedBWT.Length)
            throw new ArgumentException($"Invalid {nameof(indexOfChar)}: {indexOfChar}");

        return indexOfChar - BinarySearchWithRepetitions(
            SortedBWT, SortedBWT[indexOfChar], CharComparer, 0, indexOfChar);
    }

    /// <summary>
    /// Find the index of the first element in the sub-sequence of elements of <paramref name="list"/> from 
    /// <paramref name="fromIndex"/> to <paramref name="toIndex"/> included, which is equal to 
    /// <paramref name="elementToFind"/>, assuming that <paramref name="list"/> is sorted in ascending order.
    /// </summary>
    /// <param name="list">The directly addressable sequence where to seach the element.</param>
    /// <param name="elementToFind">The element to search for.</param>
    /// <param name="comparer">The comparer to be used when performing the search.</param>
    /// <param name="fromIndex">
    /// The first index, marking the begin of the sub-sequence of <paramref name="list"/> where to search.
    /// </param>
    /// <param name="toIndex">
    /// The last index, marking the end of the sub-sequence of <paramref name="list"/> where to search.
    /// </param>
    /// <returns>The index.</returns>
    protected static int BinarySearchWithRepetitions<T>(
        IEnumerable<T> list, T elementToFind, IComparer<T> comparer, int fromIndex, int toIndex)
    {
        var start = fromIndex;
        var end = toIndex;
        while (!Equals(list.ElementAt(start), elementToFind))
        {
            var middle = (start + end) / 2;
            if (comparer.Compare(list.ElementAt(middle), elementToFind) >= 0)
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
    /// <param name="text">The string where to seach the char.</param>
    /// <param name="charToFind">The char to search for.</param>
    /// <param name="comparer">The comparer to be used when performing the search.</param>
    /// <param name="fromIndex">
    /// The first index, marking the begin of the substring of <paramref name="text"/> where to search.
    /// </param>
    /// <param name="toIndex">
    /// The last index, marking the end of the substring of <paramref name="text"/> where to search.
    /// </param>
    /// <returns>The index.</returns>
    protected static int BinarySearchWithRepetitions(
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
