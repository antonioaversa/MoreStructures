using MoreStructures.PriorityQueues.BinaryHeap;

namespace MoreStructures.Lists.Sorting;

/// <summary>
/// An <see cref="IInPlaceSorting"/> implementation based on heapsort.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - This sorting algorithm relies entirely on the max binary heap data structure.
///       <br/>
///     - Given the list L to sort, it builds an heap H out of the entire list, passing L as backing structure for H.
///       <br/>
///     - Then, it pops items from H, one by one, appending at the back of L, where the pop has left a hole.
///       <br/>
///     - For example if L is 10 items long, the first pop will leave the item at index 9 unoccupied, the second pop 
///       will leave the item at index 8 unoccupied (the item at index 9 already is out of the picture), etc.
///       <br/>
///     - Once the last item of H is popped, the heap is empty and L is sorted in ascending order.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Building a heap in batch from a list of n items takes a linear amount of time.
///       <br/>
///     - Each pop takes a logarithmic amount of time, due to the sift down required to restore the heap property.
///       <br/>
///     - Storing the popped item at the back of the list is a constant time operation.
///       <br/>
///     - Because the heap is built in place on the provided list, the list is never replicated in memroy and only a
///       constant amount of additional space is required, for heap re-adjustment operations.
///       <br/>
///     - Therefore, Time Complexity is O(n * log(n)) and Space Complexity is O(1), where n is the number of items
///       being sorted (which can be lower than the size of the provided list).
///     </para>
/// </remarks>
public class HeapSort : IInPlaceSorting
{
    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the heapsort algorithm with the default comparer for <typeparamref name="T"/>, given by
    ///     <see cref="Comparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="HeapSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list) where T : IComparable<T> => 
        Sort(list, Comparer<T>.Default);

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the heapsort algorithm with the specified <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="HeapSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list, IComparer<T> comparer)
    {
        var heap = new BinaryHeapListWrapper<T>(list, comparer, list.Count);
        var bufferLastAvailableIndex = heap.ListCount - 1;
        while (heap.HeapCount > 0)
        {
            var maxItem = heap.Pop();
            heap[bufferLastAvailableIndex] = maxItem;
            bufferLastAvailableIndex--;
        }
    }
}
