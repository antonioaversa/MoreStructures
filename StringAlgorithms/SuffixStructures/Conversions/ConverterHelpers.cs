using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;

namespace StringAlgorithms.SuffixStructures.Conversions;

/// <summary>
/// Provides functionalities useful to multiple implementations of <see cref="IConverter"/>
/// </summary>
internal static class ConverterHelpers
{
    /// <summary>
    /// Coalesces all <see cref="SuffixTrieNode.Intermediate"/> nodes, starting from the one provided and going
    /// down the <see cref="SuffixTrieNode"/> structure, which have a single child.
    /// </summary>
    /// <param name="trieEdge">The edge pointing to the node to be coalesced with its descendants.</param>
    /// <param name="trieNode">The node to be coalesced with its descendants.</param>
    /// <returns>
    /// The couple of coalesced <see cref="SuffixTreeEdge"/> and the corresponding <see cref="SuffixTrieNode"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// If a node with children of a type different than <see cref="SuffixTrieNode.Intermediate"/> is encountered.
    /// </exception>
    /// <remarks>
    /// Example: the trie nodes path N -(1)-> N -(3)-> N -(5)-> N is coalesced into the tree nodes path N -(1,3)-> N.
    /// </remarks>
    internal static (SuffixTreeEdge coalescedTreeEdge, SuffixTrieNode coalescedTrieNode) Coalesce(
        SuffixTrieEdge trieEdge, SuffixTrieNode trieNode)
    {
        var coalescedTreeEdge = new SuffixTreeEdge(trieEdge.Start, trieEdge.Length);
        var coalescedTrieNode = trieNode;

        while (coalescedTrieNode.Children.Count == 1)
        {
            if (coalescedTrieNode is not SuffixTrieNode.Intermediate)
                throw new NotSupportedException(
                    $"{coalescedTrieNode} of type {coalescedTrieNode.GetType().Name} not supported");

            var trieChild = coalescedTrieNode.Children.Single();
            coalescedTreeEdge = new SuffixTreeEdge(
                coalescedTreeEdge.Start, coalescedTreeEdge.Length + trieChild.Key.Length);
            coalescedTrieNode = coalescedTrieNode.Children.Single().Value;
        }

        return (coalescedTreeEdge, coalescedTrieNode);
    }
}
