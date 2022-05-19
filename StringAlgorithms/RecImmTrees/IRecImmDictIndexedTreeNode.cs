namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// The node of a generic Tree Structure recursively defined and whose nodes are indexed in an immutable dictionary of 
/// edges. Has no specific use case in mind and can be a root node, an intermediate node or a leaf node.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific structure.</typeparam>
public interface IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>
{
    /// <summary>
    /// A readonly view of the children private collection of this node. Empty for leaves.
    /// </summary>
    IDictionary<TEdge, TNode> Children { get; }
}
