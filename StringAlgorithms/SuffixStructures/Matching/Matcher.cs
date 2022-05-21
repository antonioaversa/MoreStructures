namespace StringAlgorithms.SuffixStructures.Matching;

using MoreLinq;
using StringAlgorithms.RecImmTrees;
using static StringAlgorithms.Utilities.StringUtilities;

/// <summary>
/// Exposes utility methods to match a <see cref="TextWithTerminator"/> against a 
/// <see cref="ISuffixStructureNode{TEdge, TNode}"/> concretion. 
/// </summary>
public static class Matcher
{
    /// <summary>
    /// Tries to match a pattern against a Suffix Tree built on a text.
    /// </summary>
    /// <param name="node">The root of the Suffix Tree, to match the suffix of text against.</param>
    /// <param name="text">The text whose Suffix Tree has to be matched against the pattern.</param>
    /// <param name="pattern">The pattern to match. Unlike text, is a string without terminator.</param>
    /// <returns>A successful or non-successful match.</returns>
    public static Match<TreePath<TEdge, TNode>> Match<TEdge, TNode>(
        this ISuffixStructureNode<TEdge, TNode> node, 
        TextWithTerminator text, 
        string pattern) 
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> =>
        Match(node, text, pattern, 0);

    private static Match<TreePath<TEdge, TNode>> Match<TEdge, TNode>(
        this ISuffixStructureNode<TEdge, TNode> node, 
        TextWithTerminator text, 
        string pattern, 
        int textStart)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode>
    {
        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException("Must be non-null and non-emtpy.", nameof(pattern));

        var longestMatch = (
            from edge in node.Children.Keys
            let length = LongestPrefixInCommon(edge.Of(text), pattern[textStart..])
            select new { Length = length, Edge = edge })
            .MaxBy(r => r.Length)
            .FirstOrDefault();

        if (longestMatch == null)
            return new Match<TreePath<TEdge, TNode>>(
                false, textStart, 0, new());

        if (longestMatch.Length == pattern.Length - textStart)
            return new Match<TreePath<TEdge, TNode>>(
                true, textStart + longestMatch.Edge.Start, longestMatch.Length, new());

        // The edge has been fully matched but pattern is longer => chars left to match
        var childNode = node.Children[longestMatch.Edge];
        var childMatch = Match(childNode, text, pattern, textStart + longestMatch.Length);
        var pathToChild = new TreePath<TEdge, TNode>(longestMatch.Edge, childNode);
        return new Match<TreePath<TEdge, TNode>>(
            childMatch.Success, 
            textStart + longestMatch.Edge.Start, 
            longestMatch.Length + childMatch.MatchedChars,
            pathToChild.Concat(childMatch.Path));
    }
}