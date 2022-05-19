namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// An edge of a <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TPath, TBuilder}"/>, directionally linking two 
/// nodes and forming a path hop.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TPath">The type of paths of the specific structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific structure.</typeparam>
public interface IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>
{
}
