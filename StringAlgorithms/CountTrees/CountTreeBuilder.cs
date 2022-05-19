using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.CountTrees;

/// <summary>
/// Builds objects, such as edges, nodes and paths, for <see cref="CountTreeNode{TEdge, TNode, TPath, TBuilder}"/> 
/// structures.
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
public sealed record CountTreeBuilder<TEdge, TNode, TPath, TBuilder>()
    : IRecImmDictIndexedTreeBuilder<
        CountTreeEdge<TEdge, TNode, TPath, TBuilder>,
        CountTreeNode<TEdge, TNode, TPath, TBuilder>,
        CountTreePath<TEdge, TNode, TPath, TBuilder>,
        CountTreeBuilder<TEdge, TNode, TPath, TBuilder>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>, new()
{
    private static readonly TBuilder WrappedBuilder = new();

    /// <inheritdoc/>
    public CountTreePath<TEdge, TNode, TPath, TBuilder> EmptyPath() =>
        new(WrappedBuilder.EmptyPath());

    /// <inheritdoc/>
    public CountTreePath<TEdge, TNode, TPath, TBuilder> MultistepsPath(
        params (
            CountTreeEdge<TEdge, TNode, TPath, TBuilder> edge,
            CountTreeNode<TEdge, TNode, TPath, TBuilder> node)[] pathNodes) =>
        new(WrappedBuilder.MultistepsPath(
            pathNodes.Select(pn => (pn.edge.WrappedEdge, pn.node.WrappedNode)).ToArray()));

    /// <inheritdoc/>
    public CountTreePath<TEdge, TNode, TPath, TBuilder> MultistepsPath(
        IEnumerable<KeyValuePair<
            CountTreeEdge<TEdge, TNode, TPath, TBuilder>,
            CountTreeNode<TEdge, TNode, TPath, TBuilder>>> pathNodes) =>
        new(WrappedBuilder.MultistepsPath(
            pathNodes.Select(pn => KeyValuePair.Create(pn.Key.WrappedEdge, pn.Value.WrappedNode))));

    /// <inheritdoc/>
    public CountTreePath<TEdge, TNode, TPath, TBuilder> SingletonPath(
        CountTreeEdge<TEdge, TNode, TPath, TBuilder> edge, CountTreeNode<TEdge, TNode, TPath, TBuilder> node) =>
        new(WrappedBuilder.SingletonPath(edge.WrappedEdge, node.WrappedNode));
}