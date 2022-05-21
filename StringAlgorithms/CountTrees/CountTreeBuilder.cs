using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.CountTrees;

/// <summary>
/// Builds objects, such as edges and nodes, for <see cref="CountTreeNode{TEdge, TNode, TBuilder}"/> structures.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TBuilder}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TBuilder}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <typeparam name="TBuilder">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TBuilder}" path="/typeparam[@name='TBuilder']"/>
/// </typeparam>
public sealed record CountTreeBuilder<TEdge, TNode, TBuilder>()
    : IRecImmDictIndexedTreeBuilder<
        CountTreeEdge<TEdge, TNode, TBuilder>,
        CountTreeNode<TEdge, TNode, TBuilder>,
        CountTreeBuilder<TEdge, TNode, TBuilder>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TBuilder>, new()
{
}