namespace MoreStructures.RecImmTrees.Visitor;

/// <summary>
/// The emitted information of a root-to-leaf traversal of a tree composed of 
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> nodes and 
/// <see cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}"/> edges.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <param name="Node">The visited node.</param>
/// <param name="ParentNode">The parent of the node being visited. Null when visiting the root node.</param>
/// <param name="IncomingEdge">
/// The edge outgoing from the parent, traversed to reach this node. Null when visiting the root node.
/// </param>
/// <param name="Level">
/// The level of visit: 0 is assigned to the top-level node, 1 to its children, 2 to its grandchildren, etc.
/// </param>
public record TreeTraversalVisit<TEdge, TNode>(TNode Node, TNode? ParentNode, TEdge? IncomingEdge, int Level)
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>;