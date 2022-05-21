namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// An edge of a <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/>, directionally linking two 
/// nodes and forming a path hop.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
public interface IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
}
