using MoreStructures.RecImmTrees;
using MoreStructures.Utilities;
using System.Text;

namespace MoreStructures.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="TreePath{TEdge, TNode}"/> used in 
/// <see cref="ISuffixStructureNode{TEdge, TNode}"/> structures.
/// </summary>
public static class SuffixStructureTreePathExtensions
{
    /// <summary>
    /// Calculate the suffix corresponding to this path on the provided terminator-including text.
    /// </summary>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <param name="path">The path to traverse to build the suffix.</param>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>A <see cref="IValueEnumerable{T}"/> sequence of <see cref="char"/> containing the suffix.</returns>
    public static IValueEnumerable<char> SuffixFor<TEdge, TNode>(
        this TreePath<TEdge, TNode> path,
        TextWithTerminator text)
        where TEdge : TextWithTerminator.ISelector => 

        path.PathNodes
            .SelectMany(node => text[node.Key])
            .AsValue();

    /// <summary>
    /// Whether this path identifies a suffix of the provided text.
    /// </summary>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <param name="path">The path, identifying a segment of the provided text.</param>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>True if the segment of text is also a suffix the text.</returns>
    public static bool IsSuffixOf<TEdge, TNode>
        (this TreePath<TEdge, TNode> path, 
        TextWithTerminator text)
        where TEdge : TextWithTerminator.ISelector =>

        text.EndsWith(path.SuffixFor(text));

    /// <summary>
    /// Whether the provided <paramref name="path"/> includes at least once, on any node of the path, the provided
    /// <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <param name="path">The path to walk, looking for <paramref name="index"/>.</param>
    /// <param name="index">The index of the char of the text, to look for. Must be non-negative.</param>
    /// <returns>A boolean.</returns>
    public static bool ContainsIndex<TEdge, TNode>(
        this TreePath<TEdge, TNode> path,
        int index)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> => 

        index >= 0
            ? path.PathNodes.Any(pathNode => pathNode.Key.ContainsIndex(index))
            : throw new ArgumentOutOfRangeException(nameof(index), "Must be non-negative.");

    /// <summary>
    /// Whether the provided <paramref name="path"/> has at least a node starting at a index lower or equal than the 
    /// provided <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <param name="path">The path to walk, looking for <paramref name="index"/>.</param>
    /// <param name="index">The index of the char of the text, to look for. Must be non-negative.</param>
    /// <returns>A boolean.</returns>
    public static bool ContainsIndexesNonBiggerThan<TEdge, TNode>(
        this TreePath<TEdge, TNode> path,
        int index)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> =>

        index >= 0
            ? path.PathNodes.Any(pathNode => pathNode.Key.ContainsIndexesNonBiggerThan(index))
            : throw new ArgumentOutOfRangeException(nameof(index), "Must be non-negative.");
}
