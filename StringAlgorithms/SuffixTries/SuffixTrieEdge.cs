namespace StringAlgorithms.SuffixTries;

/// <summary>
/// The index key of the collection of children of a Suffix Trie node, which identifies a single char in text, used 
/// as a selector to navigate the Suffix Trie.
/// </summary>
/// <param name="Index">The index of the character in the text.</param>
public record SuffixTrieEdge(int Index) : TextWithTerminator.ISelector
{
    /// <summary>
    /// <inheritdoc cref="SuffixTrieEdge(int)" path="/param[@name='Index']"/>
    /// </summary>
    public int Index { get; init; } = Index >= 0 
        ? Index 
        : throw new ArgumentOutOfRangeException(nameof(Index), "Must be a non-negative value.");

    /// <summary>
    /// Returns the char of the provided text with terminator, identified by this edge.
    /// </summary>
    /// <param name="text">The text, to apply the edge to.</param>
    /// <returns>The character at the Index of this edge.</returns>
    public string Of(TextWithTerminator text) => text[Index] + "";
}