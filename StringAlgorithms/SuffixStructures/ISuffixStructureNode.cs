using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// The node of a Suffix Structure, linked by edges and forming paths. Represents pattern matching state.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <typeparam name="TBuilder">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TBuilder']"/>
/// </typeparam>
public interface ISuffixStructureNode<TEdge, TNode, TBuilder>
    : IRecImmDictIndexedTreeNode<TEdge, TNode>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TBuilder>
{
    /// <summary>
    /// The index of the character, the path from the root leading to this leaf starts with. Non-null for leaves only.
    /// </summary>
    int? Start { get; }
}
