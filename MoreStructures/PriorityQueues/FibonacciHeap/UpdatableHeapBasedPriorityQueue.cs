namespace MoreStructures.PriorityQueues.FibonacciHeap;

/// <summary>
/// A refinement of <see cref="HeapBasedPriorityQueue{T}"/> which supports <see cref="IUpdatablePriorityQueue{T}"/>
/// operations, such as retrieval and update of priorities and removal of items.
/// </summary>
public class UpdatableHeapBasedPriorityQueue<T> : HeapBasedPriorityQueue<T>, IUpdatablePriorityQueue<T>
    where T : notnull
{
    private Dictionary<T, HeapBasedPriorityQueue<int>> ItemToPushTimestamps { get; } = new();
    private Dictionary<int, TreeNode> PushTimestampToTreeNode { get; } = new();

    #region Public API

    /// <inheritdoc path="//*[not(self::remarks)]"/>
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
    ///     - Otherwise, the queue is iterated over, getting the <see cref="HeapBasedPriorityQueue{T}.TreeNode"/> 
    ///       corresponding to each timestamp extracted from the queue, where such node is still in a heap (it may have
    ///       been detached since).
    ///       <br/>
    ///     - The <see cref="HeapBasedPriorityQueue{T}.TreeNode"/>  is used to make a direct access to the 
    ///       corresponding <see cref="PrioritizedItem{T}"/>. The priority is taken from 
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
    ///     - Retrieving the <see cref="HeapBasedPriorityQueue{T}.TreeNode"/> from the push timestamp and the priority 
    ///       from the <see cref="PrioritizedItem{T}"/> instance are both constant-time operations.
    ///       <br/>
    ///     - Therefore Time and Space Complexity are O(dup_factor).
    ///     </para>
    /// </remarks> 
    public IEnumerable<int> GetPrioritiesOf(T item)
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

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - It first retrieves the max priority P from the max priority item the queue via 
    ///       <see cref="HeapBasedPriorityQueue{T}.Peek"/>.
    ///       <br/>
    ///     - Then, it updates the priority of the provided <paramref name="item"/> via 
    ///       <see cref="UpdatableHeapBasedPriorityQueue{T}.UpdatePriority(T, int)"/>, setting it to P + 1 and making
    ///       <paramref name="item"/> the one with max priority.
    ///       <br/>
    ///     - Finally it pops the <paramref name="item"/> via <see cref="HeapBasedPriorityQueue{T}.Pop"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Both <see cref="HeapBasedPriorityQueue{T}.Peek"/> and 
    ///       <see cref="UpdatableHeapBasedPriorityQueue{T}.UpdatePriority(T, int)"/> have constant 
    ///       Time and Space Complexity (update having constant amortized complexity).
    ///       <br/>
    ///     - However, <see cref="HeapBasedPriorityQueue{T}.Pop"/> has logarithmic Time Complexity.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n) + dup_factor) and Space Complexity is O(1), where dup_factor is 
    ///       the average number of occurrences of an item in the data structure (1 means no duplicates, 2 means the 
    ///       item appears twice, etc.).
    ///     </para>
    /// </remarks> 
    public PrioritizedItem<T>? Remove(T item)
    {
        if (Count == 0)
            return null;
        var maxPrioritizedItem = Peek();
        var treeNode = FindTreeNode(item);
        var oldPrioritizedItem = treeNode.PrioritizedItem;
        UpdatePriority(treeNode, maxPrioritizedItem.Priority + 1, oldPrioritizedItem.PushTimestamp);
        Pop();
        return oldPrioritizedItem;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm-treenode-retrieval">
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
    ///       <see cref="HeapBasedPriorityQueue{T}.TreeNode"/> instances and the 
    ///       <see cref="HeapBasedPriorityQueue{T}.TreeNode.IsInAHeap"/>.
    ///       <br/>
    ///     - If such a timestamp is not found, it means that that <paramref name="item"/> used to be present in the 
    ///       main priority, but it is not anymore. So, an <see cref="InvalidOperationException"/> is thrown.
    ///       <br/>
    ///     - If such a timestamp is found, the <see cref="PrioritizedItem{T}"/> can be accessed via the
    ///       <see cref="HeapBasedPriorityQueue{T}.TreeNode.PrioritizedItem"/> property.
    ///     </para>
    ///     <para id="complexity-treenode-retrieval">
    ///     COMPLEXITY - TREENODE RETRIEVAL PART
    ///     <br/>
    ///     - Retrieving the priority queue associated with the <paramref name="item"/> is a O(1) operation.
    ///       <br/>
    ///     - Finding the right push timestamp may require a number of <see cref="HeapBasedPriorityQueue{T}.Pop"/>
    ///       proportional to the number of times the priority of <paramref name="item"/> has been changed.
    ///       <br/>
    ///     - In the worst case, such number is equal to the number of insertion of <paramref name="item"/>.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(dup_factor) and Space Complexity is O(1), where dup_factor is 
    ///       the average number of occurrences of an item in the data structure (1 means no duplicates, 2 means the 
    ///       item appears twice, etc.).
    ///     </para>
    ///     <para id="algorithm-update">
    ///     ALGORITHM - UPDATE PART
    ///     <br/>
    ///     - The behavior of the algorith is quite differently, when the new priority for the specified item is 
    ///       higher or equal than P, as opposed to when it's lower.
    ///       <br/>
    ///     - When the priority is higher or equal and the node is a root, there is no structural change to the heap. 
    ///       The value of priority is updated and the reference to the max priority is checked and potentially 
    ///       updated.
    ///       <br/>
    ///     - When the priority is higher or equal and the node is not a root, the node is promoted to a root and its 
    ///       loser flag is reset. If the parent of the node was flagged as a loser, the parent is promoted to root 
    ///       too, and its loser flag is reset as well. That continues up to the first ancestor which is not a loser.
    ///       <br/>
    ///     - When the priority is lower, the node is not promoted to a root. Its children are, instead. As in the
    ///       <see cref="HeapBasedPriorityQueue{T}.Pop"/>, merging and max root reference update take place.
    ///       <br/>
    ///     - Finally, the <see cref="PrioritizedItem{T}"/> before the update is returned as result.
    ///     </para>
    ///     <para id="complexity-update">
    ///     COMPLEXITY - UPDATE PART
    ///     <br/>
    ///     - The complexity is different depending on the value of new priority for the specified item being higher 
    ///       or equal than the highest in the queue for that item, or lower.
    ///       <br/>
    ///     - When the value is bigger or equal than P, Time and Space Complexity are O(1), amortized.
    ///       <br/>
    ///     - When the value is smaller than P, Time Complexity and Space Complexity are both O(log(n)). Same analysis
    ///       as for <see cref="HeapBasedPriorityQueue{T}.Pop"/> applies (since very similar operations are performed).
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY - OVERALL
    ///     <br/>
    ///     - When the value is bigger or equal than P, Time Complexity is O(dup_factor) and Space Complexity is O(1), 
    ///       amortized.
    ///       <br/>
    ///     - When the value is smaller than P, Time Complexity is O(log(n) + dup_factor) and Space Complexity is O(1).
    ///     </para>
    /// </remarks> 
    public PrioritizedItem<T> UpdatePriority(T item, int newPriority)
    {
        var treeNode = FindTreeNode(item);
        return UpdatePriority(treeNode, newPriority, CurrentPushTimestamp++);
    }

    #endregion

    #region Hooks

    /// <inheritdoc/>
    protected override void RaiseItemPushed(TreeNode newRoot)
    {
        var prioritizedItem = newRoot.PrioritizedItem;
        PushTimestampToTreeNode[prioritizedItem.PushTimestamp] = newRoot;
        if (!ItemToPushTimestamps.ContainsKey(prioritizedItem.Item))
            ItemToPushTimestamps[prioritizedItem.Item] = new HeapBasedPriorityQueue<int>();
        ItemToPushTimestamps[prioritizedItem.Item].Push(prioritizedItem.PushTimestamp, prioritizedItem.Priority);
    }

    /// <inheritdoc/>
    protected override void RaiseItemPopping()
    {
        PushTimestampToTreeNode.Remove(MaxRootsListNode!.Value.PrioritizedItem.PushTimestamp);
    }

    /// <summary>
    /// Invoked just after the priority of the <see cref="PrioritizedItem{T}"/> of a 
    /// <see cref="HeapBasedPriorityQueue{T}.TreeNode"/> in the heap has changed.
    /// </summary>
    /// <param name="treeNode">The node whose item has changed priority.</param>
    /// <param name="itemBefore">The <see cref="PrioritizedItem{T}"/> as it was before the change.</param>
    protected virtual void RaiseItemPriorityChanged(TreeNode treeNode, PrioritizedItem<T> itemBefore)
    {
        var itemAfter = treeNode.PrioritizedItem;
        PushTimestampToTreeNode.Remove(itemBefore.PushTimestamp);
        PushTimestampToTreeNode[itemAfter.PushTimestamp] = treeNode;
        ItemToPushTimestamps[itemAfter.Item].Push(itemAfter.PushTimestamp, itemAfter.Priority);
    }

    #endregion

    #region Helpers

    private TreeNode FindTreeNode(T item)
    {
        if (!ItemToPushTimestamps.TryGetValue(item, out var pushTimestamps))
            throw new InvalidOperationException("The specified item is not in the queue.");

        TreeNode? treeNode = null;
        while (
            pushTimestamps.Count > 0 &&
            (!PushTimestampToTreeNode.TryGetValue(pushTimestamps.Peek().Item, out treeNode) || !treeNode.IsInAHeap))
        {
            pushTimestamps.Pop();
        }

        if (pushTimestamps.Count == 0)
            throw new InvalidOperationException("The specified item is not in the queue.");

        return treeNode!;
    }

    private PrioritizedItem<T> UpdatePriority(TreeNode treeNode, int newPriority, int newPushTimestamp)
    {
        var newPrioritizedItem =
            new PrioritizedItem<T>(treeNode.PrioritizedItem.Item, newPriority, newPushTimestamp);
        var oldPrioritizedItem = treeNode.PrioritizedItem;
        treeNode.PrioritizedItem = newPrioritizedItem;

        RaiseItemPriorityChanged(treeNode, oldPrioritizedItem);

        // Remark: due to push timestamps, priorities can never be equal: only strictly lower or strictly higher
        if (oldPrioritizedItem.CompareTo(newPrioritizedItem) <= 0)
        {
            if (treeNode.RootsListNode is not null)
                UpdateRootPriority(treeNode, newPrioritizedItem);
            else
                UpdateNonRootPriority(treeNode);
        }
        else
        {
            foreach (var child in treeNode.Children.ToList())
                PromoteChildToRoot(child);

            MergeEquiDegreeTrees();
            UpdateMaxRootsListNode();
        }

        return oldPrioritizedItem;
    }

    private void UpdateRootPriority(
        TreeNode treeNode, PrioritizedItem<T> newPrioritizedItem)
    {
        // The item is at the root of a tree and the new priority is higher => the heap constraints on the tree
        // are not violated, so just check and possibly update the reference to the root with max priority.
        if (MaxRootsListNode!.Value.PrioritizedItem.CompareTo(newPrioritizedItem) < 0)
            MaxRootsListNode = treeNode.RootsListNode;
    }

    private void UpdateNonRootPriority(TreeNode treeNode)
    {
        // The item is not at the root of a tree and the new priority is higher => the heap constraints on the
        // sub-tree of the item are not violated, by the transitivity of max.
        // However, the heap constraints on the items of the tree above the item may be violated.
        // So promote the child to root.
        var parentNode = treeNode.Parent;
        PromoteChildToRoot(treeNode);

        // If the new root, with increased priority, has a priority higher than the current max, update the reference
        // to the root with max priority, to point to the new root.
        if (MaxRootsListNode!.Value.PrioritizedItem.CompareTo(treeNode.PrioritizedItem) < 0)
            MaxRootsListNode = treeNode.RootsListNode;

        // Now, focus on the ancenstors of the disowned child: if its parent has already lost a child before (i.e.
        // it's in the losers set), it's itself promoted to the root. Same applies to the grand-parent and so on,
        // up until the first ancenstor which is not a loser, or the root.
        var ancestorNode = parentNode;
        while (ancestorNode != null)
        {
            if (Losers.Contains(ancestorNode.PrioritizedItem))
            {
                var parentOfAncestorNode = ancestorNode.Parent;
                if (parentOfAncestorNode == null)
                    break;

                PromoteChildToRoot(ancestorNode);
                ancestorNode = parentOfAncestorNode;
            }
            else 
            {
                Losers.Add(ancestorNode.PrioritizedItem);
                break;
            }
        }            
    }

    #endregion
}
