namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Builds objects, such as edges, nodes and paths, for the Suffix Structure concretion it is the builder of.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific Suffix Structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific Suffix Structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific Suffix Structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific Suffix Structure.</typeparam>
/// <remarks>
/// This interface allows to have a shared construction interface for objects among all Suffix Structure.
/// It is a workaround to the limitation of not having constructor signatures in interfaces.
/// See https://codeblog.jonskeet.uk/2008/08/29/lessons-learned-from-protocol-buffers-part-4-static-interfaces/
/// </remarks>
public interface ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
{
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
}
