namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// Builds objects, such as edges, nodes and paths, for the 
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}"/> concretion it is the builder of.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific structure.</typeparam>
/// <remarks>
/// This interface allows to have a shared construction interface for objects among all structures.
/// It is a workaround to the limitation of not having constructor signatures in interfaces.
/// See https://codeblog.jonskeet.uk/2008/08/29/lessons-learned-from-protocol-buffers-part-4-static-interfaces/
/// </remarks>
public interface IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>
{
    #region TPath building

    /// <summary>
    /// Builds an empty path, i.e. an empty sequence of nodes.
    /// </summary>
    TPath EmptyPath();

    /// <summary>
    /// Builds a path composed of a single node with its incoming edge.
    /// </summary>
    /// <param name="edge">The edge leading to the node.</param>
    /// <param name="node">The node defining the singleton path.</param>
    TPath SingletonPath(TEdge edge, TNode node);

    /// <summary>
    /// Builds a path composed of the provided couples of edges and nodes.
    /// </summary>
    /// <param name="pathNodes">An array of couples (edge, node).</param>
    TPath MultistepsPath(params (TEdge edge, TNode node)[] pathNodes);

    /// <summary>
    /// Builds a path composed of the provided sequence of key-value pairs of edges and nodes.
    /// </summary>
    /// <param name="pathNodes">A sequence of key-value pairs (edge, node).</param>
    TPath MultistepsPath(IEnumerable<KeyValuePair<TEdge, TNode>> pathNodes);

    #endregion
}
