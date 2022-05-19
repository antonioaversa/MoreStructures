using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// The node of a Suffix Structure, linked by edges and forming paths. Represents pattern matching state.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <typeparam name="TPath">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TPath']"/>
/// </typeparam>
/// <typeparam name="TBuilder">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TBuilder']"/>
/// </typeparam>
public interface ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
{
    /// <summary>
    /// The index of the character, the path from the root leading to this leaf starts with. Non-null for leaves only.
    /// </summary>
    int? Start { get; }
}
