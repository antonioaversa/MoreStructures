using MoreStructures.SuffixStructures.Builders;

namespace MoreStructures.SuffixTries.Builders;

/// <summary>
/// Builds objects, such as edges and nodes, for <see cref="SuffixTrieNode"/> structures.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     Implemented iteratively, with one level of recursion per char of each suffix of the input 
///     <see cref="TextWithTerminator"/> (where the longest suffix is the text itself). 
///     </para>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Limited by call stack depth and usable with input text of a "reasonable" length (i.e. string having a length 
///     &lt; ~1K chars).
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Time Complexity = O(t^2 * as) and Space Complexity = O(t^2) where t = length of the text to match and
///       as = size of the alphabet of the text. 
///       <br/>
///     - If the alphabet is of constant size, complexity is quadratic.
///     </para>
/// </remarks>
public class NaivePartiallyRecursiveSuffixTrieBuilder
    : IBuilder<SuffixTrieEdge, SuffixTrieNode>
{
    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="NaivePartiallyRecursiveSuffixTrieBuilder"/>
    /// </remarks>
    public SuffixTrieNode BuildTree(params TextWithTerminator[] texts)
    {
        var (fullText, terminators) = texts.GenerateFullText();

        SuffixTrieNode root = new SuffixTrieNode.Leaf(0);

        for (var suffixIndex = 0; suffixIndex < fullText.Length; suffixIndex++)
            root = IncludeSuffixIntoTrie(fullText, terminators, suffixIndex, suffixIndex, root);

        return root;
    }

    private static SuffixTrieNode.Intermediate IncludeSuffixIntoTrie(
        TextWithTerminator fullText, ISet<char> separators, int suffixCurrentIndex, int suffixIndex,
        SuffixTrieNode node)
    {
        var nodeChildren = new Dictionary<SuffixTrieEdge, SuffixTrieNode>(node.Children);
        var currentChar = fullText[suffixCurrentIndex];
        var childEdgeAndNode = nodeChildren.SingleOrDefault(c => fullText[c.Key.Index] == currentChar);

        if (childEdgeAndNode is (SuffixTrieEdge edge, SuffixTrieNode child))
        {
            // Recurse on the child, removing the first char from the current suffix
            nodeChildren[edge] = IncludeSuffixIntoTrie(
                fullText,
                separators,
                suffixCurrentIndex: suffixCurrentIndex + 1,
                suffixIndex: suffixIndex,
                node: child);
        }
        else
        {
            // Build the entire path down to the first non-terminator char
            child = new SuffixTrieNode.Leaf(suffixIndex);

            var indexOfFirstTerminator = Enumerable
                .Range(suffixCurrentIndex, fullText.Length - suffixCurrentIndex)
                .First(i => separators.Contains(fullText[i]));

            for (int i = indexOfFirstTerminator; i >= suffixCurrentIndex + 1; i--)
            {
                child = new SuffixTrieNode.Intermediate(
                    new Dictionary<SuffixTrieEdge, SuffixTrieNode> { [new(i)] = child });
            }
            nodeChildren[new(suffixCurrentIndex)] = child;
        }

        return new SuffixTrieNode.Intermediate(nodeChildren);
    }
}
