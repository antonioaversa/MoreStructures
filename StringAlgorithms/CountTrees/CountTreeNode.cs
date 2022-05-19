using StringAlgorithms.RecImmTrees;
using StringAlgorithms.Utilities;

namespace StringAlgorithms.CountTrees;

/// <summary>
/// An implementation of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}"/>, wrapping another
/// implementation of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}"/>, and counting the total
/// number of descendands the wrapped node has below (node itself excluded).
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
/// <param name="WrappedNode">The node being wrapped, and whose descendants are going to be counted.</param>
/// <remarks>
/// Due to records semantics, and the use of value readonly dictionaries, two 
/// <see cref="CountTreeNode{TEdge, TNode, TPath, TBuilder}"/> instances wrapping the same underlying node, or two 
/// equivalent nodes, will be equal. 
/// </remarks>
public sealed record CountTreeNode<TEdge, TNode, TPath, TBuilder>(TNode WrappedNode)
    : IRecImmDictIndexedTreeNode<
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
    public IDictionary<
        CountTreeEdge<TEdge, TNode, TPath, TBuilder>, 
        CountTreeNode<TEdge, TNode, TPath, TBuilder>> Children { get; init; } = 
            Wrap(WrappedNode).ToValueReadOnlyDictionary();

    private int? _descendantsCount = null;

    /// <summary>
    /// The number of descendands below this node (node itself excluded).
    /// </summary>
    /// <remarks>
    /// Lazy evaluated and thread-safe.
    /// </remarks>
    public int DescendantsCount 
    {
        get 
        {
            if (_descendantsCount != null)
                return _descendantsCount.Value;

            lock (this)
            {
                if (_descendantsCount == null)
                    _descendantsCount = Children.Count + Children.Select(c => c.Value.DescendantsCount).Sum();
                return _descendantsCount.Value;
            }
        }
    }

    private static IEnumerable<KeyValuePair<
        CountTreeEdge<TEdge, TNode, TPath, TBuilder>, 
        CountTreeNode<TEdge, TNode, TPath, TBuilder>>> Wrap(TNode wrappedNode) =>
        from edgeAndChild in wrappedNode.Children
        let childCountTreeEdge = new CountTreeEdge<TEdge, TNode, TPath, TBuilder>(edgeAndChild.Key)
        let childCountTreeNode = new CountTreeNode<TEdge, TNode, TPath, TBuilder>(edgeAndChild.Value)
        select KeyValuePair.Create(childCountTreeEdge, childCountTreeNode);
}
