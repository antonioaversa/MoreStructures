using StringAlgorithms.RecImmTrees;
using StringAlgorithms.Utilities;

namespace StringAlgorithms.CountTrees;

/// <summary>
/// A path of a <see cref="CountTreeNode{TEdge, TNode, TPath, TBuilder}"/>, a sequence of nodes and their incoming 
/// edges <see cref="CountTreeEdge{TEdge, TNode, TPath, TBuilder}"/>. 
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
/// <param name="WrappedPath">>The path being wrapped.</param>
public sealed record CountTreePath<TEdge, TNode, TPath, TBuilder>(TPath WrappedPath)
    : IRecImmDictIndexedTreePath<
        CountTreeEdge<TEdge, TNode, TPath, TBuilder>,
        CountTreeNode<TEdge, TNode, TPath, TBuilder>,
        CountTreePath<TEdge, TNode, TPath, TBuilder>,
        CountTreeBuilder<TEdge, TNode, TPath, TBuilder>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>, new()
{
    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<
        CountTreeEdge<TEdge, TNode, TPath, TBuilder>,
        CountTreeNode<TEdge, TNode, TPath, TBuilder>>> PathNodes =>
        Wrap(WrappedPath).ToValueReadOnlyCollection();

    private static IEnumerable<KeyValuePair<
        CountTreeEdge<TEdge, TNode, TPath, TBuilder>,
        CountTreeNode<TEdge, TNode, TPath, TBuilder>>> Wrap(TPath wrappedPath) =>
        from edgeAndChild in wrappedPath.PathNodes
        let childCountTreeEdge = new CountTreeEdge<TEdge, TNode, TPath, TBuilder>(edgeAndChild.Key)
        let childCountTreeNode = new CountTreeNode<TEdge, TNode, TPath, TBuilder>(edgeAndChild.Value)
        select KeyValuePair.Create(childCountTreeEdge, childCountTreeNode);
}
