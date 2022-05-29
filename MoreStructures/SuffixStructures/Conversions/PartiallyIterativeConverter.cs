using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;

namespace MoreStructures.SuffixStructures.Conversions;

using static ConverterHelpers;

/// <summary>
/// <inheritdoc cref="IConverter"/>
/// </summary>
/// <remarks>
/// Conversion is iteratively for no-branching paths (i.e. on nodes having a single child) and recursively on 
/// branching of the input <see cref="SuffixTrieNode"/>, with occasional mutation of internal state of the 
/// conversion.
/// Limited by stack depth (but less than <see cref="FullyRecursiveConverter"/>) and usable with output trees of a 
/// "reasonable" height (i.e. trees having a height &lt; ~1K nodes).
/// </remarks>
public class PartiallyIterativeConverter : IConverter
{
    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="PartiallyIterativeConverter" path="/remarks"/>
    ///     <inheritdoc cref="IConverter.TrieToTree(SuffixTrieNode)" path="/remarks"/>
    /// </remarks>
    public SuffixTreeNode TrieToTree(SuffixTrieNode trieNode) => 
        trieNode switch
        {
            SuffixTrieNode.Leaf(var leafStart) => 
                new SuffixTreeNode.Leaf(leafStart),

            SuffixTrieNode.Intermediate(var trieNodeChildren) =>
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>(
                    from trieChild in trieNodeChildren
                    let coalescedChild = IterativeCoalesce(trieChild.Key, trieChild.Value)
                    let coalescedTreeChildNode = TrieToTree(coalescedChild.coalescedTrieNode)
                    select KeyValuePair.Create(coalescedChild.coalescedTreeEdge, coalescedTreeChildNode))),

            _ => throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported")
        };

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="PartiallyIterativeConverter" path="/remarks"/>
    ///     <inheritdoc cref="IConverter.TreeToTrie(SuffixTreeNode)" path="/remarks"/>
    /// </remarks>
    public SuffixTrieNode TreeToTrie(SuffixTreeNode treeNode) => 
        treeNode switch
        {
            SuffixTreeNode.Leaf(var leafStart) => 
                new SuffixTrieNode.Leaf(leafStart),
            
            SuffixTreeNode.Intermediate(var treeNodeChildren) =>
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>(
                    from treeChild in treeNodeChildren
                    let expandedLastTrieNode = TreeToTrie(treeChild.Value)
                    let expandedChild = IterativeExpand(treeChild.Key, expandedLastTrieNode)                   
                    select KeyValuePair.Create(expandedChild.expandedTrieEdge, expandedChild.expandedTrieNode))),

            _ => throw new NotSupportedException($"{treeNode} of type {treeNode.GetType().Name} not supported")
        };
}
