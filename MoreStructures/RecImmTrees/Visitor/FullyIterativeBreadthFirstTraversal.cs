namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A lazy, fully-iterative, breadth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which visits all the nodes at the current depth, along any path of the tree, before going 
/// deeper or shallower, exploring nodes with higher or lower depth.
/// </summary>
/// <remarks>
///     <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Implemented fully iteratively, so not limited by call stack depth but rather by the maximum size of the stack 
///     stored in the heap.
///     <br/>
///     Convenient with deep trees (i.e. trees having a height &gt; ~1K nodes).
///     </para>
/// </remarks>
public class FullyIterativeBreadthFirstTraversal<TEdge, TNode> 
    : BreadthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    private record struct Item(TNode? ParentNode, TEdge? IncomingEdge, TNode Node, int Level);

    /// <inheritdoc 
    ///     cref="TreeTraversal{TEdge, TNode}.Visit(TNode)" 
    ///     path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// <b>Lazily and iteratively</b> visits the structure of the provided <paramref name= "node" />, returning the
    /// sequence of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, in breadth-first order.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="FullyIterativeBreadthFirstTraversal{TEdge, TNode}" path="/remarks"/>
    ///     <para id="algo">
    ///     ALGORITHM
    ///     <br/>
    ///     The algorithm performs a double walk:
    ///     <br/>    
    ///     - The first walk is of the nodes of the tree structure and always proceeds top-down, enqueuing each 
    ///       encountered child for each node into a "traversal" <see cref="Queue{T}"/>, which is used to reproduce the 
    ///       breadth-first order.
    ///       <br/> 
    ///     - The first walk also enqueues each encountered node into a "visit" <see cref="Queue{T}"/>, if the 
    ///       <see cref="TreeTraversal{TEdge, TNode}.TraversalOrder"/> is <see cref="TreeTraversalOrder.ParentFirst"/>, 
    ///       or it pushes it onto a "visit" <see cref="Stack{T}"/>, if it is 
    ///       <see cref="TreeTraversalOrder.ChildrenFirst"/>.
    ///       <br/> 
    ///     - The second walk goes through the "visit" queue/stack, yielding to the output sequence, so that the 
    ///       client code implementing the visitor can lazily process the nodes.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     Each of the walk goes through all the n nodes and n - 1 edges of the tree. 
    ///     <br/>
    ///     Each walk uses a O(1) insertion and extraction data structure, which contains at most n elements of 
    ///     constant size (reference to the node, reference to its parent, reference to its incoming edge).
    ///     <br/>
    ///     Time Complexity is O(n) for the first walk, when the visit queue/stack is populated and no actual node 
    ///     visit is performed, and O(n) for the second walk, when the actual visit of all nodes is performed. 
    ///     So O(n) in total.
    ///     <br/>
    ///     Space Complexity is O(2n) for the first walk, due to the traversal and visit queue/stack being allocated 
    ///     and populated, and O(n) for the second walk, when the actual visit of all nodes is performed. 
    ///     So O(n) in total.
    ///     </para>
    /// </remarks>
    public override IEnumerable<TreeTraversalVisit<TEdge, TNode>> Visit(TNode node)
    {
        switch (TraversalOrder)
        {
            case TreeTraversalOrder.ParentFirst:
                {
                    var traversalQueue = new Queue<Item>();
                    traversalQueue.Enqueue(new(default, default, node, 0));

                    var visitQueue = new Queue<Item>();

                    while (traversalQueue.Count > 0)
                        ProcessParentFirstTraversalQueue(traversalQueue, visitQueue);

                    while (visitQueue.Count > 0)
                    {
                        var (parentNode, incomingEdge, visitedNode, level) = visitQueue.Dequeue();
                        yield return new(visitedNode, new(parentNode, incomingEdge, level));
                    }
                }

                break;
            
            case TreeTraversalOrder.ChildrenFirst:
                {
                    var traversalQueue = new Queue<Item>();
                    traversalQueue.Enqueue(new(default, default, node, 0));

                    var visitStack = new Stack<Item>();

                    while (traversalQueue.Count > 0)
                        ProcessChildrenFirstTraversalQueue(traversalQueue, visitStack);

                    while (visitStack.Count > 0)
                    {
                        var (parentNode, incomingEdge, visitedNode, level) = visitStack.Pop();
                        yield return new(visitedNode, new(parentNode, incomingEdge, level));
                    }
                }

                break;

            default:
                throw new NotSupportedException($"{nameof(TraversalOrder)} {TraversalOrder} not supported.");
        }
    }

    private void ProcessParentFirstTraversalQueue(Queue<Item> traversalQueue, Queue<Item> visitQueue)
    {
        var (parentNode, incomingEdge, node, level) = traversalQueue.Dequeue();

        visitQueue.Enqueue(new(parentNode, incomingEdge, node, level));
        foreach (var child in ChildrenSorter(new(node, new(parentNode, incomingEdge, level))))
            traversalQueue.Enqueue(new(node, child.Key, child.Value, level + 1));
    }

    private void ProcessChildrenFirstTraversalQueue(Queue<Item> traversalQueue, Stack<Item> visitStack)
    {
        var (parentNode, incomingEdge, node, level) = traversalQueue.Dequeue();

        visitStack.Push(new(parentNode, incomingEdge, node, level));
        foreach (var child in ChildrenSorter(new(node, new(parentNode, incomingEdge, level))).Reverse())
            traversalQueue.Enqueue(new(node, child.Key, child.Value, level + 1));
    }
}
