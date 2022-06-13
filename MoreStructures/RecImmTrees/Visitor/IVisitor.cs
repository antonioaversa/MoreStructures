namespace MoreStructures.RecImmTrees.Visitor;

/// <summary>
/// A visit strategy of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> structures. Can be any way of moving 
/// through the structure and touching nodes: partial or exaustive, hierarchical or random, upwards or downwards, etc.
/// </summary>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <typeparam name="TVisitContext">
/// The type of visit context. Depends on the <see cref="IVisitStrategy{TNode, TVisitContext}"/> used.
/// </typeparam>
public interface IVisitStrategy<TNode, TVisitContext>
{
    /// <summary>
    /// <b>Lazily</b> visits the structure of the provided <paramref name="node"/>, returning an enumerable of the 
    /// sequence of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure,
    /// in the order defined by this <see cref="IVisitStrategy{TNode, TVisitContext}"/>.
    /// </summary>
    /// <param name="node">The node on where to start visit the structure.</param>
    /// <returns>
    /// A sequence emitting (node, visit context) couples, in the visit order defined by the visit strategy.
    /// </returns>
    IEnumerable<TVisitContext> Visit(TNode node);
}
