namespace StringAlgorithms
{
    /// <summary>
    /// The result of matching a <see cref="TextWithTerminator"/> against a <see cref="SuffixTreeNode"/>.
    /// </summary>
    /// <param name="Success">Whether the text has been completely matched or not.</param>
    /// <param name="Begin">The index in text of the best match in the Suffix Tree.</param>
    /// <param name="MatchedChars">The number of chars matched from the text.</param>
    /// <param name="Path">The path of Suffix Tree nodes visited by the matching algorithm.</param>
    public record SuffixTreeMatch(bool Success, int Begin, int MatchedChars, SuffixTreePath Path);
}