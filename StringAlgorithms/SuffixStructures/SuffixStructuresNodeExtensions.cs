namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Extension methods for all Suffix Structure node concretions.
/// </summary>
public static class SuffixStructuresNodeExtensions
{
    /// <summary>
    /// Whether the provided node is a leaf (i.e. has no children), or not.
    /// </summary>
    /// <param name="node">The node whose children have to be checked.</param>
    public static bool IsLeaf<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructureNode<TEdge, TNode, TPath, TBuilder> node)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new() => node.Children.Count == 0;

    /// <summary>
    /// Returns all paths from the provided node to a leaf.
    /// </summary>
    /// <param name="node">The node, to start the structure traversal from.</param>
    /// <returns>A sequence of pairs of node and its incoming edge.</returns>
    public static IEnumerable<TPath> GetAllNodeToLeafPaths<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructureNode<TEdge, TNode, TPath, TBuilder> node)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new()
    {
        var builder = new TBuilder();
        foreach (var edgeAndChild in node.Children)
        {
            var childToLeafPaths = edgeAndChild.Value.GetAllNodeToLeafPaths();
            if (childToLeafPaths.Any())
            {
                foreach (var childToLeafPath in childToLeafPaths)
                    yield return builder.SingletonPath(edgeAndChild.Key, edgeAndChild.Value).Concat(childToLeafPath);
            }
            else
            {
                yield return builder.SingletonPath(edgeAndChild.Key, edgeAndChild.Value);
            }
        }
    }

    /// <summary>
    /// Returns all suffixes for the provided text from the node down the structure, up to leaves.
    /// </summary>
    /// <param name="node">The node, to start the structure traversal from.</param>
    /// <param name="text">The text with terminator, whose suffixes have to be extracted.</param>
    /// <returns>A sequence of strings, each one being a suffix.</returns>
    public static IEnumerable<string> GetAllSuffixesFor<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructureNode<TEdge, TNode, TPath, TBuilder> node,
        TextWithTerminator text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new() =>
        node.GetAllNodeToLeafPaths().Select(rootToLeafPath => rootToLeafPath.SuffixFor(text));
}
