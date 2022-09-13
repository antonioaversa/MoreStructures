using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.PriorityQueues.FibonacciHeap;

/// <summary>
/// A refinement of <see cref="FibonacciHeapPriorityQueue{T}"/> which supports <see cref="IUpdatablePriorityQueue{T}"/>
/// operations, such as retrieval and update of priorities and removal of items.
/// </summary>
/// <remarks>
/// Check <see cref="DuplicatedItemsResolution{T, THeap}"/> for detailed informations about how the mapping between 
/// items of type <typeparamref name="T"/> and heap nodes of type <see cref="TreeNode{T}"/> is performed, in presence 
/// of duplicates.
/// </remarks>
public class UpdatableFibonacciHeapPriorityQueue<T> : FibonacciHeapPriorityQueue<T>, IUpdatablePriorityQueue<T>
    where T : notnull
{
    private DuplicatedItemsResolution<T, FibonacciHeapPriorityQueue<int>> DuplicatedItemsResolution { get; } = new();

    #region Public API

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="DuplicatedItemsResolution{T, THeap}.GetPrioritiesOf(T)"/>
    /// </remarks> 
    public IEnumerable<int> GetPrioritiesOf(T item) => DuplicatedItemsResolution.GetPrioritiesOf(item);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - It first retrieves the max priority P from the max priority item the queue via 
    ///       <see cref="BinomialHeapPriorityQueue{T}.Peek"/>.
    ///       <br/>
    ///     - Then, it updates the priority of the provided <paramref name="item"/> via 
    ///       <see cref="UpdatePriority(T, int)"/>, setting it to P + 1 and making <paramref name="item"/> the one with 
    ///       max priority.
    ///       <br/>
    ///     - Finally it pops the <paramref name="item"/> via <see cref="BinomialHeapPriorityQueue{T}.Pop"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Both <see cref="BinomialHeapPriorityQueue{T}.Peek"/> and <see cref="UpdatePriority(T, int)"/> have 
    ///       constant Time and Space Complexity (update having constant amortized complexity).
    ///       <br/>
    ///     - However, <see cref="BinomialHeapPriorityQueue{T}.Pop"/> has logarithmic Time Complexity.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n) + dup_factor) and Space Complexity is O(1).
    ///     </para>
    /// </remarks> 
    public PrioritizedItem<T>? Remove(T item)
    {
        if (Count == 0)
            return null;
        var maxPrioritizedItem = Peek();
        var treeNodeInFibonacciHeap = DuplicatedItemsResolution.FindTreeNode(item);
        var oldPrioritizedItem = treeNodeInFibonacciHeap.PrioritizedItem;
        UpdatePriority(treeNodeInFibonacciHeap, maxPrioritizedItem.Priority + 1, oldPrioritizedItem.PushTimestamp);
        Pop();
        return oldPrioritizedItem;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="DuplicatedItemsResolution{T, THeap}.FindTreeNode(T)"/>
    ///     <para id="algorithm-update">
    ///     ALGORITHM - FIBONACCI HEAP UPDATE PART
    ///     <br/>
    ///     - The algorith behaves quite differently, depending on whether the new priority for the specified item is 
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
    ///       <see cref="BinomialHeapPriorityQueue{T}.Pop"/>, merging and max root reference update take place.
    ///       <br/>
    ///     - Finally, the <see cref="PrioritizedItem{T}"/> before the update is returned as result.
    ///     </para>
    ///     <para id="complexity-update">
    ///     COMPLEXITY - FIBONACCI HEAP UPDATE PART
    ///     <br/>
    ///     - The complexity is different depending on the value of new priority for the specified item being higher 
    ///       or equal than the highest in the queue for that item, or lower.
    ///       <br/>
    ///     - When the value is bigger or equal than P, Time and Space Complexity are O(1), amortized.
    ///       <br/>
    ///     - When the value is smaller than P, Time Complexity and Space Complexity are both O(log(n)). Same analysis
    ///       as for <see cref="BinomialHeapPriorityQueue{T}.Pop"/> applies (since very similar operations are 
    ///       performed).
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
        var treeNodeInFibonacciHeap = DuplicatedItemsResolution.FindTreeNode(item);
        return UpdatePriority(treeNodeInFibonacciHeap, newPriority, CurrentPushTimestamp++);
    }

    #endregion

    #region Hooks

    /// <inheritdoc cref="UpdatableBinomialHeapPriorityQueue{T}.RaiseItemPushed"/>
    protected override void RaiseItemPushed(TreeNode<T> newRoot) => 
        DuplicatedItemsResolution.RaiseItemPushed(newRoot);

    /// <inheritdoc cref="UpdatableBinomialHeapPriorityQueue{T}.RaiseItemPopping"/>
    protected override void RaiseItemPopping(TreeNode<T> root) => 
        DuplicatedItemsResolution.RaiseItemPopping(root);

    /// <inheritdoc cref="UpdatableBinomialHeapPriorityQueue{T}.RaiseItemPriorityChanged"/>
    protected virtual void RaiseItemPriorityChanged(TreeNode<T> treeNode, PrioritizedItem<T> itemBefore) =>
        DuplicatedItemsResolution.RaiseItemPriorityChanged(treeNode, itemBefore);

    #endregion

    #region Helpers

    private PrioritizedItem<T> UpdatePriority(TreeNode<T> treeNode, int newPriority, int newPushTimestamp)
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
        TreeNode<T> treeNode, PrioritizedItem<T> newPrioritizedItem)
    {
        // The item is at the root of a tree and the new priority is higher => the heap constraints on the tree
        // are not violated, so just check and possibly update the reference to the root with max priority.
        if (MaxRootsListNode!.Value.PrioritizedItem.CompareTo(newPrioritizedItem) < 0)
            MaxRootsListNode = treeNode.RootsListNode;
    }

    private void UpdateNonRootPriority(TreeNode<T> treeNode)
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
