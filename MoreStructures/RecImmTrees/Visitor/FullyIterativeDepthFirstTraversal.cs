namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A fully-iterative, depth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which goes in depth as far as possible along each path of the tree, only backtracking when a 
/// leaf is reached.
/// </summary>
/// <remarks>
///     <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para>
///     Implemented fully recursively, so not limited by call stack depth but rather by the maximum size of the stack 
///     stored in the heap. Convenient with deep trees (i.e. trees having a height &gt; ~1K nodes).
///     </para>
/// </remarks>
public class FullyIterativeDepthFirstTraversal<TEdge, TNode> 
    : DepthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    private record StackFrame(TNode? ParentNode, TEdge? IncomingEdge, TNode Node, bool ChildrenStacked);

    /// <inheritdoc 
    ///     cref="DepthFirstTraversal{TEdge, TNode}.Visit(TNode, Visitor{TNode, TreeTraversalContext{TEdge, TNode}})" 
    ///     path="//*[not(self::summary)]"/>
    /// <summary>
    /// Iteratively visits the structure of the provided <paramref name= "node" />, calling the provided 
    /// <paramref name="visitor"/> on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, 
    /// in depth-first order.
    /// </summary>
    public override void Visit(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        var stack = new Stack<StackFrame> { };
        stack.Push(new (default, default, node, false));
        while (stack.Count > 0)
        {
            ProcessStack(stack, visitor);
        }
    }

    private void ProcessStack(Stack<StackFrame> stack, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        var (parentNode, incomingEdge, node, childrenStacked) = stack.Pop();

        if (node.IsLeaf())
        {
            switch (TraversalOrder)
            {
                case TreeTraversalOrder.ParentFirst:
                case TreeTraversalOrder.ChildrenFirst:
                    visitor(node, new(parentNode, incomingEdge));
                    break;

                default:
                    throw new NotSupportedException($"{nameof(TreeTraversalOrder)} {TraversalOrder} is not supported");
            }

            return;
        }

        if (!childrenStacked)
        {
            switch (TraversalOrder)
            {
                case TreeTraversalOrder.ParentFirst:
                    foreach (var child in ChildrenSorter(node.Children).Reverse())
                        stack.Push(new(node, child.Key, child.Value, false));
                    stack.Push(new(parentNode, incomingEdge, node, true));

                    break;

                case TreeTraversalOrder.ChildrenFirst:
                    stack.Push(new(parentNode, incomingEdge, node, true));
                    foreach (var child in ChildrenSorter(node.Children).Reverse())
                        stack.Push(new(node, child.Key, child.Value, false));

                    break;

                default:
                    throw new NotSupportedException($"{nameof(TreeTraversalOrder)} {TraversalOrder} is not supported");
            }

            return;
        }

        visitor(node, new(parentNode, incomingEdge));
    }
}