using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;

namespace StringAlgorithms.SuffixStructures.Conversions;

/// <summary>
/// <inheritdoc cref="IConverter"/>
/// <see cref="SuffixTrieNode"/> a "reasonable" height of the expect output <see cref="SuffixTreeNode"/>.
/// </summary>
/// <remarks>
/// Conversion is iteratively for no-branching paths (i.e. on nodes having a single child) and recursively on 
/// branching of the input <see cref="SuffixTrieNode"/>, with occasional mutation of internal state of the 
/// conversion.
/// Limited by stack depth (but less than <see cref="FullyRecursiveConverter"/> and usable with 
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
                var treeChildEdge1 = new SuffixTreeEdge(trieChildEdge.Start, trieChildEdge.Length);
                var trieChildNode1 = trieChildNode;

                while (trieChildNode1.Children.Count == 1)
                {
                    if (trieChildNode1 is not SuffixTrieNode.Intermediate)
                        throw new NotSupportedException(
                            $"{trieChildNode1} of type {trieChildNode1.GetType().Name} not supported");

                    var trieChild = trieChildNode1.Children.Single();
                    treeChildEdge1 = new SuffixTreeEdge(
                        treeChildEdge1.Start, treeChildEdge1.Length + trieChild.Key.Length);
                    trieChildNode1 = trieChildNode1.Children.Single().Value;
                }

                treeNodeChildren[treeChildEdge1] = TrieToTree(trieChildNode1);
            }

            return new SuffixTreeNode.Intermediate(treeNodeChildren);
        }

        throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported");
    }
}
