namespace StringAlgorithms;

/// <summary>
/// The index key of the collection of children of a Suffix Trie node, which identifies a single char in text, used 
/// as a selector to navigate the Suffix Trie.
/// </summary>
/// <param name="Index">The index of the character of the prefix in the text.</param>
public record Prefix(int Index)
{
    /// <summary>
    /// Returns the char of the provided text with terminator, identified by this prefix.
    /// </summary>
    /// <param name="text">The text, to apply the prefix to.</param>
    /// <returns>The character at the Index of this prefix.</returns>
    public char Of(TextWithTerminator text) => text.AsString[Index];
}