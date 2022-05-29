namespace MoreStructures.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructureEdge{TEdge, TNode}"/> edge concretions.
/// </summary>
public static class SuffixStructureEdgeExtensions
{
    /// <summary>
    /// Whether the first edge is in adjacency order w.r.t. the second edge.
    /// </summary>
    /// <param name="first">The edge to be compared for adjacency.</param>
    /// <param name="second">The edge to compare the first edge against.</param>
    /// <param name="order">The adjacency relationship order to use for comparison.</param>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <returns>True if the specified adjacency relationship is respected.</returns>
    public static bool IsAdjacentTo<TEdge, TNode>(
        this ISuffixStructureEdge<TEdge, TNode> first,
        TEdge second,
        AdjacencyOrders order = AdjacencyOrders.BeforeOrAfter)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> =>
        (order.HasFlag(AdjacencyOrders.Before) && first.Start + first.Length == second.Start) ||
        (order.HasFlag(AdjacencyOrders.After) && second.Start + second.Length == first.Start);
}
