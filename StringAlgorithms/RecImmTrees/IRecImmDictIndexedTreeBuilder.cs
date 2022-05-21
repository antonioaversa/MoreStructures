namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// Builds objects, such as edges and nodes, for the <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TBuilder}"/> 
/// concretion it is the builder of.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TBuilder">The type of builder for the specific structure.</typeparam>
/// <remarks>
/// This interface allows to have a shared construction interface for objects among all structures.
/// It is a workaround to the limitation of not having constructor signatures in interfaces.
/// See https://codeblog.jonskeet.uk/2008/08/29/lessons-learned-from-protocol-buffers-part-4-static-interfaces/
/// </remarks>
public interface IRecImmDictIndexedTreeBuilder<TEdge, TNode, TBuilder>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TBuilder>
{
}
