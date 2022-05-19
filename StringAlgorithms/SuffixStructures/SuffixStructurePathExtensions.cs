using System.Text;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructurePath{TEdge, TNode, TPath, TBuilder}"/> path concretions.
/// </summary>
public static class SuffixStructurePathExtensions
{
    /// <summary>
    /// Calculate the suffix corresponding to this path on the provided terminator-including text.
    /// </summary>
    /// <param name="path">The path to traverse to build the suffix.</param>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>A string containing the suffix.</returns>
    public static string SuffixFor<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructurePath<TEdge, TNode, TPath, TBuilder> path,
        TextWithTerminator text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new() => 
        path.PathNodes
            .Aggregate(new StringBuilder(), (acc, node) => acc.Append(text[node.Key]))
            .ToString();

    /// <summary>
    /// Whether this path identifies a suffix of the provided text.
    /// </summary>
    /// <param name="path">The path, identifying a segment of the provided text.</param>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>True if the segment of text is also a suffix the text.</returns>
    public static bool IsSuffixOf<TEdge, TNode, TPath, TBuilder>
        (this ISuffixStructurePath<TEdge, TNode, TPath, TBuilder> path, 
        TextWithTerminator text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new() =>
        text.EndsWith(path.SuffixFor(text));
}
