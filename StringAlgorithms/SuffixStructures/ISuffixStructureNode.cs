namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// The node of a Suffix Structure, linked by edges and forming paths. Represents pattern matching state.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific Suffix Structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific Suffix Structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific Suffix Structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific Suffix Structure.</typeparam>
public interface ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
{
    /// <summary>
    /// A readonly view of the children private collection of this node. Empty for leaves.
    /// </summary>
    IDictionary<TEdge, TNode> Children { get; }

    /// <summary>
    /// The index of the character, the path from the root leading to this leaf starts with. Non-null for leaves only.
    /// </summary>
    int? Start { get; }
}
