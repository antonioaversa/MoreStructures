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
    /// A selector of a part of a text with terminator.
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// The selection method of the selector, extracting a substring out of the provided text.
        /// </summary>
        /// <param name="text">The text with terminator, to extract a substring of.</param>
        /// <returns>A substring, whose length depends on the selector.</returns>
        string Of(TextWithTerminator text);
    }

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
    /// Select a part of this text by the provided selector.
    /// </summary>
    /// <param name="selector">Any selector acting on a <see cref="TextWithTerminator"/>.</param>
    /// <returns>A string containing the selected part.</returns>
    public string this[ISelector selector] => selector.Of(this);

    /// <summary>
    /// Select a part of this text by the provided range.
    /// </summary>
    /// <param name="range">The range applied to the underlying string.</param>
    /// <returns>A string containing the selected part.</returns>
    public string this[Range range] => TextAndTerminator[range];

    /// <summary>
    /// Select a part of this text by the provided index.
    /// </summary>
    /// <param name="range">The index applied to the underlying string.</param>
    /// <returns>A char containing the selected part.</returns>
    public char this[Index index] => TextAndTerminator[index];

    /// <summary>
    /// The total length of this text, including the terminator.
    /// </summary>
    public int Length => TextAndTerminator.Length;

    /// <summary>
    /// Whether this text starts with the provided suffix.
    /// </summary>
    /// <param name="prefix">A terminator-included string.</param>
    /// <returns>True if this text starts by the prefix.</returns>
    public bool StartsWith(string prefix) => TextAndTerminator.StartsWith(prefix);

    /// <summary>
    /// Whether this text ends with the provided suffix.
    /// </summary>
    /// <param name="suffix">A terminator-included string.</param>
    /// <returns>True if this text ends by the suffix.</returns>
    public bool EndsWith(string suffix) => TextAndTerminator.EndsWith(suffix);

    /// <summary>
    /// <inheritdoc/>
    /// Specifically <see cref="AsString"/>.
    /// </summary>
    public override string ToString() => TextAndTerminator;

}
