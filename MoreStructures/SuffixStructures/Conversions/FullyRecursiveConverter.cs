using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;

namespace MoreStructures.SuffixStructures.Conversions;

/// <summary>
/// <inheritdoc cref="IConverter"/>
/// </summary>
/// <remarks>
/// Implemented fully recursively, with one level of recursion per level of the input <see cref="SuffixTrieNode"/>.
/// Limited by call stack depth and usable with input trees of a "reasonable" height (i.e. trees having a height &lt; 
/// ~1K nodes).
/// </remarks>
public class FullyRecursiveConverter : IConverter
{
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="FullyRecursiveConverter" path="/remarks"/>
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
                    let treeChildEdge = new SuffixTreeEdge(trieChild.Key.Start, trieChild.Key.Length)
                    let coaleascedChild = RecursiveCoalesce(treeChildEdge, trieChild.Value)
                    let treeChildEdge1 = coaleascedChild.Item1
                    let trieChildNode1 = coaleascedChild.Item2
                    select KeyValuePair.Create(treeChildEdge1, TrieToTree(trieChildNode1))
                )),
            
            _ => throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported"),
        };

    private static (SuffixTreeEdge, SuffixTrieNode) RecursiveCoalesce(
        SuffixTreeEdge treeEdge, SuffixTrieNode trieNode) => 
        trieNode switch
        {
            SuffixTrieNode.Leaf =>
                (treeEdge, trieNode),

            SuffixTrieNode.Intermediate(var children) when children.Count != 1 =>
                (treeEdge, trieNode),

            SuffixTrieNode.Intermediate(var children) when children.Single() is var (trieChildEdge, trieChildNode) =>
                RecursiveCoalesce(
                    new SuffixTreeEdge(treeEdge.Start, treeEdge.Length + trieChildEdge.Length), trieChildNode),
            
            _ => throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported"),
        };

    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="FullyRecursiveConverter" path="/remarks"/>
    ///     <inheritdoc cref="IConverter.TreeToTrie(SuffixTreeNode)" path="/remarks"/>
    /// </remarks>
    public SuffixTrieNode TreeToTrie(SuffixTreeNode treeNode) =>
        treeNode switch
        {
            SuffixTreeNode.Leaf(var leafStart) =>
                new SuffixTrieNode.Leaf(leafStart),

            SuffixTreeNode.Intermediate(var nodeChildren) =>
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>(
                    from treeChild in nodeChildren
                    let expandedChild = RecursiveExpand(treeChild.Key, TreeToTrie(treeChild.Value), 0)
                    select KeyValuePair.Create(expandedChild.expandedTrieEdge, expandedChild.expandedTrieNode))),

            _ => throw new NotSupportedException($"{treeNode} of type {treeNode.GetType().Name} not supported"),
        };

    private static (SuffixTrieEdge expandedTrieEdge, SuffixTrieNode expandedTrieNode) RecursiveExpand(
        SuffixTreeEdge treeEdge, SuffixTrieNode trieNode, int edgeIndexDelta)
    {
        var expandedTrieEdge = new SuffixTrieEdge(treeEdge.Start + edgeIndexDelta);

        if (treeEdge.Length == edgeIndexDelta + 1)
            return (expandedTrieEdge, trieNode);

        var (childExpandedTrieEdge, childExpandedTrieNode) = RecursiveExpand(treeEdge, trieNode, edgeIndexDelta + 1);
        return (expandedTrieEdge,
            new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [childExpandedTrieEdge] = childExpandedTrieNode
            }));
    }
}
