﻿namespace StringAlgorithms.SuffixStructures;

using MoreLinq;
using static StringUtilities;

/// <summary>
/// Exposes utility methods to match a <see cref="Build(TextWithTerminator)"/> against a 
/// <see cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}"/> concretion. 
/// </summary>
public static class SuffixStructureMatcher
{
    /// <summary>
    /// Tries to match a pattern against a Suffix Tree built on a text.
    /// </summary>
    /// <param name="node">The root of the Suffix Tree, to match the suffix of text against.</param>
    /// <param name="text">The text whose Suffix Tree has to be matched against the pattern.</param>
    /// <param name="text">The pattern to match. Unlike text, is a string without terminator.</param>
    /// <returns>A successful or non-successful match.</returns>
    public static SuffixStructureMatch<TPath> Match<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructureNode<TEdge, TNode, TPath, TBuilder> node, 
        TextWithTerminator text, 
        string pattern) 
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new() =>
        Match(node, text, pattern, 0);

    private static SuffixStructureMatch<TPath> Match<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructureNode<TEdge, TNode, TPath, TBuilder> node, 
        TextWithTerminator text, 
        string pattern, 
        int textStart)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new()
    {
        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException("Must be non-null and non-emtpy.", nameof(pattern));

        var longestMatch = (
            from edge in node.Children.Keys
            let length = LongestPrefixInCommon(edge.Of(text), pattern[textStart..])
            select new { Length = length, Edge = edge })
            .MaxBy(r => r.Length)
            .FirstOrDefault();

        var builder = new TBuilder();

        if (longestMatch == null)
            return new SuffixStructureMatch<TPath>(
                false, textStart, 0, builder.EmptyPath());

        if (longestMatch.Length == pattern.Length - textStart)
            return new SuffixStructureMatch<TPath>(
                true, textStart + longestMatch.Edge.Start, longestMatch.Length, builder.EmptyPath());

        // The edge has been fully matched but pattern is longer => chars left to match
        var childNode = node.Children[longestMatch.Edge];
        var childMatch = Match(childNode, text, pattern, textStart + longestMatch.Length);
        var pathToChild = builder.SingletonPath(longestMatch.Edge, childNode);
        return new SuffixStructureMatch<TPath>(
            childMatch.Success, 
            textStart + longestMatch.Edge.Start, 
            longestMatch.Length + childMatch.MatchedChars,
            pathToChild.Concat(childMatch.Path));
    }
}