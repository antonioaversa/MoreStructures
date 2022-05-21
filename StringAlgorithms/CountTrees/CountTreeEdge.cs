using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.CountTrees;

/// <summary>
/// An implementation of <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TBuilder}"/>, wrapping another 
/// implementation of <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TBuilder}"/>, and linking two 
/// instances of wrapper nodes <see cref="CountTreeNode{TEdge, TNode, TBuilder}"/>.
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
/// <param name="WrappedEdge">The edge being wrapped, pointing to descendants which are going to be counted.</param>
/// <remarks>
/// Due to records semantics, two <see cref="CountTreeEdge{TEdge, TNode, TBuilder}"/> instances wrapping the
/// same underlying edge, or two equivalent edges, will be equal. 
/// </remarks>
public sealed record CountTreeEdge<TEdge, TNode, TBuilder>(TEdge WrappedEdge) 
    : IRecImmDictIndexedTreeEdge<
        CountTreeEdge<TEdge, TNode, TBuilder>, 
        CountTreeNode<TEdge, TNode, TBuilder>, 
        CountTreeBuilder<TEdge, TNode, TBuilder>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder>
    
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TBuilder>, new();
