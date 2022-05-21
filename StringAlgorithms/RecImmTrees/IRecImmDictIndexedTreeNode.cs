namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// The node of a generic Tree Structure recursively defined and whose nodes are indexed in an immutable dictionary of 
/// edges. Has no specific use case in mind and can be a root node, an intermediate node or a leaf node.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific structure.</typeparam>
public interface IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TBuilder>
{
    /// <summary>
    /// A readonly view of the children private collection of this node. Empty for leaves.
    /// </summary>
    IDictionary<TEdge, TNode> Children { get; }
}
