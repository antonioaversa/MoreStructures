using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;

namespace MoreStructures.SuffixStructures.Conversions;

/// <summary>
/// Provides functionalities useful to multiple implementations of <see cref="IConverter"/>
/// </summary>
internal static class ConverterHelpers
{
    /// <summary>
    /// Iteratively coalesces all <see cref="SuffixTrieNode.Intermediate"/> nodes, starting from the one provided and 
    /// going down the <see cref="SuffixTrieNode"/> structure, which have a single child.
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
    internal static (SuffixTreeEdge coalescedTreeEdge, SuffixTrieNode coalescedTrieNode) IterativeCoalesce(
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

    /// <summary>
    /// Iteratively expands the provided <see cref="SuffixTreeEdge"/>, into a chain of <see cref="SuffixTrieEdge"/> and
    /// <see cref="SuffixTrieNode"/> instances, where each node has a single child and where the last node points to 
    /// the provided <see cref="SuffixTrieNode"/>.
    /// </summary>
    /// <param name="treeEdge">The edge pointing to the node to be expandend.</param>
    /// <param name="trieNode">The node to be attached to the last expanded node in the chain.</param>
    /// <returns>
    /// The couple of expanded <see cref="SuffixTrieEdge"/> and the corresponding <see cref="SuffixTrieNode"/>.
    /// </returns>
    /// <remarks>
    /// Example: the tree edge -(1,3)-> N is expanded into the trie nodes path -(1)-> N -(2)-> N -(3)-> N*.
    /// Where N* is the provided <see cref="SuffixTrieNode"/>.
    /// </remarks>
    internal static (SuffixTrieEdge expandedTrieEdge, SuffixTrieNode expandedTrieNode) IterativeExpand(
        SuffixTreeEdge treeEdge, SuffixTrieNode trieNode)
    {
        var treeEdgeStart = treeEdge.Start;
        var expandedTrieEdge = new SuffixTrieEdge(treeEdgeStart);
        var expandedTrieNode = trieNode;
        for (var i = treeEdge.Length - 1; i >= 1; i--)
            expandedTrieNode = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(treeEdgeStart + i)] = expandedTrieNode,
            });

        return (expandedTrieEdge, expandedTrieNode);
    }
}
