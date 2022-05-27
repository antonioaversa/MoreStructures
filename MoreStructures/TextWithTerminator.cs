using System.Collections;

namespace MoreStructures;

/// <summary>
/// A text string with a terminator character, not present in the text.
/// </summary>
/// <param name="Text">A text string.</param>
/// <param name="Terminator">
/// A terminator character, not present in the text. If not specified <see cref="DefaultTerminator"/> is used.
/// </param>
/// <remarks>
/// A terminator-terminated text is required by data structures like Suffix Tries, Trees or Arrays. 
/// This object provides type safety, as it allows to tell apart terminator-terminated strings from generic ones.
/// Consistently using <see cref="TextWithTerminator"/>, rather than <see cref="string"/>, in all library 
/// functionalities ensures that the invariant of a terminator-terminated string is always respected.
/// Most string-related functionalities provided by <see cref="TextWithTerminator"/>, such as <see cref="Length"/> and
/// <see cref="this[Index]"/>, as well as <see cref="IEnumerable{T}"/> and <see cref="IEnumerable"/> support, are 
/// delegated to the underlying string.
/// </remarks>
public record TextWithTerminator(string Text, char Terminator = TextWithTerminator.DefaultTerminator)
    : IEnumerable<char>
{
    /// <summary>
    /// A selector of a part of a text with terminator.
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// Extract the substring identified by this selector, out of the provided text.
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
    /// Select a part of this text by the provided range (start index included, end index excluded).
    /// </summary>
    /// <param name="range">The range applied to the underlying string.</param>
    /// <returns>A string containing the selected part.</returns>
    public string this[Range range] => TextAndTerminator[range];

    /// <summary>
    /// Select a part of this text by the provided index (either w.r.t. the start or to the end of the text).
    /// </summary>
    /// <param name="index">The index applied to the underlying string.</param>
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
    /// Returns an enumerator that iterates through the collection of chars of the underlying <see cref="Text"/> 
    /// string, including the <see cref="Terminator"/> char.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public IEnumerator<char> GetEnumerator() => TextAndTerminator.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the collection of chars of the underlying <see cref="Text"/> 
    /// string, including the <see cref="Terminator"/> char.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)TextAndTerminator).GetEnumerator();
}
