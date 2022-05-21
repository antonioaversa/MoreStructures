using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructureNode{TEdge, TNode, TBuilder}"/> node concretions.
/// </summary>
public static class SuffixStructureNodeExtensions
{
    /// <summary>
    /// Returns all suffixes for the provided text from the node down the 
    /// <see cref="ISuffixStructureNode{TEdge, TNode, TBuilder}"/>, up to leaves.
    /// </summary>
    /// <param name="node">The node, to start the structure traversal from.</param>
    /// <param name="text">The text with terminator, whose suffixes have to be extracted.</param>
    /// <returns>A sequence of strings, each one being a suffix.</returns>
    public static IEnumerable<string> GetAllSuffixesFor<TEdge, TNode, TBuilder>(
        this ISuffixStructureNode<TEdge, TNode, TBuilder> node,
        TextWithTerminator text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TBuilder>, new() =>
        node.GetAllNodeToLeafPaths().Select(rootToLeafPath => rootToLeafPath.SuffixFor(text));
}
