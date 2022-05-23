using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;

namespace MoreStructures.SuffixStructures.Conversions;

/// <summary>
/// <inheritdoc cref="IConverter"/>
/// </summary>
/// <remarks>
/// Conversion is iteratively for no-branching paths (i.e. on nodes having a single child) and recursively on 
/// branching of the input <see cref="SuffixTrieNode"/>, with occasional mutation of internal state of the 
/// conversion.
/// Limited by stack depth (but less than <see cref="FullyRecursiveConverter"/>) and usable with output trees of a 
/// "reasonable" height.
/// </remarks>
public class PartiallyIterativeConverter : IConverter
{
    /// <inheritdoc/>
    public SuffixTreeNode TrieToTree(SuffixTrieNode trieNode)
    {
        if (trieNode is SuffixTrieNode.Leaf(var leafStart))
            return new SuffixTreeNode.Leaf(leafStart);

        if (trieNode is SuffixTrieNode.Intermediate(var trieNodeChildren))
        {
            var treeNodeChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };
            foreach (var (trieChildEdge, trieChildNode) in trieNodeChildren)
            {
                var (coalescedChildEdge, coalescedChildNode) = ConverterHelpers.Coalesce(trieChildEdge, trieChildNode);
                treeNodeChildren[coalescedChildEdge] = TrieToTree(coalescedChildNode);
            }

            return new SuffixTreeNode.Intermediate(treeNodeChildren);
        }

        throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported");
    }
}
