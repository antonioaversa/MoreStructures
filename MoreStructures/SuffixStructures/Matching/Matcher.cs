using MoreStructures.RecImmTrees;
using static MoreStructures.Utilities.StringUtilities;

namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// Exposes utility methods to match a <see cref="TextWithTerminator"/> against a 
/// <see cref="ISuffixStructureNode{TEdge, TNode}"/> concretion. 
/// </summary>
public static class Matcher
{
    /// <summary>
    /// Tries to match a pattern against a <see cref="ISuffixStructureNode{TEdge, TNode}"/> built on a text.
    /// </summary>
    /// <param name="node">The root of the Suffix Tree, to match the suffix of text against.</param>
    /// <param name="text">The text whose Suffix Tree has to be matched against the pattern.</param>
    /// <param name="pattern">The pattern to match. Unlike text, is a string without terminator.</param>
    /// <typeparam name="TEdge">
    ///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
    /// </typeparam>
    /// <typeparam name="TNode">
    ///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
    /// </typeparam>
    /// <returns>A successful or non-successful match.</returns>
    /// <remarks>
    ///     <para id="complexity">
    ///     Time Complexity = O(t * as) and Space Complexity = O(t * as) where t = length of the text to match and
    ///     as = size of the alphabet of the text. If the alphabet is of constant size, complexity is linear.
    ///     </para>
    /// </remarks>
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
            .MaxBy(r => r.Length);

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