namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// A path of a Suffix Structure, a sequence of nodes and their incoming edges. Represents multi-step pattern matching.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific Suffix Structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific Suffix Structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific Suffix Structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific Suffix Structure.</typeparam>
public interface ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
{
    /// <summary>
    /// A readonly view of the private collection of path nodes.
    /// </summary>
    IEnumerable<KeyValuePair<TEdge, TNode>> PathNodes { get; }
}
