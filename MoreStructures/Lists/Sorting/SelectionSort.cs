namespace MoreStructures.Lists.Sorting;

/// <summary>
/// An <see cref="IInPlaceSorting"/> implementation based on selection sort.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - The algorithm performs sorting in place and is online. 
///       <br/>
///     - It is not stable in its basic form and requires additional space or specific assumptions on the type of list 
///       being sorted (such as it being a linked list).
///       <br/>
///     - Compared to other quadratic comparison-based algorithms, such as <see cref="InsertionSort"/>, it is generally
///       simpler but requires in average an higher number of comparisons, therefore yielding worse performance.
///       <br/>
///     - Compared to linearithmic comparison-based algorithms, such as <see cref="HeapSort"/>, it is way simpler to
///       understand and predict in exact number of operations executed. However, the performance is sensibly worse.
///       <br/>
///     - Compared to non-comparison-based algorithms, such as counting sort, it doesn't require any assumption on the
///       type or values of the items in the input, the only requirement being their total comparability and the
///       comparison behaving according to total order operators rules.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - This sorting algorithm split the list L being sorted in two parts: the sorted part, located at the beginning 
///       of the list (L[..i]), and the unsorted part, located at the end of the list (L[i..]).
///       <br/>
///     - At the beginning the sorted part is empty (i.e. length 0) and the unsorted part covers the entire list (i.e.
///       length n).
///       <br/>
///     - The algorithm runs n iterations, where n is the number of items in the list.
///       <br/>
///     - At the beginning of iteration i, the sorted sub-list is L[..i] and the unsorted sub-list is L[i..].
///       <br/>
///     - The unsorted sub-list L[i..] is scanned linearly, looking for the index j, between i and n - 1, of the item 
///       of L[i..] with minimum value.
///       <br/>
///     - L[i] is swapped with L[j] and the iteration i terminates.
///       <br/>
///     - Now L[..(i + 1)] is the new sorted sub-list, and L[(i + 1)..] is the new unsorted sub-list.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Each of the n iterations runs n - i - 1 comparisons, to identify the index of the item with the minimum value
///       in the sub-list L[i..].
///       <br/>
///     - The total number of comparisons, over the n iterations, is around n * n / 2.
///       <br/>
///     - Therefore, Time Complexity is O(n^2) and Space Complexity is O(1), since the algorithm runs in place.
///     </para>
/// </remarks>
public class SelectionSort : IInPlaceSorting
{
    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the selection sort algorithm with the default comparer for <typeparamref name="T"/>, given by
    ///     <see cref="Comparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="SelectionSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list) where T : IComparable<T> => 
        Sort(list, Comparer<T>.Default);

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the selection sort algorithm with the specified <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="SelectionSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list, IComparer<T> comparer)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var k = SelectIndexOfSmallestItem(list, comparer, i);
            (list[i], list[k]) = (list[k], list[i]);
        }
    }

    private static int SelectIndexOfSmallestItem<T>(IList<T> list, IComparer<T> comparer, int startIndex)
    {
        int smallestItemIndex = startIndex;
        for (var j = startIndex + 1; j < list.Count; j++)
            if (comparer.Compare(list[j], list[startIndex]) < 0)
                smallestItemIndex = j;
        return smallestItemIndex;
    }
}
