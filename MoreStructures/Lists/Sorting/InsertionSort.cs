namespace MoreStructures.Lists.Sorting;

/// <summary>
/// An <see cref="IInPlaceSorting"/> implementation based on insertion sort.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - This sorting algorithm split the list L being sorted in two parts: the sorted part, located at the beginning 
///       of the list (L[..i]), and the unsorted part, located at the end of the list (L[i..]).
///       <br/>
///     - At the beginning the sorted part is empty (i.e. length 0) and the unsorted part covers the entire list (i.e.
///       length n).
///       <br/>
///     - The algorithm runs n - 1 1-based iterations, where n is the number of items in the list.
///       <br/>
///     - At the beginning of iteration i, the sorted sub-list is L[..i] and the unsorted sub-list is L[i..].
///       <br/>
///     - The first item L[i], of the unsorted sub-list L[i..], is compared against its predecessor, L[i - 1].
///       <br/>
///     - If L[i] is smaller than L[i - 1], the two items are swapped and the new L[i - 1] is compared with L[i - 2].
///       Comparisons and swapping continues until the predecessor is not bigger than its successor, potentially until
///       the head of the list is reached.
///       <br/>
///     - When a L[j] is found, which is not strictly smaller than L[j - 1], L[.. (i + 1)] is sorted, and the iteration
///       i can terminate.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Each of the n - 1 iterations runs at most i - 1 comparisons, if it has to swap all the way up to the head of 
///       the list.
///       <br/>
///     - The total number of comparisons, over the n iterations, is around n * n / 2.
///       <br/>
///     - Therefore, Time Complexity is O(n^2) and Space Complexity is O(1), since the algorithm runs in place and 
///       hence only requires additional constant space to perform the sorting.
///     </para>
/// </remarks>
public class InsertionSort : IInPlaceSorting
{
    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the insertion sort algorithm with the default comparer for <typeparamref name="T"/>, given by
    ///     <see cref="Comparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="InsertionSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list) where T : IComparable<T> => 
        Sort(list, Comparer<T>.Default);

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the insertion sort algorithm with the specified <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="InsertionSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list, IComparer<T> comparer) => 
        InsertionSortHelpers.InsertionSortOnHthIndexes(list, comparer, 1);
}
