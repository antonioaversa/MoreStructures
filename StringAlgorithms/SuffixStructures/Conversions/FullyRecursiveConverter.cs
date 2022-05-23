using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;

namespace StringAlgorithms.SuffixStructures.Conversions;

/// <summary>
/// <inheritdoc cref="IConverter"/>
/// </summary>
/// <remarks>
/// Implemented fully recursively, with one level of recursion per level of the input <see cref="SuffixTrieNode"/>.
/// Limited by call stack depth and usable with input trees of a "reasonable" height.
/// </remarks>
public class FullyRecursiveConverter : IConverter
{
    /// <inheritdoc/>
    public SuffixTreeNode TrieToTree(SuffixTrieNode trieNode) => 
        trieNode switch
        {
            SuffixTrieNode.Leaf(var leafStart) =>
                new SuffixTreeNode.Leaf(leafStart),
            SuffixTrieNode.Intermediate(var trieNodeChildren) =>
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>(
                    from trieChild in trieNodeChildren
                    let treeChildEdge = new SuffixTreeEdge(trieChild.Key.Start, trieChild.Key.Length)
                    let coaleascedChild = Coalesce(treeChildEdge, trieChild.Value)
                    let treeChildEdge1 = coaleascedChild.Item1
                    let trieChildNode1 = coaleascedChild.Item2
                    select KeyValuePair.Create(treeChildEdge1, TrieToTree(trieChildNode1))
                )),
            _ => throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported"),
        };

    private static (SuffixTreeEdge, SuffixTrieNode) Coalesce(SuffixTreeEdge treeEdge, SuffixTrieNode trieNode) => 
        trieNode switch
        {
            SuffixTrieNode.Leaf =>
                (treeEdge, trieNode),
            SuffixTrieNode.Intermediate(var children) when children.Count != 1 =>
                (treeEdge, trieNode),
            SuffixTrieNode.Intermediate(var children) when children.Single() is var (trieChildEdge, trieChildNode) =>
                Coalesce(new SuffixTreeEdge(treeEdge.Start, treeEdge.Length + trieChildEdge.Length), trieChildNode),
            _ => throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported"),
        };
}
