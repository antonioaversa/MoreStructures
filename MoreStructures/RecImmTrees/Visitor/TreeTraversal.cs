namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="IVisitStrategy{TNode, TVisitContext}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// Base class for all tree traversal strategies, such as <see cref="DepthFirstTraversal{TEdge, TNode}"/> and 
/// <see cref="BreadthFirstTraversal{TEdge, TNode}"/> strategies, which are different strategies of traversing a
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> structure top-down.
/// </summary>
public abstract class TreeTraversal<TEdge, TNode>
    : IVisitStrategy<TNode, TreeTraversalContext<TEdge, TNode>>
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

    /// <inheritdoc/>
    /// <example>
    ///     <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="/example"/>
    /// </example>
    public abstract void Visit(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor);
}
