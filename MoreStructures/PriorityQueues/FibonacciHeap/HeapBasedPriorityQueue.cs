using MoreStructures.Utilities;
using System.Collections;

namespace MoreStructures.PriorityQueues.FibonacciHeap;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on a Fibonacci Max Heap of its items.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - This heap implementation prioritises performance of <see cref="Push"/> and update operations, 
///       over <see cref="Pop"/>.
///       <br/>
///     - It does so by taking advantage of both the linked list layout and the tree layout, and taking the best of 
///       both worlds. 
///       <br/>
///     - Implementations based on linked or array lists are really fast at insertion, because they don't internally 
///       keep the data sorted. However, extraction becomes expensive, for the very same reason that data is unsorted.
///       <br/>
///     - At the other end of the spectrum, trees are logarithmic at insertion, because they have to keep data at least
///       partially sorted, at all time. Extraction, however, is very cheap.
///       <br/>
///     - So the underlying idea behind Fibonacci heap is to combine linked lists and trees, and represent the data as 
///       a forest of n-ry heap trees.
///       <br/>
///     - In doing so, both <see cref="Push(T, int)"/> and update becomes extremely fast, running in O(1) and O(1) 
///       amortized, respectively.
///       <br/>
///     - However, unlike <see cref="ArrayList.ArrayListBasedPriorityQueue{T}"/>, <see cref="Pop"/> doesn't become a
///       O(n) operation. Instead, it retains logarithmic runtime.
///       <br/>
///     - This proves to be the best compromise for applications such as the Dijkstra algorithm (implemented in 
///       <see cref="Graphs.ShortestDistance.DijkstraShortestDistanceFinder"/>), which uses a priority queue to find
///       the next best vertex.
///       <br/>
///     - Dijkstra algorithm performs O(e) <see cref="Extensions.UpdatablePriorityQueueExtensions.PushOrUpdate{T}"/>
///       operations and O(v) <see cref="Pop"/> operations, where e and v are the number of edges and vertices in the 
///       graph, respectively. 
///       <br/>
///     - In dense graphs e is O(v^2), so the number of push/update operations is way higher than the number of pop
///       operations, and it makes sense to optimize the former, at the cost of the latter.
///     </para>
/// </remarks>
public class HeapBasedPriorityQueue<T> : IPriorityQueue<T>
    where T : notnull
{
    /// <summary>
    /// A node of a tree, root or non-root, in the underlying forest representing the heap.
    /// </summary>
    protected class TreeNode
    {
        /// <summary>
        /// The item of type <typeparamref name="T"/>, with its priority and push timestamp.
        /// </summary>
        public PrioritizedItem<T> PrioritizedItem { get; set; }

        /// <summary>
        /// A <see cref="LinkedList{T}"/> of the children of this node. Empty if leaf.
        /// </summary>
        public LinkedList<TreeNode> Children { get; private set; } = new();

        /// <summary>
        /// A back-reference to the parent node. Null if a root.
        /// </summary>
        public TreeNode? Parent { get; set; } = null;

        /// <summary>
        /// A back-reference to the <see cref="LinkedListNode{T}"/> wrapper, in the <see cref="LinkedList{T}"/> of 
        /// tree roots in the underlying forest representing the heap. Null if not a root.
        /// </summary>
        public LinkedListNode<TreeNode>? RootsListNode { get; set; } = null;

        /// <summary>
        /// A back-reference to the <see cref="LinkedListNode{T}"/> wrapper, in the <see cref="LinkedList{T}"/> of
        /// children of the <see cref="Parent"/> of this node. Null if a root.
        /// </summary>
        public LinkedListNode<TreeNode>? ParentListNode { get; set; } = null;

        /// <summary>
        /// Add the provides <paramref name="treeNode"/> to the <see cref="Children"/> of this instance.
        /// </summary>
        /// <param name="treeNode">The <see cref="TreeNode"/> instance to become a child.</param>
        public void AddChild(TreeNode treeNode)
        {
            if (treeNode.Parent != null || treeNode.ParentListNode != null)
                throw new InvalidOperationException($"{nameof(treeNode)} cannot be already a child of another node.");
            if (treeNode.RootsListNode != null)
                throw new InvalidOperationException($"{nameof(treeNode)} cannot be a root.");

            treeNode.Parent = this;
            treeNode.ParentListNode = Children.AddLast(treeNode);
        }

        /// <summary>
        /// Removes this node from the <see cref="Children"/> of its <see cref="Parent"/>.
        /// </summary>
        public void DetachFromParent()
        {
            if (Parent == null || ParentListNode == null)
                throw new InvalidOperationException($"This node must be child of a node.");
            if (RootsListNode != null)
                throw new InvalidOperationException("Incoherent state: node both a child and a root.");

            Parent.Children.Remove(ParentListNode!);

            Parent = null;
            ParentListNode = null;
        }

        /// <summary>
        /// Deep copies this <see cref="TreeNode"/> and its entire structure.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="TreeNode"/>, pointing to a new, separate but equivalent structure.
        /// </returns>
        /// <remarks>
        /// It doesn't copy <see cref="Parent"/> for the top-level <see cref="TreeNode"/>, nor its 
        /// <see cref="RootsListNode"/> or <see cref="ParentListNode"/>: those have to be set, according to the 
        /// scenario, by the caller of <see cref="DeepCopy"/>.
        /// </remarks>
        public TreeNode DeepCopy()
        {
            var copy = new TreeNode { PrioritizedItem = PrioritizedItem };

            foreach (var childCopy in Children.Select(c => c.DeepCopy()))
                copy.AddChild(childCopy);

            return copy;
        }
    }

    /// <summary>
    /// The total number of items in this queue.
    /// </summary>
    private int _count;

    /// <summary>
    /// A <see cref="LinkedList{T}"/> of all the roots of the forest representing the heap.
    /// </summary>
    protected LinkedList<TreeNode> Roots { get; }

    /// <summary>
    /// Contains the <see cref="PrioritizedItem{T}"/> whose <see cref="TreeNode"/> have lost a child since last reset,
    /// and will be promoted to roots next time they will lose a child.
    /// </summary>
    protected HashSet<PrioritizedItem<T>> Losers { get; }

    /// <summary>
    /// Reference to the tree root in the forest with the highest priority. Makes Peek O(1).
    /// </summary>
    protected LinkedListNode<TreeNode>? MaxRootsListNode { get; set; }

    /// <summary>
    /// A non-negative, zero-based, monotonically strictly increasing counter, incremented at every insertion into this 
    /// data structure by a <see cref="Push(T, int)"/>.
    /// </summary>
    protected int CurrentPushTimestamp { get; set; }


    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    public HeapBasedPriorityQueue()
    {
        Roots = new();
        Losers = new();

        _count = 0;
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
    public HeapBasedPriorityQueue(HeapBasedPriorityQueue<T> source)
    {
        Roots = new LinkedList<TreeNode>(source.Roots.Select(r => r.DeepCopy()));
        foreach (var rootsListNode in Roots.AsNodes())
            rootsListNode.Value.RootsListNode = rootsListNode;

        Losers = new HashSet<PrioritizedItem<T>>(source.Losers);

        _count = source._count;
        UpdateMaxRootsListNode();
        CurrentPushTimestamp = source.CurrentPushTimestamp;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks the count internally stored, keeping track of the sum of the size of all trees in the linked list.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count => _count;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// In order to return items in heap order (i.e. the max at each step), it first copies this priority queue into a
    /// second temporary queue, which can be mutated without affecting the state of this queue.
    /// <br/>
    /// It then iterates over the copy, calling <see cref="Pop"/> until it becomes empty.
    /// <br/>
    /// Time Complexity is O(n * log(n)) (when fully enumerated), because a single <see cref="Pop"/> on a Fibonacci 
    /// Heap takes logarithmic time, and there are n items to be extracted.
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
    /// Peeks the item of the linked list pointed by the "max root" internal property.
    /// <br/>
    /// By transitivity, the item of the max root contains the max item in the queue, since all roots are non-smaller
    /// than their descendants.
    /// <br/>
    /// Therefore, Time and Space Complexity are O(1).
    /// </remarks>
    public PrioritizedItem<T> Peek()
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
    ///     - If LLN has higher priority than the root with max priority, updates the reference to the root with max 
    ///       priority, to point to LLN.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Adding a new root to the forest of trees is a constant-time operation, since the root is added at
    ///       the end of the linked list representing the forest, which keeps references to both ends of the chain of 
    ///       nodes.
    ///       <br/>
    ///     - Therefore, Time Complexity and Space Complexity are O(1).
    ///       <br/>
    ///     - Notice that this operation executes in constant time at the cost of adding a new shallow tree to the 
    ///       forest. After n <see cref="Push(T, int)"/> consecutive operations, the forest would be made of n trees
    ///       of degree 0, which is the shape of a simple heap based on a linked list.
    ///       <br/>
    ///     - This cost has to be beared by <see cref="Pop"/>, which runs in logarithmic time but merges trees of the
    ///       same degree and does so in batch. Performing such an operation in batch is key to the performance gain of
    ///       a Fibonacci Heap, w.r.t. an ArrayList or LinkedList solution.
    ///     </para>
    /// </remarks> 
    public void Push(T item, int priority)
    {
        var prioritizedItem = new PrioritizedItem<T>(item, priority, CurrentPushTimestamp++);
        AddRoot(new TreeNode { PrioritizedItem = prioritizedItem });
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
    ///     - In a Fibonacci Heap the max degree of the roots of the forest is logarithmic with the total number of 
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
    public PrioritizedItem<T> Pop()
    {
        if (MaxRootsListNode == null)
            throw new InvalidOperationException($"Can't {nameof(Pop)} on an empty queue.");

        var oldRoot = DetachFromRoots(MaxRootsListNode);
        _count--;

        foreach (var child in oldRoot.Children.ToList())
            PromoteChildToRoot(child);

        MergeEquiDegreeTrees();
        UpdateMaxRootsListNode();

        return oldRoot.PrioritizedItem;
    }

    /// <summary>
    /// Merges all trees of the <see cref="Roots"/> forest with the same degree (number of children of the root).
    /// </summary>
    protected virtual void MergeEquiDegreeTrees()
    {
        var degrees = new Dictionary<int, LinkedListNode<TreeNode>>();
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
    /// in the <see cref="TreeNode.PrioritizedItem"/> of <see cref="LinkedListNode{T}.Value"/>.
    /// </returns>
    private LinkedListNode<TreeNode> MergeRoots(
        LinkedListNode<TreeNode> first, LinkedListNode<TreeNode> second)
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
    /// Promotes the provided <see cref="TreeNode"/>, to being a root, detaching it from its current parent. 
    /// Also, resets the "loser" flag.
    /// </summary>
    /// <param name="child">A child of a node of a tree in the forest.</param>
    protected virtual void PromoteChildToRoot(TreeNode child)
    {
        child.DetachFromParent();
        AttachToRoots(child);
        Losers.Remove(child.PrioritizedItem);
    }

    /// <summary>
    /// Adds a brand new <see cref="TreeNode"/> to the heap, as a new root in the forest.
    /// </summary>
    /// <param name="newRoot">The new root.</param>
    private void AddRoot(TreeNode newRoot)
    {
        var newRootsListNode = AttachToRoots(newRoot);
        _count++;
        if (MaxRootsListNode == null ||
            MaxRootsListNode.Value.PrioritizedItem.CompareTo(newRoot.PrioritizedItem) < 0)
            MaxRootsListNode = newRootsListNode;
    }

    /// <summary>
    /// Attaches the provided <see cref="TreeNode"/> to the <see cref="Roots"/>.
    /// </summary>
    /// <param name="newRoot">A node with no parent, and not already a root.</param>
    /// <returns>
    /// The <see cref="LinkedListNode{T}"/> of <see cref="Roots"/> pointing to the <paramref name="newRoot"/>.
    /// </returns>
    private LinkedListNode<TreeNode> AttachToRoots(TreeNode newRoot)
    {
        var newRootsListNode = Roots.AddLast(newRoot);
        newRoot.RootsListNode = newRootsListNode;
        return newRootsListNode;
    }

    /// <summary>
    /// Detaches the <see cref="TreeNode"/> pointed by the provided <paramref name="rootsListNode"/> from the 
    /// <see cref="Roots"/>.
    /// </summary>
    /// <param name="rootsListNode">
    /// The <see cref="LinkedListNode{T}"/> of <see cref="Roots"/>, pointing to the <see cref="TreeNode"/> root.
    /// </param>
    /// <returns>The detached <see cref="TreeNode"/> root.</returns>
    private TreeNode DetachFromRoots(LinkedListNode<TreeNode> rootsListNode)
    {
        var oldRoot = rootsListNode.Value;
        oldRoot.RootsListNode = null;
        Roots.Remove(rootsListNode);

        return oldRoot;
    }

    /// <summary>
    /// Performs a linear scan of the roots and update the <see cref="MaxRootsListNode"/> with a reference to the root
    /// of max priority.
    /// </summary>
    protected virtual void UpdateMaxRootsListNode()
    {
        if (_count > 0)
        {
            LinkedListNode<TreeNode> maxRootNode = Roots.First!;
            foreach (var rootNode in Roots.AsNodes())
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
}
