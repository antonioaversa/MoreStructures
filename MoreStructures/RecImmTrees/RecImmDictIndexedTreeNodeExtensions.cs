namespace MoreStructures.RecImmTrees;

/// <summary>
/// Extension methods for all <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> node concretions.
/// </summary>
public static class RecImmDictIndexedTreeNodeExtensions
{
    /// <summary>
    /// Whether the provided node is a leaf (i.e. has no children), or not.
    /// </summary>
    /// <param name="node">The node whose children have to be checked.</param>
    public static bool IsLeaf<TEdge, TNode>(
        this IRecImmDictIndexedTreeNode<TEdge, TNode> node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode> => 

        node.Children.Count == 0;
}
