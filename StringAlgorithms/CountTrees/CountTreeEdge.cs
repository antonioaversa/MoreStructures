using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.CountTrees;

/// <summary>
/// An implementation of <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TPath, TBuilder}"/>, wrapping another 
/// implementation of <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TPath, TBuilder}"/>, and linking two 
/// instances of wrapper nodes <see cref="CountTreeNode{TEdge, TNode, TPath, TBuilder}"/>.
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
/// <param name="WrappedEdge">The edge being wrapped, pointing to descendants which are going to be counted.</param>
/// <remarks>
/// Due to records semantics, two <see cref="CountTreeEdge{TEdge, TNode, TPath, TBuilder}"/> instances wrapping the
/// same underlying edge, or two equivalent edges, will be equal. 
/// </remarks>
public sealed record CountTreeEdge<TEdge, TNode, TPath, TBuilder>(TEdge WrappedEdge) 
    : IRecImmDictIndexedTreeEdge<
        CountTreeEdge<TEdge, TNode, TPath, TBuilder>, 
        CountTreeNode<TEdge, TNode, TPath, TBuilder>, 
        CountTreePath<TEdge, TNode, TPath, TBuilder>, 
        CountTreeBuilder<TEdge, TNode, TPath, TBuilder>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>, new();
