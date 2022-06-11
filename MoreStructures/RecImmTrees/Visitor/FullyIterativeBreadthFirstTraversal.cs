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
    ///     cref="BreadthFirstTraversal{TEdge, TNode}.Visit(TNode, Visitor{TNode, TreeTraversalContext{TEdge, TNode}})" 
    ///     path="//*[not(self::summary)]"/>
    /// <summary>
    /// Iteratively visits the structure of the provided <paramref name= "node" />, calling the provided 
    /// <paramref name="visitor"/> on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, 
    /// in breadth-first order.
    /// </summary>
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

    private void ProcessParentFirstVisitQueue(
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

    private void ProcessChildrenFirstVisitStack(
        Stack<Item> visitStack,
        Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        var (parentNode, incomingEdge, node) = visitStack.Pop();

        visitor(node, new(parentNode, incomingEdge));
    }
}
