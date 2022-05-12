namespace StringAlgorithms;

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
            from prefixPath in node.Children.Keys
            let length = LongestPrefixInCommon(prefixPath.Of(text), pattern[textStart..])
            select new { Length = length, PrefixPath = prefixPath })
            .MaxBy(r => r.Length)
            .FirstOrDefault();

        if (longestMatch == null)
            return new SuffixTreeMatch(
                false, textStart, 0, SuffixTreePath.Empty());

        if (longestMatch.Length == pattern.Length - textStart)
            return new SuffixTreeMatch(
                true, textStart + longestMatch.PrefixPath.Start, longestMatch.Length, SuffixTreePath.Empty());

        // The prefix path has been fully matched but pattern is longer => chars left to match
        var childNode = node.Children[longestMatch.PrefixPath];
        var childMatch = Match(childNode, text, pattern, textStart + longestMatch.Length);
        var pathToChild = SuffixTreePath.Singleton(longestMatch.PrefixPath, childNode);
        return new SuffixTreeMatch(
            childMatch.Success, 
            textStart + longestMatch.PrefixPath.Start, 
            longestMatch.Length + childMatch.MatchedChars,
            pathToChild.Concat(childMatch.Path));
    }
}