namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// Extension methods for <see cref="TreePath{TEdge, TNode}"/>.
/// </summary>
public static class TreePathExtensions
{
    /// <summary>
    /// Builds a new path of nodes, appending the nodes of the second path to the first path.
    /// </summary>
    /// <param name="first">The path, to append nodes to.</param>
    /// <param name="second">The path, whose nodes have to be appended.</param>
    /// <returns>A new path, whose nodes are the concatenation of the nodes of the two paths.</returns>
    public static TreePath<TEdge, TNode> Concat<TEdge, TNode>(
        this TreePath<TEdge, TNode> first,
        TreePath<TEdge, TNode> second)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode> =>
        new(first.PathNodes.Concat(second.PathNodes));

    /// <summary>
    /// Append the provided node with its incoming edge to the provided path, bulding a new path.
    /// </summary>
    /// <param name="path">The path, to appended the node and the edge to.</param>
    /// <param name="edge">The edge, pointing to the node to be appended.</param>
    /// <param name="node">The node to be appended.</param>
    /// <returns>
    /// A new path, whose nodes are the concatenation of the nodes of the provided path and the one appended.
    /// </returns>
    public static TreePath<TEdge, TNode> Append<TEdge, TNode>(
        this TreePath<TEdge, TNode> path,
        TEdge edge,
        TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode> =>
        new(path.PathNodes.Concat(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1)));
}
