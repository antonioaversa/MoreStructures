using System.Collections;

namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on a Binary Max Heap of its items. On top of basic 
/// operations it also supports <see cref="IPeekKthPriorityQueue{T}"/>.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IPriorityQueue{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - The Binary Max Heap is used to store items with their priorities, in a way that the item with max priority is
///       immediately retrievable (making <see cref="Peek"/> a constant-time operation), and easily extractable (making 
///       <see cref="Pop"/> a logarithmic-time operation).
///       <br/>
///     - This comes a cost of the <see cref="Push(T, int)"/> operation, which is O(1) in 
///       <see cref="ArrayListBasedPriorityQueue{T}"/> and becomes a logarithmic-time operation in this implementation.
///       <br/>
///     - So this implementation can be considered as a balanced compromise between insertion and extraction, which 
///       complexifies the underlying data structure and loses some performance in insertion to obtain all-around
///       logarithmic performance.
///       <br/>
///     - Given the "exponentially better" runtime of logarithmic operations w.r.t. linear ones, such compromise makes
///       sense for most scenarios.
///     </para>
///     <para id="heap-representation">
///     BINARY MAX HEAP REPRESENTATION
///     <br/>
///     - The Binary Max Heap used for items and priorities is backed by a complete Binary Tree, represented as an 
///       Array List of its items. 
///       <br/>
///     - The root of the tree is always in position 0, its children in positions 1 and 2, grand-children in positions
///       3, 4, 5 and 6 (where 3 and 4 are children of 1 and 5 and 6 are children of 2), etc.
///       <br/>
///     - This is the most space-efficient layout for a complete tree, which allows O(1) root access, parent-to-child 
///       navigation and child-to-parent navigation, by performing simple indexing arithmetic.
///       <br/>
///     - The underlying Binary Tree is complete, hence balanced: it's height h is Math.Floor(log(n, 2)), where n is 
///       the number of nodes in the tree. For example a complete Binary Tree of 3 nodes has necessarily height 1,
///       whereas one of 4 nodes has to have height 2.
///       <br/>
///     - While the Binary Tree is complete, it is non necessarily full, meaning that the last level may not be 
///       entirely filled with leaves and the number of leaves at the last level may vary from 1 up to 2^(h + 1) - 1.
///       <br/>
///     - All modification operations (such as <see cref="Pop"/>) done on the Binary Max Heap ensure that the tree is 
///       kept complete and balanced.
///     </para>
///     <para id="repetitions">
///     REPEATED ITEMS AND PRIORITIES
///     <br/>
///     - Repeated items, as well as repeated priorities, are supported.
///       <br/>
///     - The implementation is <b>stable</b> both for priorities and items.
///       <br/>
///     - If two items I1 and I2 are pushed with the same priority P at times T1 and T2 with T1 &lt; T2, when P becomes
///       the highest priority in the heap, I1 is popped out of the heap before I2 is.
///       <br/>
///     - That also applies to the case where I1 and I2 are equal by value and different by reference.
///       <br/>
///     - Stability is achieved by keeping a "push index", i.e. a <see cref="int"/> counter set to 0 in the constructor
///       and incremented every time a new item is introduced in the queue via a <see cref="Push(T, int)"/>.
///       <br/>
///     - The push index is included in the heap item record, together with the item of type 
///       <typeparamref name="T"/> and its priority of type <see cref="int"/>.
///       <br/>
///     - This way two heap item records I1 and I2 with the same priority I1.Priority and I2.Priority, and potentially
///       the same or equal items I1.Item and I2.Item, will necessarily differ by push index, I1.PushTimestamp and 
///       I2.PushTimestamp.
///       <br/>
///      - Therefore a <b>total strict order</b> can be imposed.
///     </para>
/// </remarks>
public class HeapBasedPriorityQueue<T> : IPeekKthPriorityQueue<T>
    where T : notnull
{
    /// <summary>
    /// A non-negative, zero-based, monotonically strictly increasing counter, incremented at every insertion into this 
    /// data structure by a <see cref="Push(T, int)"/>.
    /// </summary>
    protected int _currentPushTimestamp = 0;

    /// <summary>
    /// The <see cref="List{T}"/> of <see cref="PrioritizedItem{T}"/> backing the binary max heap.
    /// </summary>
    protected List<PrioritizedItem<T>> Items { get; }


    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    /// <remarks>
    /// The underlying data structure for priorities and items is initialized to an empty structure.
    /// <br/>
    /// Therefore, Time and Space Complexity is O(1).
    /// </remarks>
    public HeapBasedPriorityQueue()
    {
        Items = new();
    }

    /// <summary>
    /// Builds a new priority queue with the same items of the provided <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The <see cref="HeapBasedPriorityQueue{T}"/> instance to use as a source of data.</param>
    /// <remarks>
    /// The underlying data structure is shallow copied.
    /// <br/>
    /// Because it is made of immutable records, a shallow-copy is enough to ensure that its mutation in 
    /// <paramref name="source"/> won't affect the new priority queue or viceversa.
    /// <br/>
    /// Because the data structure contain O(n) items, Time and Space Complexity are O(n), where n is the number of
    /// items in <paramref name="source"/>.
    /// </remarks>
    public HeapBasedPriorityQueue(HeapBasedPriorityQueue<T> source)
    {
        Items = new(source.Items);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks the count of the underlying array list.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count => Items.Count;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// In order to return items in heap order (i.e. the max at each step), it first copies this priority queue into a
    /// second temporary queue, which can be mutated without affecting the state of this queue.
    /// <br/>
    /// It then iterates over the copy, calling <see cref="Pop"/> until it becomes empty.
    /// <br/>
    /// Time Complexity is O(n * log(n)) (when fully enumerated), because a single <see cref="Pop"/> on an complete 
    /// binary heap takes logarithmic time, and there are n items to be extracted.
    /// <br/>
    /// Space Complexity is O(n), as a copy of this queue is required as auxiliary data structure to emit elements in 
    /// the right order of priority.
    /// </remarks> 
    public IEnumerator<T> GetEnumerator()
    {
        var copy = new HeapBasedPriorityQueue<T>(this);
        while (copy.Count > 0)
            yield return copy.Pop().Item;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="GetEnumerator"/>
    /// </remarks>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Peeks the item with max priority from the root of the heap, if any.
    /// <br/>
    /// That is located at index 0 in the underlying list, and can be accessed in constant-time.
    /// <br/>
    /// Therefore, Time and Space Complexity are O(1).
    /// </remarks>
    public PrioritizedItem<T> Peek()
    {
        if (Items.Count == 0)
            throw new InvalidOperationException($"Can't {nameof(Peek)} on an empty queue.");
        return Items[0];
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Peeks the first item from the heap and stores to return it as result.
    ///       <br/>
    ///     - Takes the last item of the heap and moves to the root, in first position.
    ///       <br/>
    ///     - Restores heap constraints by recursively sifting down new root, as many time as needed.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - <see cref="Peek"/> and removal of item with max priority and last leaf of the heap are all constant-time
    ///       operations.
    ///       <br/>
    ///     - Since the heap is an complete binary tree, the heigh of the three is O(log(n)).
    ///       <br/>
    ///     - So the number of "sift down" operations is logarithmic w.r.t. the input.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n)) and Space Complexity is O(1), since modifications are all done 
    ///       in-place in underlying data structures.
    ///     </para>
    /// </remarks>
    public PrioritizedItem<T> Pop()
    {
        if (Items.Count == 0)
            throw new InvalidOperationException($"Can't {nameof(Pop)} on an empty queue.");

        RaiseItemPopping();
        var result = Peek();

        var lastIndex = Items.Count - 1;
        Items[0] = Items[lastIndex];
        Items.RemoveAt(lastIndex);

        if (Items.Count > 0)
            SiftDown(0);
        return result;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     Negative priorities are supported.
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Adds a new leaf to the heap, carrying priority, item and unique push index.
    ///       <br/>
    ///     - Restores heap constraints by recursively sifting up new leaf, as many time as needed.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Adding a new item with given priority and a leaf to the heap are constant-time operations.
    ///       <br/>
    ///     - Since the heap is an complete binary tree, the heigh of the three is O(log(n)).
    ///       <br/>
    ///     - So the number of "sift up" operations is logarithmic w.r.t. the input.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n)) and Space Complexity is O(1), since modifications are all done 
    ///       in-place in underlying data structure.
    ///     </para>
    /// </remarks>
    public void Push(T item, int priority)
    {
        Push(item, priority, _currentPushTimestamp);
        _currentPushTimestamp++;
    }

    private void Push(T item, int priority, int pushTimestamp)
    {
        Items.Add(new(item, priority, pushTimestamp));
        RaiseItemPushed();
        SiftUp(Items.Count - 1);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - If <paramref name="k"/> is negative, an <see cref="ArgumentException"/> is returned.
    ///       <br/>
    ///     - If <paramref name="k"/> is non-smaller than the <see cref="Count"/>, <see langword="null"/> is returned.
    ///       <br/>
    ///     - If <paramref name="k"/> is 0, <see cref="Peek"/> is returned.
    ///       <br/>
    ///     - Otherwise, the main algorithm loop is performed, at most <paramref name="k"/> times.
    ///       <br/>
    ///     - A dedicated <see cref="HeapBasedPriorityQueue{T}"/> C of <see cref="int"/> values is instantiated.
    ///       <br/>
    ///     - The values of C are the indexes of the underlying list of this priority queue, and identify candidates
    ///       for the <paramref name="k"/>-th largest item.
    ///       <br/>
    ///     - Such candidates are sorted in C by priority and push timestamps, exactly in the same way they are
    ///       sorted in this priority queue.
    ///       <br/>
    ///     - At the beginning only the root of the priority queue (i.e. index 0) is pushed to C.
    ///       <br/>
    ///     - At each iteration the max of C is popped from C and its left and right children (if any) are pushed into
    ///       C.
    ///       <br/>
    ///     - After <paramref name="k"/> iterations, the <see cref="Peek"/> of C gives the <paramref name="k"/>-th 
    ///       largest item.
    ///       <br/>
    ///     - Notice that C cannot run short of candidates (due to lack of children), because of the preconditions
    ///       on <paramref name="k"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Checks on the value of <paramref name="k"/> w.r.t. the size of this priority queue and direct access to
    ///       the underlying list, to return the final result once the index has been found, are both done in constant 
    ///       time.
    ///       <br/>
    ///     - Candidates queue instantiation and 1st push into it are also constant time operations.
    ///       <br/>
    ///     - The main loop consist of k iterations.
    ///       <br/>
    ///     - At each iteration 1 item is popped and 2 are pushed, so the candidates queue grows of 1 item per cycle.
    ///       <br/>
    ///     - Each <see cref="Pop"/> and <see cref="Push(T, int)"/> operation on the candidates queue has logarithmic
    ///       run, since they are done on a <see cref="HeapBasedPriorityQueue{T}"/> instance.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(k * log(k)) and Space Complexity is O(k).
    ///     </para>
    /// </remarks>
    public PrioritizedItem<T>? PeekKth(int k)
    {
        if (k < 0) throw new ArgumentException("Must be non-negative.", nameof(k));
        if (k >= Items.Count) return null;
        if (k == 0) return Peek();

        var candidates = new HeapBasedPriorityQueue<int>();
        candidates.Push(0, Items[0].Priority, Items[0].PushTimestamp);
        while (k > 0)
        {
            var maxIndex = candidates.Pop();

            var leftChildIndex = LeftChildOf(maxIndex.Item);
            if (leftChildIndex >= 0)
                candidates.Push(
                    leftChildIndex, Items[leftChildIndex].Priority, Items[leftChildIndex].PushTimestamp);

            var rightChildIndex = RightChildOf(maxIndex.Item);
            if (rightChildIndex >= 0)
                candidates.Push(
                    rightChildIndex, Items[rightChildIndex].Priority, Items[rightChildIndex].PushTimestamp);

            k--;
        }

        return Items[candidates.Peek().Item];
    }

    /// <summary>
    /// Invoked just after an item has been pushed into <see cref="Items"/> (at the end of it), and before the 
    /// "sifting up" procedure is performed.
    /// </summary>
    protected virtual void RaiseItemPushed() { }

    /// <summary>
    /// Invoked just before an item is removed from <see cref="Items"/> (at the beginning of it), and before 
    /// "sifting down" procedure is performed.
    /// </summary>
    protected virtual void RaiseItemPopping() { }

    /// <summary>
    /// Invoked just after two items have been swapped of position in <see cref="Items"/>.
    /// </summary>
    /// <param name="index1">The index of the first item swapped.</param>
    /// <param name="index2">The index of the second item swapped.</param>
    protected virtual void RaiseItemsSwapped(int index1, int index2) { }

    /// <summary>
    /// Restores the heap constraint on the item at the specified <paramref name="nodeIndex"/> w.r.t. its ancestors in
    /// the tree.
    /// </summary>
    /// <param name="nodeIndex">The index of the item to check.</param>
    protected virtual void SiftUp(int nodeIndex)
    {
        var parentIndex = ParentOf(nodeIndex);

        // If the node doesn't have a parent, it means we reached the root of the tree, so there is nothing to sift up.
        if (parentIndex < 0)
            return;

        var parentValue = Items[parentIndex];
        var nodeValue = Items[nodeIndex];
        if (parentValue.CompareTo(nodeValue) < 0)
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
    protected virtual void SiftDown(int nodeIndex)
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
        // - left > node and no right or left > right => left becomes the new parent of node and right
        // - right > node and right > left => right becomes the new parent of left and node
        // Notice that TreeItem.CompareTo never gives 0 due to always increasing PushTimestamp at each Push
        var nodeValue = Items[nodeIndex];
        if (leftChildValue.CompareTo(nodeValue) > 0 &&
            (rightChildIndex < 0 || leftChildValue.CompareTo(Items[rightChildIndex]) > 0))
        {
            Items[nodeIndex] = leftChildValue;
            Items[leftChildIndex] = nodeValue;
            RaiseItemsSwapped(nodeIndex, leftChildIndex);

            SiftDown(leftChildIndex);
        }
        else if (rightChildIndex >= 0 &&
            Items[rightChildIndex].CompareTo(nodeValue) > 0 && Items[rightChildIndex].CompareTo(leftChildValue) > 0)
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
        2 * nodeIndex + 1 is var result && result < Items.Count ? result : -1;
    private int RightChildOf(int nodeIndex) => 
        2 * nodeIndex + 2 is var result && result < Items.Count ? result : -1;
}
