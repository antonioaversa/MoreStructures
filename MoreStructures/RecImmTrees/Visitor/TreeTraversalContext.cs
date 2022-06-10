namespace MoreStructures.RecImmTrees.Visitor;

/// <summary>
/// The contextual information in a root-to-leaf traversal of a tree composed of 
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> nodes and 
/// <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}"/> edges.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <param name="ParentNode">The parent of the node being visited. Null when visiting the root node.</param>
/// <param name="IncomingEdge">
/// The edge outgoing from the parent, traversed to reach this node. Null when visiting the root node.
/// </param>
public record TreeTraversalContext<TEdge, TNode>(TNode? ParentNode, TEdge? IncomingEdge)
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>;
