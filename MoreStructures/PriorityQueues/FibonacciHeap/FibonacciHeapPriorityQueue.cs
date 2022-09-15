using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.PriorityQueues.FibonacciHeap;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on a Fibonacci Max Heap of its items.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     - A <b>Fibonacci Heap</b> is a <b>Binomial Heap</b> which has its <b>heap max property</b> and <b>binomial 
///       layout</b> invariants always satisfied, but can have <b>unicity of degree</b> temporarily invalidated 
///       (until next <see cref="BinomialHeapPriorityQueue{T}.Pop"/> operation).
///       <br/>
///     - Nodes of the heaps can be flagged as <b>losers</b>, meaning that they have lost at least once a children,
///       after its promotion to being a new root.
///     </para>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - This heap implementation is a Binomial Heap refinement which prioritises performance of <see cref="Push"/> 
///       and update operations over <see cref="BinomialHeapPriorityQueue{T}.Pop"/>.
///       <br/>
///     - It does so by taking advantage of both the linked list layout and the tree layout, pretty much like
///       Binomial Heaps. 
///       <br/>
///     - Implementations based on linked or array lists are really fast at insertion, because they don't internally 
///       keep the data sorted. However, extraction becomes expensive, for the very same reason that data is unsorted.
///       <br/>
///     - At the other end of the spectrum, trees are logarithmic at insertion, because they have to keep data at least
///       partially sorted, at all time. Extraction, however, is very cheap.
///       <br/>
///     - So the underlying idea behind Fibonacci Heaps is to combine linked lists and trees, and represent the data as 
///       a forest of n-ry heap trees, exactly like Binomial Heaps, and exploit the easy insertion in linked list
///       (which is not implemented by the standard Push into a Binomial Heap).
///       <br/>
///     - In doing so, both <see cref="Push(T, int)"/> and update becomes extremely fast, running in O(1) and O(1) 
///       amortized, respectively.
///       <br/>
///     - However, unlike <see cref="ArrayList.ArrayListPriorityQueue{T}"/>, 
///       <see cref="BinomialHeapPriorityQueue{T}.Pop"/> doesn't become a O(n) operation. Instead, it retains 
///       logarithmic runtime.
///       <br/>
///     - This proves to be the best compromise for applications such as the Dijkstra algorithm (implemented in 
///       <see cref="Graphs.ShortestDistance.DijkstraShortestDistanceFinder"/>), which uses a priority queue to find
///       the next best vertex.
///       <br/>
///     - Dijkstra algorithm performs O(e) <see cref="Extensions.UpdatablePriorityQueueExtensions.PushOrUpdate{T}"/>
///       operations and O(v) <see cref="BinomialHeapPriorityQueue{T}.Pop"/> operations, where e and v are the number 
///       of edges and vertices in the graph, respectively. 
///       <br/>
///     - In dense graphs e is O(v^2), so the number of push/update operations is way higher than the number of pop
///       operations, and it makes sense to optimize the former, at the cost of the latter.
///     </para>
/// </remarks>
public partial class FibonacciHeapPriorityQueue<T> : BinomialHeapPriorityQueue<T>
    where T : notnull
{
    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    public FibonacciHeapPriorityQueue()
        :base()
    {
    }

    /// <summary>
    /// Builds a deep, separate copy of the provided <paramref name="source"/> priority queue.
    /// </summary>
    /// <param name="source">The priority queue to be copied over.</param>
    /// <remarks>
    /// Doesn't copy the items themselves, it only deep-copies the internal structure of the <paramref name="source"/>
    /// queue.
    /// </remarks>
    protected FibonacciHeapPriorityQueue(FibonacciHeapPriorityQueue<T> source)
        :base(source)
    {
    }

    #region Public API

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// In order to return items in heap order (i.e. the max at each step), it first copies this priority queue into a
    /// second temporary queue, which can be mutated without affecting the state of this queue.
    /// <br/>
    /// It then iterates over the copy, calling <see cref="BinomialHeapPriorityQueue{T}.Pop"/> until it becomes empty.
    /// <br/>
    /// Time Complexity is O(n * log(n)) (when fully enumerated), because a single 
    /// <see cref="BinomialHeapPriorityQueue{T}.Pop"/> on a Fibonacci Heap takes logarithmic time, and there are n 
    /// items to be extracted.
    /// <br/>
    /// Space Complexity is O(n), as a copy of this queue is required as auxiliary data structure to emit elements in 
    /// the right order of priority.
    /// </remarks> 
    public override IEnumerator<T> GetEnumerator()
    {
        var copy = new FibonacciHeapPriorityQueue<T>(this);
        while (copy.Count > 0)
            yield return copy.Pop().Item;
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
    ///       <br/>
    ///     - Unlike <see cref="BinomialHeapPriorityQueue{T}.Push(T, int)"/>, it doesn't perform any rebalancing of
    ///       the forest just yet, meaning that the binomial property may be temporarily violated until the next 
    ///       <see cref="BinomialHeapPriorityQueue{T}.Pop"/> operation.
    ///       <br/>
    ///     - The delay of "housekeeping" operations to restore the shape property of a binomial heap, and the grouping
    ///       of such "housekeeping" operations into a single batch, executed at the next 
    ///       <see cref="BinomialHeapPriorityQueue{T}.Pop"/>, is one of the core efficiency mechanisms of the
    ///       Fibonacci Heap.
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
    ///     - This cost has to be beared by <see cref="BinomialHeapPriorityQueue{T}.Pop"/>, which runs in logarithmic 
    ///       time but merges trees of the same degree and does so in batch.
    ///     </para>
    /// </remarks> 
    public override void Push(T item, int priority)
    {
        var prioritizedItem = new PrioritizedItem<T>(item, priority, CurrentPushTimestamp++, PushTimestampEras[^1]);
        var newRoot = new TreeNode<T> { PrioritizedItem = prioritizedItem };
        AddRoot(newRoot);
        RaiseItemPushed(newRoot);
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Promotes the provided <see cref="TreeNode{T}"/>, to being a root, detaching it from its current parent. 
    /// Also, resets the "loser" flag of the child (behavior specific to Fibonacci Heaps).
    /// </summary>
    /// <param name="child">A child of a node of a tree in the forest.</param>
    protected override void PromoteChildToRoot(TreeNode<T> child)
    {
        base.PromoteChildToRoot(child);
        child.IsALoser = false;
    }

    #endregion
}
