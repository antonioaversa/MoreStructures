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
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>True if a suffix.</returns>
    public static bool IsSuffixOf<TEdge, TNode, TPath, TBuilder>
        (this ISuffixStructurePath<TEdge, TNode, TPath, TBuilder> path, 
        TextWithTerminator text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new() =>
        text.EndsWith(path.SuffixFor(text));

    /// <summary>
    /// Builds a new path of nodes, appending the nodes of the second path to the first path.
    /// </summary>
    /// <param name="first">The path, to append nodes to.</param>
    /// <param name="second">The path, whose nodes have to be appended.</param>
    /// <returns>A new path, whose nodes are the concatenation of the nodes of the two paths.</returns>
    public static TPath Concat<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructurePath<TEdge, TNode, TPath, TBuilder> first,
        ISuffixStructurePath<TEdge, TNode, TPath, TBuilder> second)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new() =>
        new TBuilder().MultistepsPath(first.PathNodes.Concat(second.PathNodes));

    /// <summary>
    /// Append the provided node with its incoming edge to the provided path, bulding a new path.
    /// </summary>
    /// <param name="edge">The path, to appended the node and the edge to.</param>
    /// <param name="edge">The edge, pointing to the node to be appended.</param>
    /// <param name="node">The node to be appended.</param>
    /// <returns>
    /// A new path, whose nodes are the concatenation of the nodes of the provided path and the one appended.
    /// </returns>
    public static TPath Append<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructurePath<TEdge, TNode, TPath, TBuilder> path,
        TEdge edge, 
        TNode node)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new () =>
        new TBuilder().MultistepsPath(path.PathNodes.Concat(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1)));
}
