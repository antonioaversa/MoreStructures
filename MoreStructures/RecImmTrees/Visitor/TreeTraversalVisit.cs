namespace MoreStructures.RecImmTrees.Visitor;

/// <summary>
/// The emitted information of a root-to-leaf traversal of a tree composed of 
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> nodes and 
/// <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}"/> edges.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <param name="Node">The visited node.</param>
/// <param name="Context">The contextual information associated with the visit of the node.</param>
public record TreeTraversalVisit<TEdge, TNode>(TNode Node, TreeTraversalContext<TEdge, TNode> Context)
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>;