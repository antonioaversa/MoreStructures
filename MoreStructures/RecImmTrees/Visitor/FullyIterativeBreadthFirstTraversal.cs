namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A fully-iterative, breadth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which visits all the nodes at the current depth, along any path of the tree, before going 
/// deeper or shallower, exploring nodes with higher or lower depth.
/// </summary>
/// <remarks>
///     <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para>
///     Implemented fully recursively, so not limited by call stack depth but rather by the maximum size of the stack 
///     stored in the heap. Convenient with deep trees (i.e. trees having a height &gt; ~1K nodes).
///     </para>
/// </remarks>
public class FullyIterativeBreadthFirstTraversal<TEdge, TNode> 
    : BreadthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    private record struct Item(TNode? ParentNode, TEdge? IncomingEdge, TNode Node);

    /// <inheritdoc 
    ///     cref="TreeTraversal{TEdge, TNode}.Visit(TNode, Visitor{TNode, TreeTraversalContext{TEdge, TNode}})" 
    ///     path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// <b>Eagerly and iteratively</b> visits the structure of the provided <paramref name= "node" />, calling the 
    /// provided <paramref name="visitor"/> on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the 
    /// structure, in breadth-first order.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="FullyIterativeBreadthFirstTraversal{TEdge, TNode}" path="/remarks"/>
    ///     <para id="algo">
    ///     The algorithm perform a double walk:
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
    ///     - The second walk goes through the "visit" queue/stack, calling the visitor on each of the nodes.
    ///     </para>
    ///     <para id="complexity">
    ///     Each of the walk goes through all the n nodes and n - 1 edges of the tree. Each walk uses a O(1) insertion
    ///     and extraction data structure, which contains at most n elements of constant size (reference to the node,
    ///     reference to its parent, reference to its incoming edge).
    ///     <br/>
    ///     Time Complexity is O(n) for the first walk, when the visit queue/stack is populated and no actual node 
    ///     visit is performed, and O(n * Tv) for the second walk, when the actual visit of all nodes is performed,
    ///     where Tv is the Time Complexity of the visitor. So O(n * Tv) in total.
    ///     <br/>
    ///     Space Complexity is O(2n) for the first walk, due to the traversal and visit queue/stack being allocated 
    ///     and populated, and O(n * Sv) for the second walk, when the actual visit of all nodes is performed, where
    ///     Sv is the Space Complexity of the visitor. So O(n * Sv) in total.
    ///     </para>
    /// </remarks>
    public override void Visit(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        switch (TraversalOrder)
        {
            case TreeTraversalOrder.ParentFirst:
                {
                    var traversalQueue = new Queue<Item>();
                    traversalQueue.Enqueue(new(default, default, node));

                    var visitQueue = new Queue<Item>();

                    while (traversalQueue.Count > 0)
                        ProcessParentFirstTraversalQueue(traversalQueue, visitQueue);

                    while (visitQueue.Count > 0)
                        ProcessParentFirstVisitQueue(visitQueue, visitor);
                }

                break;
            
            case TreeTraversalOrder.ChildrenFirst:
                {
                    var traversalQueue = new Queue<Item>();
                    traversalQueue.Enqueue(new(default, default, node));

                    var visitStack = new Stack<Item>();

                    while (traversalQueue.Count > 0)
                        ProcessChildrenFirstTraversalQueue(traversalQueue, visitStack);

                    while (visitStack.Count > 0)
                        ProcessChildrenFirstVisitStack(visitStack, visitor);
                }

                break;

            default:
                throw new NotSupportedException($"{nameof(TraversalOrder)} {TraversalOrder} not supported.");
        }
    }

    private void ProcessParentFirstTraversalQueue(Queue<Item> traversalQueue, Queue<Item> visitQueue)
    {
        var (parentNode, incomingEdge, node) = traversalQueue.Dequeue();

        visitQueue.Enqueue(new(parentNode, incomingEdge, node));
        foreach (var child in ChildrenSorter(node.Children))
            traversalQueue.Enqueue(new(node, child.Key, child.Value));
    }

    private static void ProcessParentFirstVisitQueue(
        Queue<Item> visitQueue,
        Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        var (parentNode, incomingEdge, node) = visitQueue.Dequeue();

        visitor(node, new(parentNode, incomingEdge));
    }

    private void ProcessChildrenFirstTraversalQueue(Queue<Item> traversalQueue, Stack<Item> visitStack)
    {
        var (parentNode, incomingEdge, node) = traversalQueue.Dequeue();

        visitStack.Push(new(parentNode, incomingEdge, node));
        foreach (var child in ChildrenSorter(node.Children).Reverse())
            traversalQueue.Enqueue(new(node, child.Key, child.Value));
    }

    private static void ProcessChildrenFirstVisitStack(
        Stack<Item> visitStack,
        Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        var (parentNode, incomingEdge, node) = visitStack.Pop();

        visitor(node, new(parentNode, incomingEdge));
    }
}
