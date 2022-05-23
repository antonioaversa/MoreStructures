using MoreStructures.RecImmTrees;

namespace MoreStructures.CountTrees;

/// <summary>
/// An implementation of <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}"/>, wrapping another 
/// implementation of <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}"/>, and linking two 
/// instances of wrapper nodes <see cref="CountTreeNode{TEdge, TNode}"/>.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <param name="WrappedEdge">The edge being wrapped, pointing to descendants which are going to be counted.</param>
/// <remarks>
/// Due to records semantics, two <see cref="CountTreeEdge{TEdge, TNode}"/> instances wrapping the
/// same underlying edge, or two equivalent edges, will be equal. 
/// </remarks>
public sealed record CountTreeEdge<TEdge, TNode>(TEdge WrappedEdge) 
    : IRecImmDictIndexedTreeEdge<CountTreeEdge<TEdge, TNode>, CountTreeNode<TEdge, TNode>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>;
