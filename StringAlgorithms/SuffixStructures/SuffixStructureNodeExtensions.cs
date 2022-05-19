using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}"/> node concretions.
/// </summary>
public static class SuffixStructureNodeExtensions
{
    /// <summary>
    /// Returns all suffixes for the provided text from the node down the 
    /// <see cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}"/>, up to leaves.
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
