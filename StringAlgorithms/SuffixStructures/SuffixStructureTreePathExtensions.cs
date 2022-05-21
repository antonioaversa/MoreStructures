using StringAlgorithms.RecImmTrees;
using System.Text;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="TreePath{TEdge, TNode}"/> used in 
/// <see cref="ISuffixStructureNode{TEdge, TNode}"/> structures.
/// </summary>
public static class SuffixStructureTreePathExtensions
{
    /// <summary>
    /// Calculate the suffix corresponding to this path on the provided terminator-including text.
    /// </summary>
    /// <param name="path">The path to traverse to build the suffix.</param>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>A string containing the suffix.</returns>
    public static string SuffixFor<TEdge, TNode>(
        this TreePath<TEdge, TNode> path,
        TextWithTerminator text)
        where TEdge : TextWithTerminator.ISelector => 
        path.PathNodes
            .Aggregate(new StringBuilder(), (acc, node) => acc.Append(text[node.Key]))
            .ToString();

    /// <summary>
    /// Whether this path identifies a suffix of the provided text.
    /// </summary>
    /// <param name="path">The path, identifying a segment of the provided text.</param>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>True if the segment of text is also a suffix the text.</returns>
    public static bool IsSuffixOf<TEdge, TNode>
        (this TreePath<TEdge, TNode> path, 
        TextWithTerminator text)
        where TEdge : TextWithTerminator.ISelector =>
        text.EndsWith(path.SuffixFor(text));
}
