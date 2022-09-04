using System.Collections;

namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on a Binary Max Heap of its items.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IPriorityQueue{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - The Binary Max Heap is used to store the priorities, in a way that the max priority is immediately 
///       retrievable (making <see cref="Peek"/> a constant-time operation), and easily extractable (making 
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
///     - The Binary Max Heap used for priorities is backed by a complete Binary Tree, represented as and Array List 
///       of its items. 
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
///     </para>
/// </remarks>
public class HeapBasedPriorityQueue<T> : IPriorityQueue<T>
    where T : notnull
{
    private record struct TreeItem(int Priority);

    private List<TreeItem> Priorities { get; }
    private Dictionary<int, LinkedList<T>> ItemsByPriority { get; }


    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    /// <remarks>
    /// The underlying data structures for priorities and items are initialized to empty structures.
    /// <br/>
    /// Therefore, Time and Space Complexity is O(1).
    /// </remarks>
    public HeapBasedPriorityQueue()
    {
        Priorities = new();
        ItemsByPriority = new();
    }

    /// <summary>
    /// Builds a new priority queue with the same items of the provided <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The <see cref="HeapBasedPriorityQueue{T}"/> instance to use as a source of data.</param>
    /// <remarks>
    /// The underlying data structure used for priorities is shallow copied.
    /// <br/>
    /// Because it is made of immutable records, a shallow-copy is enough to ensure that its mutation in 
    /// <paramref name="source"/> won't affect the new priority queue or viceversa.
    /// <br/>
    /// The underlying data structure used for items is deep-copied instead, because it contains linked lists which
    /// would be mutated, if either of the priority queues is changed.
    /// <br/>
    /// Because both data structures contain O(n) items, Time and Space Complexity are O(n), where n is the number of
    /// items in <paramref name="source"/>.
    /// </remarks>
    public HeapBasedPriorityQueue(HeapBasedPriorityQueue<T> source)
    {
        Priorities = new(source.Priorities);
        ItemsByPriority = new(
            source.ItemsByPriority.Select(kvp => KeyValuePair.Create(kvp.Key, new LinkedList<T>(kvp.Value))));
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks the count of the underlying array list.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count => Priorities.Count;

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
    /// Peeks the max priority from the root of the priority heap.
    /// <br/>
    /// Then retrieves the 1st item in the linked list of items with such priority, from the dictionary of items.
    /// <br/>
    /// Accessing the root of the heap, retrieving the linked list of items with a given priority and accessing the
    /// first item of a linked list are all constant-time operations, which only require constant space.
    /// <br/>
    /// Therefore, Time and Space Complexity are O(1).
    /// </remarks>
    public ItemAndPriority<T> Peek()
    {
        if (Priorities.Count == 0)
            throw new InvalidOperationException($"Can't ${nameof(Peek)} on an empty queue.");
        var maxPriority = Priorities[0].Priority;
        return new(ItemsByPriority[maxPriority].First!.Value, maxPriority);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Peeks the first item from the queue and stores to return it as result.
    ///       <br/>
    ///     - Removes the first item from the linked list of items with max priority, as it is being popped out.
    ///       <br/>
    ///     - Removes the first priority from the linked list of priorities for the item, for the same reason.
    ///       <br/>
    ///     - Takes the last priority of the heap and moves to the root, in first position.
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
    public ItemAndPriority<T> Pop()
    {
        if (Priorities.Count == 0)
            throw new InvalidOperationException($"Can't ${nameof(Pop)} on an empty queue.");
        var maxPriority = Priorities[0].Priority;
        var result = Peek();
        ItemsByPriority[maxPriority].RemoveFirst();

        var lastLeaf = Priorities[^1];
        Priorities[0] = new TreeItem(lastLeaf.Priority);
        Priorities.RemoveAt(Priorities.Count - 1);

        SiftDown(0);
        return result;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     Negative priorities are supported.
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Adds a new leaf to the priority heap and a new item to the linked list of items with the given priority,
    ///       creating the entry and the linked list if it doesn't exist.
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
    ///       in-place in underlying data structures.
    ///     </para>
    /// </remarks>
    public void Push(T item, int priority)
    {
        Priorities.Add(new(priority));
        if (!ItemsByPriority.ContainsKey(priority))
            ItemsByPriority[priority] = new();
        ItemsByPriority[priority].AddLast(item);

        var newLeafIndex = Priorities.Count - 1;
        SiftUp(newLeafIndex);
    }

    private void SiftUp(int nodeIndex)
    {
        var parentIndex = ParentOf(nodeIndex);

        if (parentIndex < 0)
            return;

        var parentValue = Priorities[parentIndex];
        var nodeValue = Priorities[nodeIndex];
        if (parentValue.Priority < nodeValue.Priority)
        {
            var newLeaf = new TreeItem(parentValue.Priority);
            var newParent = new TreeItem(nodeValue.Priority);
            Priorities[parentIndex] = newParent;
            Priorities[nodeIndex] = newLeaf;

            SiftUp(parentIndex);
        }
    }

    private void SiftDown(int nodeIndex)
    {
        var leftChildIndex = LeftChildOf(nodeIndex);

        // If the node doesn't have a left child, it definitely has no right child, since the tree is complete.
        // Therefore the node is a leaf and there is nothing to sift down.
        if (leftChildIndex < 0)
            return;

        var leftChildValue = Priorities[leftChildIndex];

        var rightChildIndex = RightChildOf(nodeIndex);
        var rightChildValue = rightChildIndex >= 0 ? Priorities[rightChildIndex] : new(int.MinValue);

        // Cases where heap property is respected: node >= left >= right, node >= right >= left
        // Cases where heap property has to be restored:
        // - left > node and no right or left >= right => left becomes the new parent of node and right
        // - right > node and right >= left => right becomes the new parent of left and node
        var nodeValue = Priorities[nodeIndex];
        if (leftChildValue.Priority > nodeValue.Priority &&
            (rightChildIndex < 0 || leftChildValue.Priority >= rightChildValue.Priority))
        {
            var newNodeValue = new TreeItem(leftChildValue.Priority);
            var newLeftChildValue = new TreeItem(nodeValue.Priority);
            Priorities[nodeIndex] = newNodeValue;
            Priorities[leftChildIndex] = newLeftChildValue;

            SiftDown(leftChildIndex);
        }
        else if (rightChildIndex >= 0 && 
            rightChildValue.Priority > Math.Max(nodeValue.Priority, leftChildValue.Priority))
        {
            var newNodeValue = new TreeItem(rightChildValue.Priority);
            var newRightChildValue = new TreeItem(nodeValue.Priority);
            Priorities[nodeIndex] = newNodeValue;
            Priorities[rightChildIndex] = newRightChildValue;

            SiftDown(rightChildIndex);
        }
    }

    private static int ParentOf(int nodeIndex) => 
        nodeIndex == 0 ? -1 : (nodeIndex - 1) / 2;
    private int LeftChildOf(int nodeIndex) => 
        2 * nodeIndex + 1 is var result && result < Priorities.Count ? result : -1;
    private int RightChildOf(int nodeIndex) => 
        2 * nodeIndex + 2 is var result && result < Priorities.Count ? result : -1;
}