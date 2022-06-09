namespace MoreStructures.RecImmTrees.Visitor;

/// <summary>
/// The visit logic of a <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/>. It's not correlated to the 
/// strategy with which nodes are visited, which is defined by this 
/// <see cref="IVisitStrategy{TEdge, TNode, TVisitContext}"/>.
/// </summary>
/// <param name="node">The node being visited.</param>
/// <param name="context">
/// The context in which the visit is performed, such as the incoming edge traversed to reach the node or the parent
/// node, traversed at the previous iteration or recursive step.
/// <br/>
/// Depending on the actual <see cref="IVisitStrategy{TEdge, TNode, TVisitContext}"/> used, and how such visit traverse 
/// the data structure, the context may contain different data.
/// </param>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TVisitContext">
/// The type of visit context. Depends on the <see cref="IVisitStrategy{TEdge, TNode, TVisitContext}"/> used.
/// </typeparam>
public delegate void Visitor<TNode, in TVisitContext>(TNode node, TVisitContext context);

/// <summary>
/// A visit strategy of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> structures.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TVisitContext">
/// The type of visit context. Depends on the <see cref="IVisitStrategy{TEdge, TNode, TVisitContext}"/> used.
/// </typeparam>
public interface IVisitStrategy<TEdge, TNode, TVisitContext>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <summary>
    /// Visits the structure of the provided <paramref name="node"/>, calling the provided <paramref name="visitor"/>
    /// on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, in the order defined by this
    /// <see cref="IVisitStrategy{TEdge, TNode, TVisitContext}"/>.
    /// </summary>
    /// <param name="node">The node on where to start visit the structure.</param>
    /// <param name="visitor">The visit logic, invoked on each of the visit nodes.</param>
    void Visit(TNode node, Visitor<TNode, TVisitContext> visitor);
}
