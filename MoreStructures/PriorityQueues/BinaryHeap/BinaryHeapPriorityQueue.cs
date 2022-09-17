using System.Collections;
using System.Diagnostics.CodeAnalysis;
using MoreStructures.PriorityQueues.ArrayList;

namespace MoreStructures.PriorityQueues.BinaryHeap;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on a Binary Max Heap of its items. On top of basic 
/// operations it also supports <see cref="IPeekKthPriorityQueue{T}"/> and 
/// <see cref="IMergeablePriorityQueue{T, TPQTarget}"/>.
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
///       <see cref="ArrayListPriorityQueue{T}"/> and becomes a logarithmic-time operation in this implementation.
///       <br/>
///     - So this implementation can be considered as a balanced compromise between insertion and extraction, which 
///       complexifies the underlying data structure and loses some performance in insertion to obtain all-around
///       logarithmic performance.
///       <br/>
///     - Given the "exponentially better" runtime of logarithmic operations w.r.t. linear ones, such compromise makes
///       sense for most scenarios.
///       <br/>
///     - Merging two Binary Max Heap structures, however, still requires linear time. If merging performance is
///       critical, a more advanced tree-based implementation, such as 
///       <see cref="BinomialHeap.BinomialHeapPriorityQueue{T}"/>, 
///       <see cref="FibonacciHeap.FibonacciHeapPriorityQueue{T}"/> and their derivations, should be used instead.
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
public class BinaryHeapPriorityQueue<T> 
    : IPeekKthPriorityQueue<T>, IMergeablePriorityQueue<T, BinaryHeapPriorityQueue<T>>
    where T : notnull
{
    /// <summary>
    /// A non-negative, zero-based, monotonically strictly increasing counter, incremented at every insertion into this 
    /// data structure by a <see cref="Push(T, int)"/>.
    /// </summary>
    protected int CurrentPushTimestamp { get; set; } = 0;

    /// <summary>
    /// The wrapped <see cref="IList{T}"/> of <see cref="PrioritizedItem{T}"/> backing the binary max heap.
    /// </summary>
    protected BinaryHeapListWrapper<PrioritizedItem<T>> Items { get; }

    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    /// <remarks>
    /// The underlying data structure for priorities and items is initialized to an empty structure.
    /// <br/>
    /// Therefore, Time and Space Complexity is O(1).
    /// </remarks>
    public BinaryHeapPriorityQueue()
    {
        Items = 
            new(new List<PrioritizedItem<T>>(), Comparer<PrioritizedItem<T>>.Default, 0)
            {
                RaiseItemPushed = RaiseItemPushed,
                RaiseItemPopping = RaiseItemPopping,
                RaiseItemsSwapped = RaiseItemsSwapped,
            };
    }

    /// <summary>
    /// Builds a new priority queue with the same items of the provided <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The <see cref="BinaryHeapPriorityQueue{T}"/> instance to use as a source of data.</param>
    /// <remarks>
    /// The underlying data structure is shallow copied.
    /// <br/>
    /// Because it is made of immutable records, a shallow-copy is enough to ensure that its mutation in 
    /// <paramref name="source"/> won't affect the new priority queue or viceversa.
    /// <br/>
    /// Because the data structure contain O(n) items, Time and Space Complexity are O(n), where n is the number of
    /// items in <paramref name="source"/>.
    /// </remarks>
    protected BinaryHeapPriorityQueue(BinaryHeapPriorityQueue<T> source)
    {
        Items = 
            new(source.Items)
            {
                RaiseItemPushed = RaiseItemPushed,
                RaiseItemPopping = RaiseItemPopping,
                RaiseItemsSwapped = RaiseItemsSwapped,
            };
    }

    #region Public API

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks the count of the underlying heap.
    /// <br/>
    /// The size of the heap may be smaller than the size of the underlying list, if there is buffer at the end.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public virtual int Count => Items.HeapCount;

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
    public virtual IEnumerator<T> GetEnumerator()
    {
        var copy = new BinaryHeapPriorityQueue<T>(this);
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
    public virtual PrioritizedItem<T> Peek() => Items.Peek();

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
    public virtual PrioritizedItem<T> Pop() => Items.Pop();

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
    public virtual void Push(T item, int priority) => Items.Push(new(item, priority, CurrentPushTimestamp++));

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
    ///     - A dedicated <see cref="BinaryHeapPriorityQueue{T}"/> C of <see cref="int"/> values is instantiated.
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
    ///       run, since they are done on a <see cref="BinaryHeapPriorityQueue{T}"/> instance.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(k * log(k)) and Space Complexity is O(k).
    ///     </para>
    /// </remarks>
    public virtual PrioritizedItem<T>? PeekKth(int k) => Items.PeekKth(k) is (var result, true) ? result : null;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Pushing all items in the <paramref name="targetPriorityQueue"/> via <see cref="Push(T, int)"/>, would 
    ///       result in O(m * log(m)) Time Complexity, where m is the number of items in the
    ///       <paramref name="targetPriorityQueue"/>.
    ///       <br/>
    ///     - Instead, each of the m items from the <paramref name="targetPriorityQueue"/> is added to the underlying 
    ///       array list of this queue, at the end.
    ///       <br/>
    ///     - Then, the content of <paramref name="targetPriorityQueue"/> is cleared, to respect the contract defined 
    ///       by <see cref="IMergeablePriorityQueue{T, TPQTarget}"/>.
    ///       <br/>
    ///     - Finally, the heap property is restored globally for all items in the underlying array list, by sifting
    ///       down all items in the first half of the list, proceeding backwards from the middle of the list to its
    ///       first item.
    ///       <br/>
    ///     - Such global sift down is required for the first half of the items only, because the second half only 
    ///       contains leaves of the tree, for which a sift down would do nothing (i.e. the heap property is already 
    ///       satisfied).
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Appending m items has a linear cost over m.
    ///       <br/>
    ///     - Clearing the target only takes constant time.
    ///       <br/>
    ///     - Restoring the heap property globally would seem to take n / 2 * log(n), where n is the total number of 
    ///       items in this queue, after the merge: the number of items to sift down plus the cost of sifting down the 
    ///       tree. That would give O(n * log(n)) complexity: not a real improvement over the naive approach of pushing
    ///       n items.
    ///       <br/>
    ///     - However, the length of the path to sift down is not as big as the entire height of the tree. Instead, the
    ///       closer the starting node is to the leave, the smaller it becomes: leaves have sift down paths of length
    ///       0, their parent of length 1, etc., up to the root, which has sift down path of length equal to the height
    ///       of the tree.
    ///       <br/>
    ///     - A key observation is that in a complete and full tree there are more leaves than all nodes in other 
    ///       levels combined, and that applies to all levels w.r.t. all smaller levels.
    ///       <br/>
    ///     - So, sift down will cost less for way more nodes, resulting in overall O(n) Time Complexity.
    ///       <br/>
    ///     - Space Complexity is O(m), since m items are added to the list storing the items of this queue.
    ///     </para>
    /// </remarks>
    public virtual void MergeFrom(BinaryHeapPriorityQueue<T> targetPriorityQueue)
    {
        foreach (var prioritizedItem in targetPriorityQueue.Items)
        {
            Items.Push(new(prioritizedItem.Item, prioritizedItem.Priority, CurrentPushTimestamp), false);
            CurrentPushTimestamp++;
        }

        targetPriorityQueue.Clear();

        Items.RestoreHeapProperty();
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Just clears the underlying array list.
    /// <br/>
    /// The internal push timestamp counter is not reset, nor the era. Therefore, new pushes after the clear will 
    /// receive push timestamps strictly higher than the ones assigned to the items in the queue before the clear.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public virtual void Clear() => Items.Clear();

    #endregion

    #region Hooks

    /// <summary>
    /// Invoked just after an item has been pushed into <see cref="Items"/>.
    /// </summary>
    /// <param name="indexPushed">
    /// The index of the item being pushed. 
    /// <br/>
    /// When the heap is at the beginning of the list (which is the layout used by this queue), it is equal to 
    /// Count - 1 if the underlying list is fully utilized, and it's smaller than that otherwise (there is buffer at 
    /// the end of the list, after the last item of the heap).
    /// </param>
    protected virtual void RaiseItemPushed(int indexPushed) { }

    /// <summary>
    /// Invoked just before an item is removed from <see cref="Items"/>.
    /// </summary>
    /// <param name="indexPopped">
    /// The index of the item being popped.
    /// <br/>
    /// When the heap is at the beginning of the list (which is the layout used by this queue), it is equal to 0.
    /// </param>
    /// <param name="indexInBufferArea">
    /// The index of the item of the last leaf, which is going to be swapped with the root during pop.
    /// </param>
    protected virtual void RaiseItemPopping(int indexPopped, int indexInBufferArea) { }

    /// <summary>
    /// Invoked just after two items have been swapped of position in <see cref="Items"/>.
    /// </summary>
    /// <param name="index1">The index of the first item swapped.</param>
    /// <param name="index2">The index of the second item swapped.</param>
    protected virtual void RaiseItemsSwapped(int index1, int index2) { }

    #endregion
}
