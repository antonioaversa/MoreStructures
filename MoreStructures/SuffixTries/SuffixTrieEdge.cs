using MoreStructures.SuffixStructures;
using MoreStructures.SuffixTrees;

namespace MoreStructures.SuffixTries;

/// <summary>
/// The index key of the collection of children of a <see cref="SuffixTrieNode"/>, which identifies a single char in 
/// text, used as a selector to navigate the <see cref="SuffixTrieNode"/> in text pattern matching.
/// </summary>
/// <param name="Index">The index of the character in the text.</param>
public record SuffixTrieEdge(int Index) 
    : SuffixTreeEdge(Index, 1), ISuffixStructureEdge<SuffixTrieEdge, SuffixTrieNode>
{
}