namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="IVisitStrategy{TEdge, TNode, TVisitContext}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A fully-recursive, depth-first <see cref="IVisitStrategy{TEdge, TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which goes in depth as far as possible along each path of the tree, only backtracking when a 
/// leaf is reached.
/// </summary>
/// <remarks>
///     <inheritdoc cref="IVisitStrategy{TEdge, TNode, TVisitContext}" path="/remarks"/>
///     <para>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
/// </remarks>
public class FullyRecursiveDepthFirstTraversal<TEdge, TNode> 
    : IVisitStrategy<TEdge, TNode, TreeTraversalContext<TEdge, TNode>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <summary>
    /// The traversal order between parent and its children, to be applied when visiting the tree. By default 
    /// <see cref="TreeTraversalOrder.ParentFirst"/> is applied, meaning that the parent node is visited before
    /// its children.
    /// </summary>
    public TreeTraversalOrder TraversalOrder { get; init; } = TreeTraversalOrder.ParentFirst;

    /// <summary>
    /// The order of visit of the children. By default <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}.Children"/>
    /// is returned as is, and no specific order is imposed to the sequence of (edge, node) couples, during the visit.
    /// </summary>
    /// <remarks>
    /// Specifying a well-defined, deterministic order ensures that children are visited in a consistent and 
    /// reproducible way across executions of the visit.
    /// </remarks>
    public Func<IEnumerable<KeyValuePair<TEdge, TNode>>, IEnumerable<KeyValuePair<TEdge, TNode>>> ChildrenSorter 
    { 
        get; 
        set; 
    } = children => children;

    /// <inheritdoc 
    ///     cref="IVisitStrategy{TEdge, TNode, TVisitContext}.Visit(TNode, Visitor{TNode, TVisitContext})" 
    ///     path="//*[not(self::summary)]"/>
    /// <summary>
    /// Visits the structure of the provided<paramref name= "node" />, calling the provided<paramref name="visitor"/>
    /// on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, in depth-first order.
    /// </summary>
    public void Visit(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor) => 
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