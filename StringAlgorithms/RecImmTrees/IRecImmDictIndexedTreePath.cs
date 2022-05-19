namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// A path of a <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}"/> structure, an immutable 
/// sequence of nodes and their incoming edges <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TPath, TBuilder}"/>.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific structure.</typeparam>
public interface IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>
{
    /// <summary>
    /// A readonly view of the private collection of path nodes.
    /// </summary>
    IEnumerable<KeyValuePair<TEdge, TNode>> PathNodes { get; }
}
