namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// Extension methods for all <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode, TBuilder}"/> node concretions.
/// </summary>
public static class RecImmDictIndexedTreeNodeExtensions
{
    /// <summary>
    /// Whether the provided node is a leaf (i.e. has no children), or not.
    /// </summary>
    /// <param name="node">The node whose children have to be checked.</param>
    public static bool IsLeaf<TEdge, TNode, TBuilder>(
        this IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder> node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TBuilder>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder>
        where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TBuilder>, new() => 
        node.Children.Count == 0;

    /// <summary>
    /// Returns all paths from the provided node to a leaf.
    /// </summary>
    /// <param name="node">The node, to start the structure traversal from.</param>
    /// <returns>A sequence of pairs of node and its incoming edge.</returns>
    public static IEnumerable<TreePath<TEdge, TNode, TBuilder>> GetAllNodeToLeafPaths<TEdge, TNode, TBuilder>(
        this IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder> node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TBuilder>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TBuilder>
        where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TBuilder>, new()
    {
        foreach (var edgeAndChild in node.Children)
        {
            var childToLeafPaths = edgeAndChild.Value.GetAllNodeToLeafPaths();
            if (childToLeafPaths.Any())
            {
                foreach (var childToLeafPath in childToLeafPaths)
                    yield return new TreePath<TEdge, TNode, TBuilder>(edgeAndChild.Key, edgeAndChild.Value)
                        .Concat(childToLeafPath);
            }
            else
            {
                yield return new TreePath<TEdge, TNode, TBuilder>(edgeAndChild.Key, edgeAndChild.Value);
            }
        }
    }
}
