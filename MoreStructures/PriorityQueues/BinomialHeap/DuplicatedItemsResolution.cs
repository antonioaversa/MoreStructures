namespace MoreStructures.PriorityQueues.BinomialHeap;

/// <summary>
/// An object storing and keeping up-to-date the "<typeparamref name="TItems"/> to <see cref="TreeNode{T}"/>" 
/// back-references, necessary to find back the <see cref="TreeNode{T}"/> in the heap with highest priority for a given 
/// item, without exposing iterators to the client.
/// </summary>
/// <typeparam name="TItems"><inheritdoc cref="IPriorityQueue{T}"/></typeparam>
/// <typeparam name="THeap">
/// A type constructor for an heap of <see cref="int"/>. Needed to store all push timestamps for each item.
/// </typeparam>
/// <remarks>
/// In order to support updates and deletions of items, two additional data structures are introduced:
/// <br/>
/// - a <see cref="Dictionary{TKey, TValue}"/> Item2PT, mapping items <c>I</c> of type <typeparamref name="TItems"/> to 
///   <typeparamref name="THeap"/> instances, containing <see cref="PrioritizedItem{T}.PushTimestamp"/>
///   values of type <see cref="int"/>, of <see cref="PrioritizedItem{T}"/> instances containing <c>I</c>.
///   <br/>
/// - a <see cref="Dictionary{TKey, TValue}"/> PT2Idx from <see cref="PrioritizedItem{T}.PushTimestamp"/> values to
///   <see cref="TreeNode{T}"/> of type <see cref="int"/>, into the backing <typeparamref name="THeap"/> structure of 
///   this priority queue.
///   <br/>
/// <br/>
/// Every time a request to remove or update an item <c>I</c> from the priority queue is made, Item2PT is used to
/// retrieve all the <see cref="PrioritizedItem{T}.PushTimestamp"/> values of <see cref="PrioritizedItem{T}"/>
/// instances with item.
/// <br/>
/// Those push timestamps can be multiple because the same item can be added multiple times to the queue.
/// <br/>
/// The push timestamps are organized themselves in per-item priority queues, with the same priority as the items
/// in the main priority queue.
/// <br/>
/// This way, the push timestamp of highest priority for a given item can be peeked in constant time and extracted in 
/// logarithmic time.
/// <br/>
/// Once the timestamp of highest priority has been found, the corresponding <see cref="TreeNode{T}"/> (if any) in the 
/// backing <typeparamref name="THeap"/> structure of this priority queue can be found in constant time via the PT2Idx 
/// dictionary.
/// </remarks>
public class DuplicatedItemsResolution<TItems, THeap>
    where TItems : notnull
    where THeap : IPriorityQueue<int>, new()
{
    private Dictionary<TItems, THeap> ItemToPushTimestamps { get; } = new();
    private Dictionary<int, TreeNode<TItems>> PushTimestampToTreeNode { get; } = new();

    /// <summary>
    /// Clears the content of this object.
    /// </summary>
    public void Clear()
    {
        ItemToPushTimestamps.Clear();
        PushTimestampToTreeNode.Clear();
    }

    /// <inheritdoc cref="IUpdatablePriorityQueue{T}.GetPrioritiesOf(T)" path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - First, the priority queue of push timestamps for the provided <paramref name="item"/> is retrieved from 
    ///       the dictionary of per-item queues of push timestamps.
    ///       <br/>
    ///     - If such a queue is not found, <paramref name="item"/> is not present in the main priority queue, and an 
    ///       empty sequence is returned.
    ///       <br/>
    ///     - Otherwise, the queue is iterated over, getting the <see cref="TreeNode{T}"/> corresponding to each 
    ///       timestamp extracted from the queue, where such node is still in a heap (it may have been detached since).
    ///       <br/>
    ///     - The <see cref="TreeNode{T}"/>  is used to make a direct access to 
    ///       the corresponding <see cref="PrioritizedItem{T}"/>. The priority is taken from 
    ///       <see cref="PrioritizedItem{T}.Priority"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Retrieving the priority queue of push timestamps from the dictionary of per-item priority queues is a 
    ///       O(1) operation.
    ///       <br/>
    ///     - Iterating such a priority queue requires duplicating the underlying data structure, which is a 
    ///       O(dup_factor) operation, where dup_factor is the average number of occurrences of an item in the data 
    ///       structure (1 means no duplicates, 2 means the item appears twice, etc.).
    ///       <br/>
    ///     - Retrieving the <see cref="TreeNode{T}"/> from the push timestamp and the priority from the 
    ///       <see cref="PrioritizedItem{T}"/> instance are both constant-time operations.
    ///       <br/>
    ///     - Therefore Time and Space Complexity are O(dup_factor), where dup_factor is 
    ///       the average number of occurrences of an item in the data structure (1 means no duplicates, 2 means the 
    ///       item appears twice, etc.).
    ///     </para>
    /// </remarks> 
    public IEnumerable<int> GetPrioritiesOf(TItems item)
    {
        if (!ItemToPushTimestamps.TryGetValue(item, out var pushTimestamps))
            return Enumerable.Empty<int>();

        return
            from pushTimestamp in pushTimestamps
            where PushTimestampToTreeNode.ContainsKey(pushTimestamp)
            let treeNode = PushTimestampToTreeNode[pushTimestamp]
            where treeNode.IsInAHeap
            select treeNode.PrioritizedItem.Priority;
    }

    /// <summary>
    /// Retrieves the <see cref="TreeNode{T}"/> associated with the provided <paramref name="item"/> in the queue,
    /// selecting the one of highest priority in case of duplicates (i.e. multiple occurrences of 
    /// <paramref name="item"/> within the priority queue).
    /// </summary>
    /// <param name="item">The item to be mapped to a <see cref="TreeNode{T}"/>.</param>
    /// <returns>The corresponding <see cref="TreeNode{T}"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Raised when the provided <paramref name="item"/> is not found in the queue, so it can't be mapped to a 
    /// <see cref="TreeNode{T}"/>.
    /// </exception>
    /// <remarks>
    ///     <para id = "algorithm-treenode-retrieval" >
    ///     ALGORITHM - TREENODE RETRIEVAL PART
    ///     <br/>
    ///     - The priority queue of push timestamps for the provided <paramref name="item"/> is retrieved from the
    ///       dictionary of per-item queues of push timestamps.
    ///       <br/>
    ///     - If such a priority queue is not found, it means that <paramref name="item"/> is not present in the main
    ///       priority queue either. So, an <see cref="InvalidOperationException"/> is thrown.
    ///       <br/>
    ///     - If the priority queue is found, push timestamps are popped from it until the root of the priority queue 
    ///       contains a valid timestamp ts, i.e. a timestamp present in the dictionary mapping timestamps to
    ///       <see cref="TreeNode{T}"/> instances and the <see cref="TreeNode{T}.IsInAHeap"/>.
    ///       <br/>
    ///     - If such a timestamp is not found, it means that that <paramref name="item"/> used to be present in the 
    ///       main priority, but it is not anymore. So, an <see cref="InvalidOperationException"/> is thrown.
    ///       <br/>
    ///     - If such a timestamp is found, the <see cref="PrioritizedItem{T}"/> can be accessed via the
    ///       <see cref="TreeNode{T}.PrioritizedItem"/> property.
    ///     </para>
    ///     <para id="complexity-treenode-retrieval">
    ///     COMPLEXITY - TREENODE RETRIEVAL PART
    ///     <br/>
    ///     - Retrieving the priority queue associated with the <paramref name="item"/> is a O(1) operation.
    ///       <br/>
    ///     - Finding the right push timestamp may require a number of <see cref="BinomialHeapPriorityQueue{T}.Pop"/>
    ///       proportional to the number of times the priority of <paramref name="item"/> has been changed.
    ///       <br/>
    ///     - In the worst case, such number is equal to the number of insertion of <paramref name="item"/>.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(dup_factor) and Space Complexity is O(1), where dup_factor is 
    ///       the average number of occurrences of an item in the data structure (1 means no duplicates, 2 means the 
    ///       item appears twice, etc.).
    ///     </para>
    /// </remarks>
    public TreeNode<TItems>? FindTreeNode(TItems item)
    {
        if (!ItemToPushTimestamps.TryGetValue(item, out var pushTimestamps))
            return null;

        TreeNode<TItems>? treeNode = null;
        while (
            pushTimestamps.Count > 0 &&
            (!PushTimestampToTreeNode.TryGetValue(pushTimestamps.Peek().Item, out treeNode) || !treeNode.IsInAHeap))
        {
            pushTimestamps.Pop();
        }

        if (pushTimestamps.Count == 0)
            return null;

        return treeNode;
    }

    /// <summary>
    /// To be invoked just after a <paramref name="newRoot"/> has been pushed into the heap.
    /// </summary>
    /// <param name="newRoot">The new root pushed into the heap forest.</param>
    public void RaiseItemPushed(TreeNode<TItems> newRoot)
    {
        var prioritizedItem = newRoot.PrioritizedItem;
        PushTimestampToTreeNode[prioritizedItem.PushTimestamp] = newRoot;
        if (!ItemToPushTimestamps.TryGetValue(prioritizedItem.Item, out var pushTimestamps))
            ItemToPushTimestamps[prioritizedItem.Item] = pushTimestamps = new THeap();
        pushTimestamps.Push(prioritizedItem.PushTimestamp, prioritizedItem.Priority);
    }

    /// <summary>
    /// To be invoked just before the provided <paramref name="root"/> is popped from the heap.
    /// </summary>
    /// <param name="root">The root about to be popped from the heap forest.</param>
    public void RaiseItemPopping(TreeNode<TItems> root)
    {
        PushTimestampToTreeNode.Remove(root.PrioritizedItem.PushTimestamp);
    }

    /// <summary>
    /// To be invoked just after the priority of a <see cref="TreeNode{T}"/> has changed.
    /// </summary>
    /// <param name="treeNode">The node in the heap which has changed priority.</param>
    /// <param name="itemBefore">The <see cref="PrioritizedItem{T}"/> as it was before the priority change.</param>
    public void RaiseItemPriorityChanged(TreeNode<TItems> treeNode, PrioritizedItem<TItems> itemBefore)
    {
        var itemAfter = treeNode.PrioritizedItem;
        PushTimestampToTreeNode.Remove(itemBefore.PushTimestamp);
        PushTimestampToTreeNode[itemAfter.PushTimestamp] = treeNode;
        ItemToPushTimestamps[itemAfter.Item].Push(itemAfter.PushTimestamp, itemAfter.Priority);
    }

    /// <summary>
    /// Invoked just after two items have had their <see cref="PrioritizedItem{T}"/> swapped.
    /// </summary>
    /// <param name="treeNode1">The first node.</param>
    /// <param name="treeNode2">The second node.</param>
    public void RaiseItemsSwapped(TreeNode<TItems> treeNode1, TreeNode<TItems> treeNode2)
    {
        PushTimestampToTreeNode[treeNode1.PrioritizedItem.PushTimestamp] = treeNode1;
        PushTimestampToTreeNode[treeNode2.PrioritizedItem.PushTimestamp] = treeNode2;
    }
}
