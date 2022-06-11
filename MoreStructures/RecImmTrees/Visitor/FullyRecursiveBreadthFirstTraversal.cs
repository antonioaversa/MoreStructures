namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A fully-recursive, breadth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which visits all the nodes at the current depth, along any path of the tree, before going 
/// deeper or shallower, exploring nodes with higher or lower depth.
/// </summary>
/// <remarks>
///     <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
/// </remarks>
public class FullyRecursiveBreadthFirstTraversal<TEdge, TNode>
    : BreadthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <inheritdoc 
    ///     cref="TreeTraversal{TEdge, TNode}.Visit(TNode, Visitor{TNode, TreeTraversalContext{TEdge, TNode}})"
    ///     path="//*[not(self::summary)]" />
    /// <summary>
    /// Recursively visits the structure of the provided <paramref name= "node" />, calling the provided 
    /// <paramref name="visitor"/> on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, 
    /// in breadth-first order.
    /// </summary>
    public override void Visit(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        visitor(node, new(default, default));
        VisitR(node, visitor);
    }

    private void VisitR(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor)
    {
        foreach (var child in node.Children)
            visitor(child.Value, new(node, child.Key));

        foreach (var child in node.Children)
            Visit(child.Value, visitor);
    }
}