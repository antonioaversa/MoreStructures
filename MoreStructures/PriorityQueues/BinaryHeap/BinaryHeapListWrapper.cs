namespace MoreStructures.PriorityQueues.BinaryHeap;

/// <summary>
/// A wrapper around a <see cref="IList{T}"/>, which preserve the max heap property on the subset of items of the list 
/// from index 0 to its count - 1, keeping a buffer of listCount - <see cref="HeapCount"/> items.
/// </summary>
/// <typeparam name="T">The type of items in the wrapped list.</typeparam>
/// <remarks>
/// Can be used to support HeapSort or a priority queue based on a Max Binary Heap.
/// <br/>
/// In the first case the buffer are is used. In the second case it is not.
/// </remarks>
public sealed class BinaryHeapListWrapper<T>
{
    private IList<T> Items { get; }
    private IComparer<T> Comparer { get; }
    
    /// <summary>
    /// The number of items in the heap only, buffer area of the underlying list excluded.
    /// </summary>
    /// <remarks>
    /// May be smaller than the current <see cref="ICollection{T}.Count"/> of the underlying <see cref="IList{T}"/>, 
    /// which in turn may be smaller than the current Length of the underlying <see cref="Array"/>.
    /// </remarks>
    public int HeapCount { get; private set; }

    /// <summary>
    /// Invoked just after an item has been pushed into <see cref="Items"/> (at the end of it), and before the 
    /// "sifting up" procedure is performed.
    /// </summary>
    public Action<int> RaiseItemPushed { get; init; } = i => { };

    /// <summary>
    /// Invoked just before an item is removed from <see cref="Items"/> (at the beginning of it), and before 
    /// "sifting down" procedure is performed.
    /// </summary>
    public Action RaiseItemPopping { get; init; } = () => { };

    /// <summary>
    /// Invoked just after two items have been swapped of position in <see cref="Items"/>.
    /// </summary>
    public Action<int, int> RaiseItemsSwapped { get; init; } = (i1, i2) => { };


    /// <summary>
    ///     <inheritdoc cref="BinaryHeapListWrapper{T}"/>
    ///     <br/>
    ///     Built around the provided <see cref="IList{T}"/> <paramref name="items"/>.
    /// </summary>
    /// <param name="items">The <see cref="IList{T}"/> of items to be wrapped.</param>
    /// <param name="comparer">The comparer to be used to establish a order relationship between items.</param>
    /// <param name="count">
    /// The size of the subset of <paramref name="items"/> to be kept in order according to the max heap property.
    /// <br/>
    /// Goes from index 0 to index <paramref name="count"/> - 1.
    /// <br/>
    /// Current size must be non-bigger than the <see cref="ICollection{T}.Count"/>.
    /// </param>
    public BinaryHeapListWrapper(IList<T> items, IComparer<T> comparer, int count)
    {
        if (count < 0 || count > items.Count)
            throw new ArgumentException(
                $"Must be non-negative and at most the size of {nameof(items)}", nameof(items));

        Items = items;
        Comparer = comparer;
        HeapCount = count;

        RestoreHeapProperty();
    }
    
    /// <summary>
    ///     <inheritdoc cref="BinaryHeapListWrapper{T}"/>
    ///     <br/>
    ///     Built from the provided <see cref="BinaryHeapListWrapper{T}"/> <paramref name="source"/>.
    /// </summary>
    public BinaryHeapListWrapper(BinaryHeapListWrapper<T> source)
    {
        Items = new List<T>(source.Items);
        Comparer = source.Comparer;
        HeapCount = source.HeapCount;

        RestoreHeapProperty();
    }


    /// <summary>
    /// Restores the max heap property, ensuring that each node of the heap is at least as big as its children, if any.
    /// </summary>
    public void RestoreHeapProperty()
    {
        for (var i = HeapCount / 2 + 1; i >= 0; i--)
            SiftDown(i);
    }

    /// <summary>
    /// Peeks the item with max priority from the root of the heap, if any.
    /// </summary>
    public T Peek()
    {
        if (HeapCount == 0)
            throw new InvalidOperationException($"Can't {nameof(Peek)} on an empty heap.");
        return Items[0];
    }

    /// <summary>
    /// Pops the item with max priority from the root of the heap, if any, moving the last leaf to the now vacant root
    /// and sifting it down until the heap property is restored.
    /// </summary>
    /// <returns>The popped item.</returns>
    public T Pop()
    {
        if (HeapCount == 0)
            throw new InvalidOperationException($"Can't {nameof(Pop)} on an empty heap.");

        RaiseItemPopping();
        var result = Peek();

        var lastHeapIndex = HeapCount - 1;
        (Items[0], Items[lastHeapIndex]) = (Items[lastHeapIndex], Items[0]);
        HeapCount--;

        if (HeapCount > 0)
            SiftDown(0);
        return result;
    }

    /// <summary>
    /// Pushes the provided <paramref name="item"/> into the heap, sifting it up until the max heap property is 
    /// restored.
    /// </summary>
    /// <param name="item">The item to be added to the heap.</param>
    /// <param name="siftUp">
    ///     Whether the sift up procedure should be executed or not. By default it is set to <see langword="true"/>.
    ///     <br/>
    ///     If it is not executed, the max heap property will be temporary violated.
    /// </param>
    public void Push(T item, bool siftUp = true)
    {
        int index;
        if (HeapCount < Items.Count)
        {
            Items[HeapCount] = item;
            index = HeapCount;
        }
        else
        {
            Items.Add(item);
            index = Items.Count - 1;
        }
        HeapCount++;
        RaiseItemPushed(index);

        if (siftUp)
            SiftUp(index);
    }

    /// <summary>
    /// Retrieves the item of the heap with priority <paramref name="k"/>, without extracting any of the items in the 
    /// heap.
    /// </summary>
    /// <param name="k">The non-negative priority rank: 0 means highest priority, 1 second highest, etc.</param>
    /// <returns>
    /// The item with k-th highest priority if any, with <see langword="true"/> as valid flag.
    /// <br/>
    /// The default for <typeparamref name="T"/> otherwise, with <see langword="false"/> as valid flag.
    /// </returns>
    public (T? result, bool valid) PeekKth(int k)
    {
        if (k < 0) throw new ArgumentException("Must be non-negative.", nameof(k));
        if (k >= HeapCount) return (default, false);
        if (k == 0) return (Peek(), true);

        var candidates = new BinaryHeapListWrapper<(int, T)>(new List<(int, T)>(), new Item2Comparer(Comparer), 0);
        candidates.Push((0, Items[0]));
        while (k > 0)
        {
            var (maxIndex, _) = candidates.Pop();

            var leftChildIndex = LeftChildOf(maxIndex);
            if (leftChildIndex >= 0)
                candidates.Push((leftChildIndex, Items[leftChildIndex]));

            var rightChildIndex = RightChildOf(maxIndex);
            if (rightChildIndex >= 0)
                candidates.Push((rightChildIndex, Items[rightChildIndex]));

            k--;
        }

        return (Items[candidates.Peek().Item1], true);
    }

    /// <summary>
    /// Pops all items of the heap in sequence, from the one with highest priority to the one with lowest priority.
    /// </summary>
    /// <returns>A sequence of <typeparamref name="T"/> instances.</returns>
    public IEnumerable<T> PopAll()
    {
        while (HeapCount > 0)
            yield return Pop();
    }

    private sealed record Item2Comparer(IComparer<T> Comparer) : IComparer<(int, T)>
    {
        public int Compare((int, T) x, (int, T) y) => Comparer.Compare(x.Item2, y.Item2);
    }

    /// <summary>
    /// Restores the heap constraint on the item at the specified <paramref name="nodeIndex"/> w.r.t. its ancestors in
    /// the tree.
    /// </summary>
    /// <param name="nodeIndex">The index of the item to check.</param>
    internal void SiftUp(int nodeIndex)
    {
        var parentIndex = ParentOf(nodeIndex);

        // If the node doesn't have a parent, it means we reached the root of the tree, so there is nothing to sift up.
        if (parentIndex < 0)
            return;

        var parentValue = Items[parentIndex];
        var nodeValue = Items[nodeIndex];
        if (Comparer.Compare(parentValue, nodeValue) < 0)
        {
            Items[parentIndex] = nodeValue;
            Items[nodeIndex] = parentValue;
            RaiseItemsSwapped(parentIndex, nodeIndex);

            SiftUp(parentIndex);
        }
    }

    /// <summary>
    /// Restores the heap constraint on the item at the specified <paramref name="nodeIndex"/> w.r.t. its descendants 
    /// in the tree.
    /// </summary>
    /// <param name="nodeIndex">The index of the item to check.</param>
    private void SiftDown(int nodeIndex)
    {
        var leftChildIndex = LeftChildOf(nodeIndex);

        // If the node doesn't have a left child, it definitely has no right child, since the tree is complete.
        // Therefore the node is a leaf and there is nothing to sift down.
        if (leftChildIndex < 0)
            return;

        var leftChildValue = Items[leftChildIndex];

        var rightChildIndex = RightChildOf(nodeIndex);

        // Cases where heap property is respected: node > left > right, node > right > left
        // Cases where heap property has to be restored:
        // - left > node and no right or left >= right => left becomes the new parent of node and right
        // - right > node and right > left => right becomes the new parent of left and node
        var nodeValue = Items[nodeIndex];
        if (Comparer.Compare(leftChildValue, nodeValue) > 0 &&
            (rightChildIndex < 0 || Comparer.Compare(leftChildValue, Items[rightChildIndex]) >= 0))
        {
            Items[nodeIndex] = leftChildValue;
            Items[leftChildIndex] = nodeValue;
            RaiseItemsSwapped(nodeIndex, leftChildIndex);

            SiftDown(leftChildIndex);
        }
        else if (rightChildIndex >= 0 &&
            Comparer.Compare(Items[rightChildIndex], nodeValue) > 0 && 
            Comparer.Compare(Items[rightChildIndex], leftChildValue) > 0)
        {
            Items[nodeIndex] = Items[rightChildIndex];
            Items[rightChildIndex] = nodeValue;
            RaiseItemsSwapped(nodeIndex, rightChildIndex);

            SiftDown(rightChildIndex);
        }
    }

    private static int ParentOf(int nodeIndex) =>
        nodeIndex == 0 ? -1 : (nodeIndex - 1) / 2;
    private int LeftChildOf(int nodeIndex) =>
        2 * nodeIndex + 1 is var result && result < HeapCount ? result : -1;
    private int RightChildOf(int nodeIndex) =>
        2 * nodeIndex + 2 is var result && result < HeapCount ? result : -1;

    #region Access to Underlying List<T>

    /// <summary>
    /// The number of items in the underlying list, heap and buffer are included.
    /// </summary>
    /// <remarks>
    /// It's always non-smaller than <see cref="HeapCount"/>, since all the heap items are contained in the underlying
    /// list.
    /// </remarks>
    public int ListCount => Items.Count;

    /// <summary>
    /// Retrieves the <paramref name="index"/>-th item in the underlying list (heap or buffer).
    /// </summary>
    /// <param name="index">
    /// A non-negative <see cref="int"/>. It can be non-smaller than <see cref="HeapCount"/>, if an element of the 
    /// buffer is being accessed, but it necessarily has to be smaller than the <see cref="ICollection{T}.Count"/> of 
    /// the underlying <see cref="IList{T}"/>.
    /// </param>
    /// <returns>The item, an instance of type <typeparamref name="T"/>.</returns>
    public T this[int index] 
    { 
        get => Items[index]; 
        set => Items[index] = value; 
    }

    /// <summary>
    /// Clears the underlying list (heap and buffer), wiping all its items out, if the list is not readonly.
    /// <br/>
    /// If it is readonly (e.g. an array), it just resets the <see cref="HeapCount"/>.
    /// </summary>
    public void Clear()
    {
        if (!Items.IsReadOnly)
            Items.Clear();
        HeapCount = 0;
    }

    /// <summary>
    /// Returns an enumerator of <typeparamref name="T"/> instances, going through all items of the underlying list,
    /// not just the first <see cref="HeapCount"/> items which form the heap, but also the buffer area at the end of the
    /// underlying list.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <typeparamref name="T"/>.</returns>
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    #endregion
}
