using MoreStructures.RecImmTrees;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructureNode{TEdge, TNode}"/> node concretions.
/// </summary>
public static class SuffixStructureNodeExtensions
{
    /// <summary>
    /// Returns all suffixes for the provided text from the node down the 
    /// <see cref="ISuffixStructureNode{TEdge, TNode}"/>, up to leaves.
    /// </summary>
    /// <param name="node">The node, to start the structure traversal from.</param>
    /// <param name="text">The text with terminator, whose suffixes have to be extracted.</param>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <returns>A sequence of <see cref="IValueEnumerable{T}"/>, each one being a suffix.</returns>
    public static IValueEnumerable<IValueEnumerable<char>> GetAllSuffixesFor<TEdge, TNode>(
        this ISuffixStructureNode<TEdge, TNode> node,
        TextWithTerminator text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> =>
        node
            .GetAllNodeToLeafPaths()
            .Select(rootToLeafPath => rootToLeafPath.SuffixFor(text))
            .AsValue();
}
