using StringAlgorithms.RecImmTrees;
using StringAlgorithms.Utilities;

namespace StringAlgorithms.CountTrees;

/// <summary>
/// An implementation of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/>, wrapping another
/// implementation of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/>, and counting the total
/// number of descendands the wrapped node has below (node itself excluded).
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <param name="WrappedNode">The node being wrapped, and whose descendants are going to be counted.</param>
/// <remarks>
/// Due to records semantics, and the use of value readonly dictionaries, two 
/// <see cref="CountTreeNode{TEdge, TNode}"/> instances wrapping the same underlying node, or two 
/// equivalent nodes, will be equal. 
/// </remarks>
public sealed record CountTreeNode<TEdge, TNode>(TNode WrappedNode)
    : IRecImmDictIndexedTreeNode<
        CountTreeEdge<TEdge, TNode>,
        CountTreeNode<TEdge, TNode>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <inheritdoc/>
    public IDictionary<
        CountTreeEdge<TEdge, TNode>,
        CountTreeNode<TEdge, TNode>> Children { get; init; } =
            Wrap(WrappedNode).ToValueReadOnlyDictionary();

    /// <summary>
    /// A private readonly object used for synchronization across multiple calls on the same instance.
    /// </summary>
    /// <remarks>
    /// A <see cref="ValueReadOnlyCollection{T}"/> is used not to break value equality of this record.
    /// </remarks>
    private readonly ValueReadOnlyCollection<object> _descendantsCountLock = new(Array.Empty<object>());
    
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

            lock (_descendantsCountLock)
            {
                if (_descendantsCount == null)
                    _descendantsCount = Children.Count + Children.Select(c => c.Value.DescendantsCount).Sum();
                return _descendantsCount.Value;
            }
        }
    }

    private static IEnumerable<KeyValuePair<
        CountTreeEdge<TEdge, TNode>, 
        CountTreeNode<TEdge, TNode>>> Wrap(TNode wrappedNode) =>
        from edgeAndChild in wrappedNode.Children
        let childCountTreeEdge = new CountTreeEdge<TEdge, TNode>(edgeAndChild.Key)
        let childCountTreeNode = new CountTreeNode<TEdge, TNode>(edgeAndChild.Value)
        select KeyValuePair.Create(childCountTreeEdge, childCountTreeNode);
}
