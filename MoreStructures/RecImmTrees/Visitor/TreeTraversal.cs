namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="IVisitStrategy{TNode, TVisitContext}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// Base class for all tree traversal strategies, such as <see cref="DepthFirstTraversal{TEdge, TNode}"/> and 
/// <see cref="BreadthFirstTraversal{TEdge, TNode}"/> strategies, which are different strategies of traversing a
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> structure top-down.
/// </summary>
/// <remarks>
///     <para id="traversal-vs-visit">
///     TRAVERSAL VS VISIT
///     <br/>
///     The word "traversal" in <see cref="TreeTraversal{TEdge, TNode}"/> and its derivations, is here used with 
///     narrower scope than the word "visit" in <see cref="IVisitStrategy{TNode, TVisitContext}"/>.
///     <br/>
///     - "Traversal" is used here as common class between DFS and BFS, as a visit strategy that starts from the root 
///       of the tree and proceeds downwards, following edges and terminating when leafs are reached.
///       <br/>
///     - "Visit" is used in a more general sense, as any algorithm which "touches" 0 or more nodes of the tree, 
///       walking the tree in any possible way (up, down, sideways, ...).
///     </para>
/// </remarks>
public abstract class TreeTraversal<TEdge, TNode>
    : IVisitStrategy<TNode, TreeTraversalVisit<TEdge, TNode>>
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
    public Func<TreeTraversalVisit<TEdge, TNode>, IEnumerable<KeyValuePair<TEdge, TNode>>> ChildrenSorter
    {
        get;
        set;
    } = visit => visit.Node.Children;

    /// <inheritdoc/>
    /// <example>
    ///     <inheritdoc cref="BreadthFirstTraversal{TEdge, TNode}" path="/example"/>
    /// </example>
    public abstract IEnumerable<TreeTraversalVisit<TEdge, TNode>> Visit(TNode node);
}
