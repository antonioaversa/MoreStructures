namespace MoreStructures.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructureEdge{TEdge, TNode}"/> edge concretions.
/// </summary>
public static class SuffixStructureEdgeExtensions
{
    /// <summary>
    /// Whether <paramref name="first"/> is in adjacency order w.r.t. <paramref name="second"/>, acoording to the 
    /// provided <paramref name="order"/>.
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

    /// <summary>
    /// Whether the provided <paramref name="edge"/> contains the provided <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <param name="edge">The edge to check.</param>
    /// <param name="index">The index of the char of the text, to look for. Must be non-negative.</param>
    /// <returns>A boolean.</returns>
    public static bool ContainsIndex<TEdge, TNode>(
        this ISuffixStructureEdge<TEdge, TNode> edge,
        int index)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> =>

        index >= 0
        ? edge.Start <= index && index < edge.Start + edge.Length
        : throw new ArgumentOutOfRangeException(nameof(index), "Must be non-negative.");

    /// <summary>
    /// Whether the provided <paramref name="edge"/> has a <see cref="ISuffixStructureNode{TEdge, TNode}.Start"/> 
    /// lower or equal than the provided <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <param name="edge">The edge to check.</param>
    /// <param name="index">The index of the char of the text, to look for. Must be non-negative.</param>
    /// <returns>A boolean.</returns>
    public static bool ContainsIndexesNonBiggerThan<TEdge, TNode>(
        this ISuffixStructureEdge<TEdge, TNode> edge,
        int index)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> =>

        index >= 0
        ? edge.Start <= index
        : throw new ArgumentOutOfRangeException(nameof(index), "Must be non-negative.");
}
