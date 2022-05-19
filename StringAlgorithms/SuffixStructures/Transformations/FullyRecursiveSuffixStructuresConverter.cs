using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;

namespace StringAlgorithms.SuffixStructures.Transformations
{
    /// <summary>
    /// <inheritdoc cref="ISuffixStructuresConverter"/> 
    /// Conversions are done recursively and with no mutations.
    /// </summary>
    public class FullyRecursiveSuffixStructuresConverter : ISuffixStructuresConverter
    {
        /// <inheritdoc/>
        public SuffixTreeNode TrieToTree(SuffixTrieNode trieNode) => trieNode switch
        {
            SuffixTrieNode.Leaf(var leafStart) =>
                new SuffixTreeNode.Leaf(leafStart),
            SuffixTrieNode.Intermediate(var trieNodeChildren) =>
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>(
                    from trieChildNode in trieNodeChildren
                    let treeChildEdge = new SuffixTreeEdge(trieChildNode.Key.Start, trieChildNode.Key.Length)
                    let coaleascedChild = Coalesce(trieChildNode.Key, trieChildNode.Value)
                    let treeChildEdge1 = coaleascedChild.Item1
                    let trieChildNode1 = coaleascedChild.Item2
                    select KeyValuePair.Create(treeChildEdge1, TrieToTree(trieChildNode1))
                )),
            _ => throw new NotSupportedException($"{trieNode} of type {trieNode?.GetType().Name} not supported"),
        };

        private static (SuffixTreeEdge, SuffixTrieNode) Coalesce(
            SuffixTreeEdge treeChildEdge, SuffixTrieNode trieChildNode)
        {
            if (trieChildNode.Children.Count != 1)
                return (treeChildEdge, trieChildNode);

            var trieChild = trieChildNode.Children.Single();
            return Coalesce(
                new SuffixTreeEdge(treeChildEdge.Start, treeChildEdge.Length + trieChild.Key.Length), 
                trieChild.Value);
        }
    }
}