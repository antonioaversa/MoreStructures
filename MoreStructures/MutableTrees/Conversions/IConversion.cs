using MoreStructures.SuffixTrees;

namespace MoreStructures.MutableTrees.Conversions;

/// <summary>
/// A converter of <see cref="MutableTree"/>.
/// </summary>
internal interface IConversion
{
    /// <summary>
    /// Converts the provided <paramref name="mutableTree"/> to an immutable <see cref="SuffixTreeNode"/> structure.
    /// </summary>
    /// <param name="mutableTree">The mutable, recursive, dictionary indexed tree to be converted.</param>
    /// <param name="fullText">
    /// The text, the tree of which has to be converted.<br/>
    /// Required to perform trimming of edges in generalized trees.
    /// </param>
    /// <param name="terminators">
    /// The set of terminators in <paramref name="fullText"/>. Contains at least one char. Contains multiple chars
    /// when <paramref name="fullText"/> is the concatenation of multiple <see cref="TextWithTerminator"/>.<br/>
    /// Required to perform trimming of edges in generalized trees.
    /// </param>
    /// <returns>An immutable, recursive, dictionary indexed tree.</returns>
    public SuffixTreeNode ConvertToSuffixTree(
        MutableTree.Node mutableTree, TextWithTerminator fullText, ISet<char> terminators);
}
