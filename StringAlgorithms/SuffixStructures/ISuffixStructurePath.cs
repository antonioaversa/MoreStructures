using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// A path of a <see cref="ISuffixStructureEdge{TEdge, TNode, TPath, TBuilder}"/>, a sequence of nodes and their 
/// incoming edges <see cref="ISuffixStructureEdge{TEdge, TNode, TPath, TBuilder}"/>. Represents multi-step pattern 
/// matching.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreePath{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreePath{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <typeparam name="TPath">
///     <inheritdoc cref="IRecImmDictIndexedTreePath{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TPath']"/>
/// </typeparam>
/// <typeparam name="TBuilder">
///     <inheritdoc cref="IRecImmDictIndexedTreePath{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TBuilder']"/>
/// </typeparam>
public interface ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
{
}
