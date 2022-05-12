namespace StringAlgorithms;

/// <summary>
/// A text string with a terminator character, not present in the text.
/// </summary>
/// <param name="Text">A text string.</param>
/// <param name="Terminator">
/// A terminator character, not present in the text. If not specified <see cref="DefaultTerminator"/> is used.
/// </param>
/// <remarks>
/// A terminator-terminated text is required by data structures like Suffix Trees. 
/// This object provides type safety, as it allows to tell apart terminator-terminated strings from generic ones.
/// </remarks>
public record TextWithTerminator(string Text, char Terminator = TextWithTerminator.DefaultTerminator)
{
    /// <summary>
    /// The special character used as a default terminator for the text to build the Suffix Tree of, when no custom 
    /// terminator is specified. Should not be present in the text.
    /// </summary>
    public const char DefaultTerminator = '$';

    /// <summary>
    /// A text string.
    /// </summary>
    public string Text { get; init; } = Text;

    /// <summary>
    /// A terminator character, not present in the text.
    /// </summary>
    public char Terminator { get; init; } =
        !Text.Contains(Terminator) 
        ? Terminator 
        : throw new ArgumentException($"{nameof(Terminator)} shouldn't be included in {nameof(Text)}");

    private readonly string TextAndTerminator = Text + Terminator;

    /// <summary>
    /// Returns this text with terminator in textual form, appending the terminator to the text.
    /// </summary>
    public string AsString => TextAndTerminator;

    /// <summary>
    /// <inheritdoc/>
    /// Specifically <see cref="AsString"/>.
    /// </summary>
    public override string ToString() => TextAndTerminator;

}
