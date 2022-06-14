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
///     Time Complexity = O(t^2 * as) and Space Complexity = O(t^2) where t = length of the text to match and
///     as = size of the alphabet of the text. If the alphabet is of constant size, complexity is quadratic.
///     </para>
/// </remarks>
public class NaivePartiallyRecursiveSuffixTrieBuilder
    : IBuilder<SuffixTrieEdge, SuffixTrieNode>
{
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="NaivePartiallyRecursiveSuffixTrieBuilder" path="/remarks"/>
    /// </remarks>
    public SuffixTrieNode BuildTree(TextWithTerminator text)
    {
        SuffixTrieNode root = new SuffixTrieNode.Leaf(0);

        for (var suffixIndex = 0; suffixIndex < text.Length; suffixIndex++)
            root = IncludeSuffixIntoTrie(text, suffixIndex, suffixIndex, root);

        return root;
    }

    private static SuffixTrieNode.Intermediate IncludeSuffixIntoTrie(
        TextWithTerminator text, int suffixCurrentIndex, int suffixIndex, SuffixTrieNode node)
    {
        var nodeChildren = new Dictionary<SuffixTrieEdge, SuffixTrieNode>(node.Children);
        var childEdgeAndNode = nodeChildren
            .SingleOrDefault(c => text[c.Key] == text[new SuffixTrieEdge(suffixCurrentIndex)]);

        if (childEdgeAndNode is (SuffixTrieEdge edge, SuffixTrieNode child))
        {
            nodeChildren[edge] = IncludeSuffixIntoTrie(text, suffixCurrentIndex + 1, suffixIndex, child);
        }
        else
        {
            child = new SuffixTrieNode.Leaf(suffixIndex);
            for (int i = text.Length - 1; i >= suffixCurrentIndex + 1; i--)
            {
                child = new SuffixTrieNode.Intermediate(
                    new Dictionary<SuffixTrieEdge, SuffixTrieNode> { [new(i)] = child });
            }
            nodeChildren[new(suffixCurrentIndex)] = child;
        }

        return new SuffixTrieNode.Intermediate(nodeChildren);
    }
}
