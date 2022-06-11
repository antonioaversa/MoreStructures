namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A fully-recursive, depth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which goes in depth as far as possible along each path of the tree, only backtracking when a 
/// leaf is reached.
/// </summary>
/// <remarks>
///     <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
/// </remarks>
public class FullyRecursiveDepthFirstTraversal<TEdge, TNode> 
    : DepthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <inheritdoc 
    ///     cref="TreeTraversal{TEdge, TNode}.Visit(TNode, Visitor{TNode, TreeTraversalContext{TEdge, TNode}})" 
    ///     path="//*[not(self::summary)]"/>
    /// <summary>
    /// Recursively visits the structure of the provided<paramref name= "node" />, calling the provided 
    /// <paramref name="visitor"/> on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, 
    /// in depth-first order.
    /// </summary>
    public override void Visit(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor) => 
        Visit(node, visitor, default, default);

    private void Visit(
        TNode node,
        Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor, 
        TNode? parentNode,
        TEdge? parentEdge)
    {
        switch (TraversalOrder)
        {
            case TreeTraversalOrder.ParentFirst:
                visitor(node, new(parentNode, parentEdge));
                foreach (var child in ChildrenSorter(node.Children))
                    Visit(child.Value, visitor, node, child.Key);

                break;

            case TreeTraversalOrder.ChildrenFirst:
                foreach (var child in ChildrenSorter(node.Children))
                    Visit(child.Value, visitor, node, child.Key);
                visitor(node, new(parentNode, parentEdge));

                break;

            default:
                throw new NotSupportedException($"{nameof(TreeTraversalOrder)} {TraversalOrder} is not supported");
        }
    }
}
