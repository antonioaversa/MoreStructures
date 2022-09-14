using MoreStructures.Utilities;
using System.Collections;

namespace MoreStructures.PriorityQueues.BinomialHeap;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on a Binomial Max Heap of its items. It also supports
/// <see cref="IMergeablePriorityQueue{T, TPQTarget}"/> operations.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IPriorityQueue{T}"/></typeparam>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     - A <b>Binomial Heap</b> is a forest of <b>n-ary trees</b>, each respecting the <b>heap max property</b>, 
///       having a <b>binomial layout</b> and <b>unicity of degree</b>.
///       <br/>
///     - A tree respects the heap max property when each node has a value to be non-smaller than all its children 
///       (and, by transitivity, its grand-children, grand-grand-children etc.).
///       <br/>
///     - A tree has a binomial layout when is in the set of tree defined in the following constructive way: a
///       singleton tree is a binomial tree of degree 0, a binomial tree of degree k + 1 is obtained merging two
///       binomial trees of degree k, so that the root of one becomes immediate child of the root of the other.
///       <br/>
///     - A tree has unicity of degree when it is the only tree in the forest having its degree, which is the number of
///       children it has. That means that there can be a single tree with 0 children (singleton), a single tree with 
///       1 child, etc.
///     </para>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - This heap implementation is conceptually extending <see cref="BinaryHeap.BinaryHeapPriorityQueue{T}"/>, 
///       making heaps easily mergeable (i.e. in less time than O(n).
///       <br/>
///     - Binary Heaps provide logarithmic insertion and extraction. They can also provide linear construction, when
///       the data is provided in batch and not online.
///       <br/>
///     - However, Binary Heap have O(n * log(n)) Time Complexity in merge. Merging the smaller heap into the bigger
///       one, in the worst case the two heaps being merged have comparable size n / 2, resulting into an overall 
///       O(n / 2 * log(n / 2)) = O(n * log(n)) Time Complexity.
///       <br/>
///     - Merging the underlying arrays and building the new Binary Heap in batch would improve performance, yielding
///       O(n) Time Complexity. Still an expensive operation, as it means going through all elements of one heap.
///       <br/>
///     - Binomial Heaps overcome this limitation and offer sub-linear performance by taking advantage of both the 
///       linked list layout and the tree layout, and taking the best of both worlds. 
///       <br/>
///     - So the underlying idea behind Binomial Heaps is to combine linked lists and trees, and represent the data as 
///       a forest of n-ry heap trees (respecting the binomial layout), which can be easily merged together into a
///       single Binomial Heap, due to their "recurrent" structure.
///       <br/>
///     - While <see cref="Push(T, int)"/> and <see cref="Pop"/> retain logarithmic complexity, merging also becomes
///       a logarithmic operation.
///     </para>
/// </remarks>
public partial class BinomialHeapPriorityQueue<T> 
    : IPriorityQueue<T>, IMergeablePriorityQueue<T, BinomialHeapPriorityQueue<T>>
    where T : notnull
{
    /// <summary>
    /// A <see cref="LinkedList{T}"/> of all the roots of the forest representing the heap.
    /// </summary>
    protected LinkedList<TreeNode<T>> Roots { get; }

    /// <summary>
    /// The total number of items in this queue.
    /// </summary>
    protected int ItemsCount { get; set; }

    /// <summary>
    /// Reference to the tree root in the forest with the highest priority. Makes Peek O(1).
    /// </summary>
    protected LinkedListNode<TreeNode<T>>? MaxRootsListNode { get; set; }

    /// <summary>
    /// A non-negative, zero-based, monotonically strictly increasing counter, incremented at every insertion into this 
    /// data structure by a <see cref="Push(T, int)"/>.
    /// </summary>
    protected int CurrentPushTimestamp { get; set; }

    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    public BinomialHeapPriorityQueue()
    {
        Roots = new();

        ItemsCount = 0;
        MaxRootsListNode = null;
        CurrentPushTimestamp = 0;
    }

    /// <summary>
    /// Builds a deep, separate copy of the provided <paramref name="source"/> priority queue.
    /// </summary>
    /// <param name="source">The priority queue to be copied over.</param>
    /// <remarks>
    /// Doesn't copy the items themselves, it only deep-copies the internal structure of the <paramref name="source"/>
    /// queue.
    /// </remarks>
    public BinomialHeapPriorityQueue(BinomialHeapPriorityQueue<T> source)
    {
        Roots = new LinkedList<TreeNode<T>>(source.Roots.Select(r => r.DeepCopy()));
        foreach (var rootsListNode in Roots.AsNodes())
            rootsListNode.Value.RootsListNode = rootsListNode;

        ItemsCount = source.ItemsCount;
        UpdateMaxRootsListNode();
        CurrentPushTimestamp = source.CurrentPushTimestamp;
    }

    #region Public API

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks the count internally stored, keeping track of the sum of the size of all trees in the linked list.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public virtual int Count => ItemsCount;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// In order to return items in heap order (i.e. the max at each step), it first copies this priority queue into a
    /// second temporary queue, which can be mutated without affecting the state of this queue.
    /// <br/>
    /// It then iterates over the copy, calling <see cref="Pop"/> until it becomes empty.
    /// <br/>
    /// Time Complexity is O(n * log(n)) (when fully enumerated), because a single <see cref="Pop"/> on a Binomial 
    /// Heap takes logarithmic time, and there are n items to be extracted.
    /// <br/>
    /// Space Complexity is O(n), as a copy of this queue is required as auxiliary data structure to emit elements in 
    /// the right order of priority.
    /// </remarks> 
    public virtual IEnumerator<T> GetEnumerator()
    {
        var copy = new BinomialHeapPriorityQueue<T>(this);
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
    /// Peeks the item of the linked list pointed by the "max root" internal property.
    /// <br/>
    /// By transitivity, the item of the max root contains the max item in the queue, since all roots are non-smaller
    /// than their descendants.
    /// <br/>
    /// Therefore, Time and Space Complexity are O(1).
    /// </remarks>
    public virtual PrioritizedItem<T> Peek()
    {
        if (MaxRootsListNode == null)
            throw new InvalidOperationException($"Can't {nameof(Peek)} on an empty queue.");

        return MaxRootsListNode.Value.PrioritizedItem;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Adds a new singleton tree T (degree 0) to the forest, wrapping the provided <paramref name="item"/> with 
    ///       its <paramref name="priority"/> into an object R with no children, and wrapping R into a 
    ///       <see cref="LinkedListNode{T}"/> LLN, which represents the root of T.
    ///       <br/>
    ///     - Then all trees with the same degree are merged together, with the same procedure explained in the
    ///       documentation of <see cref="Pop"/>. 
    ///       <br/>
    ///     - That ensures that the binomial shape of the forest (i.e. binomial trees all of different degrees) is 
    ///       preserved. Without merging, if a tree of degree 0 (i.e. a singleton tree) was already present before the
    ///       push of the new root, the binomial heap property would have been violated.
    ///       <br/>
    ///     - After all equi-degree trees have been merged, a new linear scan of the root is done, to update the 
    ///       reference to the root with highest priority (so that <see cref="Peek"/> will work correctly and in 
    ///       constant time).
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Adding a new root to the forest of trees is a constant-time operation, since the root is added at
    ///       the end of the linked list representing the forest, which keeps references to both ends of the chain of 
    ///       nodes.
    ///       <br/>
    ///     - Merging equi-degree trees, to restore the binomial shape of the heap forest, and updating the max root 
    ///       reference are both logarithmic operations in time. The merge also requires a space proportional to the
    ///       logarithm of the number of items in the heap, to instantiate its data structure.
    ///       <br/>
    ///     - Check the complexity analysis of <see cref="Pop"/> for further details.
    ///       <br/>
    ///     - Therefore, Time and Space Complexity are both O(log(n)).
    ///     </para>
    /// </remarks> 
    public virtual void Push(T item, int priority)
    {
        var prioritizedItem = new PrioritizedItem<T>(item, priority, CurrentPushTimestamp++);
        var newRoot = new TreeNode<T> { PrioritizedItem = prioritizedItem };
        AddRoot(newRoot);
        RaiseItemPushed(newRoot);
        MergeEquiDegreeTrees();
        UpdateMaxRootsListNode();
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The value of the tree root with highest priority is read, then the root is removed from the forest.
    ///       <br/>
    ///     - The orphan children of the removed root are promoted to being roots and added to the forest.
    ///       <br/>
    ///     - Then all trees with the same degree are merged together, where the root with lower priority becomes 
    ///       immediate child of the root with higher priority.
    ///       <br/>
    ///     - Merging is done efficiently by linearly scanning all the tree roots in the forest, and keeping an array
    ///       A of tree roots indexed by their degree.
    ///       <br/>
    ///     - Every time a second tree with the same degree is encountered: if T1 has degree k and A[k] already 
    ///       references another tree T2 with degree k, T1 and T2 are merged into a tree whose root has degree k + 1, 
    ///       A[k] is reset and A[k + 1] is set.
    ///       <br/>
    ///     - After all equi-degree trees have been merged, a new linear scan of the root is done, to update the 
    ///       reference to the root with highest priority.
    ///     </para>
    ///     <para>
    ///     COMPLEXITY
    ///     <br/>
    ///     - Removing the root with highest priority from the forest is a O(1) operation.
    ///       <br/>
    ///     - Promoting to roots all its children is proportional to the number of children, a root of the forest has.
    ///       <br/>
    ///     - In a Binomial Heap the max degree of the roots of the forest is logarithmic with the total number of 
    ///       items in the heap. Therefore, promotion of children to roots is a O(log(n)) operation.
    ///       <br/>
    ///     - Merging equi-degree trees requires a full scan of the roots in the forest. If the forest were made of
    ///       a lot of shallow trees, that would result into a O(n) operation.
    ///       <br/>
    ///     - However, while <see cref="Push(T, int)"/> increases every time by 1 the count of trees, after n 
    ///       insertions in O(1), a single O(n) scan done by a <see cref="Pop"/> would merge trees, making them
    ///       logarithmic in number.
    ///       <br/>
    ///     - Updating the max root reference takes time proportional to the number of trees in the forest AFTER the
    ///       merging.
    ///       <br/>
    ///     - Because the merging of equi-degree trees leaves a forest of trees all of different degrees, and the max 
    ///       degree is logarithmic with n, at the end of the merging procedure there can be at most a logarithmic
    ///       number of different trees in the forest.
    ///       <br/>
    ///     - So the linear scan of tree roots in the forest to find the max root reference takes only a logarithmic
    ///       amount of time.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n)). Space Complexity is also O(log(n)), since merging equi-degree
    ///       trees requires instantiating an array index by degrees, and max degree is O(log(n)).
    ///     </para>
    /// </remarks> 
    public virtual PrioritizedItem<T> Pop()
    {
        if (MaxRootsListNode == null)
            throw new InvalidOperationException($"Can't {nameof(Pop)} on an empty queue.");

        RaiseItemPopping(MaxRootsListNode.Value);
        var oldRoot = DetachFromRoots(MaxRootsListNode);
        ItemsCount--;

        foreach (var child in oldRoot.Children.ToList())
            PromoteChildToRoot(child);

        MergeEquiDegreeTrees();
        UpdateMaxRootsListNode();

        return oldRoot.PrioritizedItem;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Iterates over all the roots of the target heap.
    ///       <br/>
    ///     - Add each root R to the linked list of roots of the source heap and increases the total items count of 
    ///       the source heap by the number of items in R, which can be calculated without traversing, from the number 
    ///       of immediate children c of R as 2^c, being R a binomial tree.
    ///       tree.
    ///       <br/>
    ///     - For each added root, <see cref=" RaiseRootMerged(TreeNode{T})"/> is invoked.
    ///       <br/>
    ///     - Then binomial heap shape is restored by merging together all trees with the same degree and a new linear 
    ///       scan of the root is done, to update the reference to the root with highest priority, exactly as in 
    ///       <see cref="Push(T, int)"/> and <see cref="Pop"/>. 
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - For this analysis, events, and in particular <see cref="RaiseRootMerged(TreeNode{T})"/>, are considered
    ///       O(1) both in Time and Space Complexity.
    ///       <br/>
    ///     - The number of roots of the target heap is logarithmic with the number m of items in the target heap.
    ///       <br/>
    ///     - Adding each root R of the target heap to the forest of the source heap and increasing the items count are
    ///       both constant-time operations.
    ///       <br/>
    ///     - Housekeeping operations, done after that on the source heap, take logarithmic time, as explained in 
    ///       <see cref="Pop"/>.
    ///       <br/>
    ///     - Clearing the target is also a constant-time operation.
    ///       <br/>
    ///     - Therefore, Time and Space Complexity are O(log(m)).
    ///     </para>
    /// </remarks>
    public virtual void Merge(BinomialHeapPriorityQueue<T> targetPriorityQueue)
    {
        foreach (var targetRoot in targetPriorityQueue.Roots)
        {
            AttachToRoots(targetRoot);
            ItemsCount += (int)Math.Pow(2, targetRoot.Children.Count);
            RaiseRootMerged(targetRoot);
        }

        MergeEquiDegreeTrees();
        UpdateMaxRootsListNode();

        targetPriorityQueue.Clear();
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// First, calls <see cref="RaiseItemsClearing"/>.
    /// <br/>
    /// Then, removes all the trees from the forest, reset to the items count to 0 and set the reference to the 
    /// max priority root to <see langword="null"/>.
    /// <br/>
    /// The internal push timestamp counter is not reset. Therefore, new pushes after the clear will receive push
    /// timestamps strictly higher than the ones assigned to the items in the queue before the clear.
    /// <br/>
    /// Time and Space Complexity are O(1), with <see cref="RaiseItemsClearing"/> assumed to be O(1).
    /// </remarks>
    public virtual void Clear()
    {
        RaiseItemsClearing();

        Roots.Clear();

        ItemsCount = 0;
        MaxRootsListNode = null;
    }

    #endregion

    #region Hooks

    /// <summary>
    /// Invoked just after an item has been pushed into the heap (as a root).
    /// </summary>
    /// <param name="newRoot">The <see cref="TreeNode{T}"/> added to the heap.</param>
    /// <remarks>
    /// Not invoked on merging, for which <see cref="RaiseRootMerged(TreeNode{T})"/> is invoked instead.
    /// </remarks>
    protected virtual void RaiseItemPushed(TreeNode<T> newRoot) { }

    /// <summary>
    /// Invoked just before an item is removed from the heap.
    /// </summary>
    /// <param name="root">The <see cref="TreeNode{T}"/> about to be removed from the heap.</param>
    protected virtual void RaiseItemPopping(TreeNode<T> root) { }

    /// <summary>
    /// Invoked just before the all the items in the heap are wiped out.
    /// </summary>
    protected virtual void RaiseItemsClearing() { }

    /// <summary>
    /// Invoked just after an heap tree has been added to the forest (root and all its descendants).
    /// </summary>
    /// <param name="root">The root <see cref="TreeNode{T}"/> of the heap tree added to the forest.</param>
    protected virtual void RaiseRootMerged(TreeNode<T> root) { }

    #endregion

    #region Helpers

    /// <summary>
    /// Updates the reference to the max priority root to the provided <paramref name="rootsListNode"/>,
    /// if that root has a higher priority than the value of the current max priority root.
    /// </summary>
    /// <param name="rootsListNode">The root whose priority has been increased.</param>
    protected void UpdateMaxRootsListNodeAfterRootNewOrIncrease(LinkedListNode<TreeNode<T>> rootsListNode)
    {
        if (MaxRootsListNode == null ||
            MaxRootsListNode.Value.PrioritizedItem.CompareTo(rootsListNode.Value.PrioritizedItem) < 0)
            MaxRootsListNode = rootsListNode;
    }

    /// <summary>
    /// Performs a linear scan of the roots and update the <see cref="MaxRootsListNode"/> with a reference to the root
    /// of max priority.
    /// </summary>
    protected void UpdateMaxRootsListNode()
    {
        if (ItemsCount > 0)
        {
            LinkedListNode<TreeNode<T>> maxRootNode = Roots.First!;
            foreach (var rootNode in Roots.AsNodes().Skip(1))
            {
                if (rootNode.Value.PrioritizedItem.CompareTo(maxRootNode.Value.PrioritizedItem) > 0)
                    maxRootNode = rootNode;
            }

            MaxRootsListNode = maxRootNode;
        }
        else
        {
            MaxRootsListNode = null;
        }
    }

    /// <summary>
    /// Adds a brand new <see cref="TreeNode{T}"/> to the heap, as a new root in the forest.
    /// </summary>
    /// <param name="newRoot">The new root.</param>
    protected void AddRoot(TreeNode<T> newRoot)
    {
        var newRootsListNode = AttachToRoots(newRoot);
        ItemsCount++;
        UpdateMaxRootsListNodeAfterRootNewOrIncrease(newRootsListNode);
    }

    /// <summary>
    /// Attaches the provided <see cref="TreeNode{T}"/> to the <see cref="Roots"/>.
    /// </summary>
    /// <param name="newRoot">A node with no parent, and not already a root.</param>
    /// <returns>
    /// The <see cref="LinkedListNode{T}"/> of <see cref="Roots"/> pointing to the <paramref name="newRoot"/>.
    /// </returns>
    protected LinkedListNode<TreeNode<T>> AttachToRoots(TreeNode<T> newRoot)
    {
        var newRootsListNode = Roots.AddLast(newRoot);
        newRoot.RootsListNode = newRootsListNode;
        return newRootsListNode;
    }

    /// <summary>
    /// Merges all trees of the <see cref="Roots"/> forest with the same degree (number of children of the root).
    /// </summary>
    protected virtual void MergeEquiDegreeTrees()
    {
        var degrees = new Dictionary<int, LinkedListNode<TreeNode<T>>>();
        foreach (var rootsListNode in Roots.AsNodes().ToList())
        {
            var currentRootsListNode = rootsListNode;
            var currentRootsListNodeDegree = currentRootsListNode.Value.Children.Count;
            while (degrees.TryGetValue(currentRootsListNodeDegree, out var otherRoot))
            {
                var newRootsListNode = MergeRoots(currentRootsListNode, otherRoot);
                degrees.Remove(currentRootsListNodeDegree);

                currentRootsListNodeDegree = newRootsListNode.Value.Children.Count;
                currentRootsListNode = newRootsListNode;
            }

            degrees[currentRootsListNodeDegree] = currentRootsListNode;
        }
    }

    /// <summary>
    /// Merges the two provided trees of the forest into a single one, preserving the heap property.
    /// </summary>
    /// <param name="first">
    /// The <see cref="LinkedListNode{T}"/> of <see cref="Roots"/> pointing to the root of the first tree to merge.
    /// </param>
    /// <param name="second">
    /// The <see cref="LinkedListNode{T}"/> of <see cref="Roots"/> pointing to the root of the second tree to merge.
    /// </param>
    /// <returns>
    /// The <see cref="LinkedListNode{T}"/> of <see cref="Roots"/> pointing to the root of the merged tree: either
    /// <paramref name="first"/> or <paramref name="second"/>, depending on the <see cref="PrioritizedItem{T}"/> stored
    /// in the <see cref="TreeNode{T}.PrioritizedItem"/> of <see cref="LinkedListNode{T}.Value"/>.
    /// </returns>
    protected LinkedListNode<TreeNode<T>> MergeRoots(
        LinkedListNode<TreeNode<T>> first, LinkedListNode<TreeNode<T>> second)
    {
        if (first.Value.PrioritizedItem.CompareTo(second.Value.PrioritizedItem) >= 0)
        {
            DetachFromRoots(second);
            first.Value.AddChild(second.Value);
            return first;
        }

        DetachFromRoots(first);
        second.Value.AddChild(first.Value);
        return second;
    }

    /// <summary>
    /// Detaches the <see cref="TreeNode{T}"/> pointed by the provided <paramref name="rootsListNode"/> from the 
    /// <see cref="Roots"/>.
    /// </summary>
    /// <param name="rootsListNode">
    /// The <see cref="LinkedListNode{T}"/> of <see cref="Roots"/>, pointing to the <see cref="TreeNode{T}"/> root.
    /// </param>
    /// <returns>The detached <see cref="TreeNode{T}"/> root.</returns>
    protected TreeNode<T> DetachFromRoots(LinkedListNode<TreeNode<T>> rootsListNode)
    {
        var oldRoot = rootsListNode.Value;
        oldRoot.RootsListNode = null;
        Roots.Remove(rootsListNode);

        return oldRoot;
    }

    /// <summary>
    /// Promotes the provided <see cref="TreeNode{T}"/>, to being a root, detaching it from 
    /// its current parent.
    /// </summary>
    /// <param name="child">A child of a node of a tree in the forest.</param>
    protected virtual void PromoteChildToRoot(TreeNode<T> child)
    {
        child.DetachFromParent();
        AttachToRoots(child);
    }

    #endregion
}
