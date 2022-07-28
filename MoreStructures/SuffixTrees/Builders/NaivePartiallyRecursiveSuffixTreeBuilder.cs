using MoreStructures.SuffixStructures.Builders;
using static MoreStructures.Utilities.StringUtilities;

namespace MoreStructures.SuffixTrees.Builders;

/// <summary>
/// Builds objects, such as edges and nodes, for <see cref="SuffixTreeNode"/> structures.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     Implemented as an iteration of recursive visit of the tree being built, with as many iterations as the number 
///     of suffix of the input (where the longest suffix is the text itself) and one level of recursion per char of 
///     each suffix. 
///     <br/>
///     Limited by call stack depth and usable with input text of a "reasonable" length (i.e. string having a length 
///     &lt; ~1K chars).
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Time Complexity = O(t^2 * as) and Space Complexity = O(t) where t = length of the text to match and
///       as = size of the alphabet of the text. If the alphabet is of constant size, Time Complexity is quadratic.
///       <br/>
///     - Compared to tries, trees are more compact due to edge coalescing and edge label compression (i.e. edge 
///       strings stored as pair (start, length), rather than as a substring of length chars). Each recursion add a 
///       leaf and at most one intermediate node, so Space Complexity ~ 2 * t = O(t).
///     </para>
/// </remarks>
public class NaivePartiallyRecursiveSuffixTreeBuilder
    : IBuilder<SuffixTreeEdge, SuffixTreeNode>
{
    /// <inheritdoc path="//*[not self::summary or self::remarks]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="NaivePartiallyRecursiveSuffixTreeBuilder" path="/remarks"/>
    /// </remarks>
    public SuffixTreeNode BuildTree(params TextWithTerminator[] texts)
    {
        var (fullText, terminators) = texts.GenerateFullText();

        SuffixTreeNode root = new SuffixTreeNode.Leaf(0);

        for (var suffixIndex = 0; suffixIndex < fullText.Length; suffixIndex++)
            root = IncludeSuffixIntoTree(fullText, terminators, suffixIndex, suffixIndex, root);

        return root;
    }

    private static SuffixTreeNode.Intermediate IncludeSuffixIntoTree(
        TextWithTerminator text, ISet<char> terminators, int suffixCurrentIndex, int suffixIndex, SuffixTreeNode node)
    {
        var nodeChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>(node.Children);
        var currentChar = text[suffixCurrentIndex];
        var edgeSame1stChar = nodeChildren.Keys.SingleOrDefault(edge => text[edge.Start] == currentChar);

        if (edgeSame1stChar == null)
        {
            // Find first index which corresponds to a terminator
            var index1stTerminator = Enumerable
                .Range(suffixCurrentIndex, text.Length - suffixCurrentIndex)
                .First(i => terminators.Contains(text[i]));

            var edge = new SuffixTreeEdge(suffixCurrentIndex, index1stTerminator - suffixCurrentIndex + 1);
            nodeChildren[edge] = new SuffixTreeNode.Leaf(suffixIndex);
        }
        else
        {
            // Compare text[suffixCurrentIndex, ...] and text[edgeSame1stChar.Start, ...] for longest edge in common.
            // If the prefix in common is shorter than the edge with the same first char, create an intermediate node,
            // push down the child pointed by the edge in the current node and add a new node for the reminder of
            // text[suffixCurrentIndex, ...] (up to next terminator) as second child of the intermediate.
            // Otherwise, eat prefixLength chars from the edge, move to the child pointed by the edge entirely matching
            // the beginning of the current suffix and repeat the same operation.

            var prefixLength = LongestCommonPrefix(
                text[suffixCurrentIndex..], edgeSame1stChar.Of(text));

            var oldChild = nodeChildren[edgeSame1stChar];
            if (prefixLength < edgeSame1stChar.Length)
            {
                var charsToNextSeparator = Enumerable.Range(0, text.Length - suffixCurrentIndex - prefixLength)
                    .First(i => terminators.Contains(text[suffixCurrentIndex + prefixLength + i]));
                var newLeaf = new SuffixTreeNode.Leaf(suffixIndex);
                var newIntermediate = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>()
                {
                    [new(
                        edgeSame1stChar.Start + prefixLength,
                        edgeSame1stChar.Length - prefixLength)] = oldChild,
                    [new(
                        suffixCurrentIndex + prefixLength,
                        charsToNextSeparator + 1)] = newLeaf,
                });
                nodeChildren.Remove(edgeSame1stChar);
                nodeChildren[new(edgeSame1stChar.Start, prefixLength)] = newIntermediate;
            }
            else
            {
                var newChild = IncludeSuffixIntoTree(
                    text, terminators, suffixCurrentIndex + prefixLength, suffixIndex, oldChild);
                nodeChildren.Remove(edgeSame1stChar);
                nodeChildren[edgeSame1stChar] = newChild;
            }
        }

        return new SuffixTreeNode.Intermediate(nodeChildren);
    }
}
