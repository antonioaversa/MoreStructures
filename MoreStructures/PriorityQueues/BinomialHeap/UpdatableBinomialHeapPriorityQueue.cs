using MoreStructures.PriorityQueues.FibonacciHeap;

namespace MoreStructures.PriorityQueues.BinomialHeap;

/// <summary>
/// A refinement of <see cref="BinomialHeapPriorityQueue{T}"/> which supports <see cref="IUpdatablePriorityQueue{T}"/>
/// operations, such as retrieval and update of priorities and removal of items.
/// </summary>
/// <remarks>
/// Check <see cref="DuplicatedItemsResolution{T, THeap}"/> for detailed informations about how the mapping between 
/// items of type <typeparamref name="T"/> and heap nodes of type <see cref="TreeNode{T}"/> is performed, in presence 
/// of duplicates.
/// </remarks>
public class UpdatableBinomialHeapPriorityQueue<T> : BinomialHeapPriorityQueue<T>, IUpdatablePriorityQueue<T>
    where T : notnull
{
    private DuplicatedItemsResolution<T, BinomialHeapPriorityQueue<int>> DuplicatedItemsResolution { get; } = new();

    #region Public API

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Clears the <see cref="BinomialHeapPriorityQueue{T}"/> structures and the additional 
    /// <see cref="DuplicatedItemsResolution{TItems, THeap}"/> object introduced to support updates and deletions.
    /// <br/>
    /// Time and Space Complexity is O(1).
    /// </remarks>
    public override void Clear()
    {
        base.Clear();
        DuplicatedItemsResolution.Clear();
    }

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
    ///     - <see cref="BinomialHeapPriorityQueue{T}.Peek"/> has constant Time and Space Complexity.
    ///       <br/>
    ///     - However, <see cref="BinomialHeapPriorityQueue{T}.Pop"/> and <see cref="UpdatePriority(T, int)"/> have 
    ///       logarithmic Time Complexity.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n) + dup_factor) and Space Complexity is O(1).
    ///     </para>
    /// </remarks> 
    public PrioritizedItem<T>? Remove(T item)
    {
        if (Count == 0)
            return null;

        var maxPrioritizedItem = Peek();
        var treeNodeInBinomialHeap = DuplicatedItemsResolution.FindTreeNode(item);
        if (treeNodeInBinomialHeap == null)
            return null;

        var oldPrioritizedItem = treeNodeInBinomialHeap.PrioritizedItem;
        UpdatePriority(treeNodeInBinomialHeap, maxPrioritizedItem.Priority + 1, oldPrioritizedItem.PushTimestamp);
        Pop();
        return oldPrioritizedItem;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="DuplicatedItemsResolution{T, THeap}.FindTreeNode(T)"/>
    ///     <para id="algorithm-update">
    ///     ALGORITHM - BINOMIAL HEAP UPDATE PART
    ///     <br/>
    ///     - When the priority is higher or equal, the value of priority is updated and the node is sifted up the 
    ///       tree, until the heap property is restored. If the root of the tree is reached, the reference to the max 
    ///       priority is checked and potentially updated.
    ///       <br/>
    ///     - When the priority is lower, the value of priority is updated and the node is sifted down the tree, until
    ///       the heap property is restored. If the node was a root, a linear scan of all the roots is made, to update
    ///       the reference to the max priority, which may have changed due to the decrease in priority.
    ///       <br/>
    ///     - No merging is required, since no node has been added or removed from any of the tree of the heap forest.
    ///       Nodes have been swapped by sift up or sift down procedures, but the shape of each tree, and so the
    ///       layout of the forest, hasn't changed. Therefore the binomial heap shape hasn't been violated.
    ///       <br/>
    ///     - Finally, the <see cref="PrioritizedItem{T}"/> before the update is returned as result.
    ///     </para>
    ///     <para id="complexity-update">
    ///     COMPLEXITY - BINOMIAL HEAP UPDATE PART
    ///     <br/>
    ///     - Sift up, sift down and linear scan of roots are all logarithmic operations in time and constant in space.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n)) and Space Complexity is O(1).
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY - OVERALL
    ///     <br/>
    ///     - Time Complexity is O(log(n) + dup_factor) and Space Complexity is O(1).
    ///       <br/>
    ///     - Notice how this is higher than the Time Complexity for the corresponding functionality in a Fibonacci
    ///       Heap, which supports both pushing and priority updating in constant time.
    ///     </para>
    /// </remarks> 
    public PrioritizedItem<T> UpdatePriority(T item, int newPriority)
    {
        var treeNodeInBinomialHeap = DuplicatedItemsResolution.FindTreeNode(item);
        if (treeNodeInBinomialHeap == null)
            throw new InvalidOperationException("The specified item is not in the queue.");
        return UpdatePriority(treeNodeInBinomialHeap, newPriority, CurrentPushTimestamp++);
    }

    #endregion

    #region Hooks

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Hands over to <see cref="DuplicatedItemsResolution{T, THeap}.RaiseItemPushed(TreeNode{T})"/>.
    /// </remarks>
    protected override void RaiseItemPushed(TreeNode<T> newRoot) =>
        DuplicatedItemsResolution.RaiseItemPushed(newRoot);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Hands over to <see cref="DuplicatedItemsResolution{T, THeap}.RaiseItemPopping(TreeNode{T})"/>.
    /// </remarks>
    protected override void RaiseItemPopping(TreeNode<T> root) =>
        DuplicatedItemsResolution.RaiseItemPopping(root);

    /// <summary>
    /// Invoked just after the priority of the <see cref="PrioritizedItem{T}"/> of a <see cref="TreeNode{T}"/> in the 
    /// heap has changed.
    /// </summary>
    /// <param name="treeNode">The node whose item has changed priority.</param>
    /// <param name="itemBefore">The <see cref="PrioritizedItem{T}"/> as it was before the change.</param>
    /// <remarks>
    /// Hands over to 
    /// <see cref="DuplicatedItemsResolution{T, THeap}.RaiseItemPriorityChanged(TreeNode{T}, PrioritizedItem{T})"/>.
    /// </remarks>
    protected virtual void RaiseItemPriorityChanged(TreeNode<T> treeNode, PrioritizedItem<T> itemBefore) =>
        DuplicatedItemsResolution.RaiseItemPriorityChanged(treeNode, itemBefore);

    /// <summary>
    /// Invoked just after two items have had their <see cref="PrioritizedItem{T}"/> swapped.
    /// </summary>
    /// <param name="treeNode1">The first node.</param>
    /// <param name="treeNode2">The second node.</param>
    /// <remarks>
    /// Hands over to 
    /// <see cref="DuplicatedItemsResolution{T, THeap}.RaiseItemsSwapped(TreeNode{T}, TreeNode{T})"/>.
    /// </remarks>
    protected virtual void RaiseItemsSwapped(TreeNode<T> treeNode1, TreeNode<T> treeNode2) =>
        DuplicatedItemsResolution.RaiseItemsSwapped(treeNode1, treeNode2);

    #endregion

    #region Helpers

    private PrioritizedItem<T> UpdatePriority(TreeNode<T> treeNode, int newPriority, int newPushTimestamp)
    {
        var newPrioritizedItem =
            new PrioritizedItem<T>(treeNode.PrioritizedItem.Item, newPriority, newPushTimestamp, CurrentEra);
        var oldPrioritizedItem = treeNode.PrioritizedItem;
        treeNode.PrioritizedItem = newPrioritizedItem;

        RaiseItemPriorityChanged(treeNode, oldPrioritizedItem);

        if (newPrioritizedItem.CompareTo(oldPrioritizedItem) >= 0)
            SiftUp(treeNode);
        else
            SiftDown(treeNode);

        return oldPrioritizedItem;
    }

    private void SiftUp(TreeNode<T> treeNode)
    {
        var ancestor = treeNode;
        while (
            ancestor.Parent is TreeNode<T> ancestorParent &&
            ancestorParent.PrioritizedItem.CompareTo(ancestor.PrioritizedItem) < 0)
        {
            (ancestor.PrioritizedItem, ancestorParent.PrioritizedItem) =
                (ancestorParent.PrioritizedItem, ancestor.PrioritizedItem);
            RaiseItemsSwapped(ancestor, ancestorParent);

            ancestor = ancestorParent;
        }

        // If we are in SiftUp, it means that the priority has increased. The SiftUp may have bubbled up the increase
        // up to the root of the tree. In such a case we need to check whether the new root has higher priority than
        // the current max priority root, and update it if necessary.
        if (ancestor.RootsListNode != null)
            UpdateMaxRootsListNodeAfterRootNewOrIncrease(ancestor.RootsListNode);
    }

    private void SiftDown(TreeNode<T> treeNode)
    {
        var descendant = treeNode;
        while (
            descendant.Children.MaxBy(c => c.PrioritizedItem) is TreeNode<T> descendantMaxChild &&
            descendantMaxChild.PrioritizedItem.CompareTo(descendant.PrioritizedItem) > 0)
        {
            (descendant.PrioritizedItem, descendantMaxChild.PrioritizedItem) =
                (descendantMaxChild.PrioritizedItem, descendant.PrioritizedItem);
            RaiseItemsSwapped(descendant, descendantMaxChild);

            descendant = descendantMaxChild;
        }

        // If we are in SiftDown, it means that the priority has decreased. If the treeNode was a root, the SiftDown
        // may have moved the root down the tree, and promoted one of its child to being the root. And even if it
        // didn't happen, the root has now lower priority. So, if the max priority root was pointing to the root of
        // this tree, we need to update the current max priority root, performing a linear scan of the roots.
        if (ReferenceEquals(MaxRootsListNode!.Value, treeNode))
            UpdateMaxRootsListNode();
    }

    #endregion
}
