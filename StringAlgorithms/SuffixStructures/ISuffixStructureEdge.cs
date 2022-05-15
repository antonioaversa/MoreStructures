namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// An edge of a Suffix Structure, directionally linking two nodes and forming a path hop. Represents prefix matching.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific Suffix Structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific Suffix Structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific Suffix Structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific Suffix Structure.</typeparam>
public interface ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder> : TextWithTerminator.ISelector
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
{
    /// <summary>
    /// The index of the first character of the edge string in the text.
    /// </summary>
    int Start { get; }

    /// <summary>
    /// The length of the edge string.
    /// </summary>
    int Length { get; }
}
