using System.Collections;

namespace MoreStructures;

/// <summary>
/// A text string with a terminator character which has been rotated leftwards or rightwards, of a number of positions 
/// (0 included).
/// </summary>
/// <param name="RotatedText">The  text string, containing the terminator character once, in any position of the text.</param>
/// <param name="Terminator">
/// A terminator character, present in the text at most once. If not specified 
/// <see cref="TextWithTerminator.DefaultTerminator"/> is used.
/// </param>
/// <remarks>
/// A terminator-terminated rotated text is required by Barrows-Wheeler Transform operations, such as inversion. 
/// This object provides type safety, as it allows to tell apart rotated terminator-terminated strings from generic 
/// ones.
/// Consistently using <see cref="RotatedTextWithTerminator"/>, rather than <see cref="string"/>, in all library 
/// functionalities ensures that the invariant of a rotated terminator-terminated string is always respected.
/// Most string-related functionalities provided by <see cref="RotatedTextWithTerminator"/>, such as 
/// <see cref="Length"/> and <see cref="this[Index]"/>, as well as <see cref="IEnumerable{T}"/> and 
/// <see cref="IEnumerable"/> support, are delegated to the underlying string.
/// </remarks>
public record RotatedTextWithTerminator(string RotatedText, char Terminator = TextWithTerminator.DefaultTerminator)
    : IEnumerable<char>
{
    /// <summary>
    /// <inheritdoc cref="RotatedTextWithTerminator" path="/param[@name='RotatedText']"/>
    /// </summary>
    public string RotatedText { get; init; } = RotatedText;

    /// <summary>
    /// <inheritdoc cref="RotatedTextWithTerminator" path="/param[@name='Terminator']"/>
    /// </summary>
    public char Terminator { get; init; } =
        RotatedText.Count(c => c == Terminator) == 1 
        ? Terminator 
        : throw new ArgumentException($"{nameof(Terminator)} should occur in {nameof(RotatedText)} exactly once");

    /// <summary>
    /// Select a part of <see cref="RotatedText"/> by the provided selector.
    /// </summary>
    /// <param name="selector">Any selector acting on a <see cref="RotatedTextWithTerminator"/>.</param>
    /// <returns>A string containing the selected part.</returns>
    public string this[TextWithTerminator.ISelector selector] => selector.OfRotated(this);

    /// <summary>
    /// Select a part of <see cref="RotatedText"/> by the provided range (start index included, end index excluded).
    /// </summary>
    /// <param name="range">The range applied to the underlying string.</param>
    /// <returns>A string containing the selected part.</returns>
    public string this[Range range] => RotatedText[range];

    /// <summary>
    /// Select a part of <see cref="RotatedText"/> by the provided index (either w.r.t. the start or to the end of the 
    /// text).
    /// </summary>
    /// <param name="index">The index applied to the underlying string.</param>
    /// <returns>A char containing the selected part.</returns>
    public char this[Index index] => RotatedText[index];

    /// <summary>
    /// The total length of <see cref="RotatedText"/>, including the terminator.
    /// </summary>
    public int Length => RotatedText.Length;

    /// <summary>
    /// Whether this text starts with <paramref name="prefix"/>.
    /// </summary>
    /// <param name="prefix">A terminator-included string.</param>
    /// <returns>True if <see cref="RotatedText"/> starts by <paramref name="prefix"/>.</returns>
    public bool StartsWith(string prefix) => RotatedText.StartsWith(prefix);

    /// <summary>
    /// Whether this text ends with <paramref name="suffix"/>.
    /// </summary>
    /// <param name="suffix">A terminator-included string.</param>
    /// <returns>True if <see cref="RotatedText"/> ends by <paramref name="suffix"/>.</returns>
    public bool EndsWith(string suffix) => RotatedText.EndsWith(suffix);

    /// <summary>
    /// Returns an enumerator that iterates through the collection of chars of the underlying <see cref="RotatedText"/> 
    /// string, including the <see cref="Terminator"/> char.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public IEnumerator<char> GetEnumerator() => RotatedText.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the collection of chars of the underlying <see cref="RotatedText"/> 
    /// string, including the <see cref="Terminator"/> char.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)RotatedText).GetEnumerator();
}
