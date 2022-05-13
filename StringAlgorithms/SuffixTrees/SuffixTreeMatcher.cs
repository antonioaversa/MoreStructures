namespace StringAlgorithms.SuffixTrees;

using MoreLinq;
using static StringUtilities;

/// <summary>
/// Exposes utility methods to match a <see cref="Build(TextWithTerminator)"/> against a <see cref="SuffixTreeNode"/>.
/// </summary>
public static class SuffixTreeMatcher
{
    /// <summary>
    /// Tries to match a pattern against a Suffix Tree built on a text.
    /// </summary>
    /// <param name="node">The root of the Suffix Tree, to match the suffix of text against.</param>
    /// <param name="text">The text whose Suffix Tree has to be matched against the pattern.</param>
    /// <param name="text">The pattern to match. Unlike text, is a string without terminator.</param>
    /// <returns>A successful or non-successful match.</returns>
    public static SuffixTreeMatch Match(
        this SuffixTreeNode node, TextWithTerminator text, string pattern) =>
        Match(node, text, pattern, 0);

    private static SuffixTreeMatch Match(
        this SuffixTreeNode node, TextWithTerminator text, string pattern, int textStart)
    {
        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException(nameof(pattern));

        var longestMatch = (
            from edge in node.Children.Keys
            let length = LongestPrefixInCommon(edge.Of(text), pattern[textStart..])
            select new { Length = length, Edge = edge })
            .MaxBy(r => r.Length)
            .FirstOrDefault();

        if (longestMatch == null)
            return new SuffixTreeMatch(
                false, textStart, 0, SuffixTreePath.Empty());

        if (longestMatch.Length == pattern.Length - textStart)
            return new SuffixTreeMatch(
                true, textStart + longestMatch.Edge.Start, longestMatch.Length, SuffixTreePath.Empty());

        // The edge has been fully matched but pattern is longer => chars left to match
        var childNode = node.Children[longestMatch.Edge];
        var childMatch = Match(childNode, text, pattern, textStart + longestMatch.Length);
        var pathToChild = SuffixTreePath.Singleton(longestMatch.Edge, childNode);
        return new SuffixTreeMatch(
            childMatch.Success, 
            textStart + longestMatch.Edge.Start, 
            longestMatch.Length + childMatch.MatchedChars,
            pathToChild.Concat(childMatch.Path));
    }
}