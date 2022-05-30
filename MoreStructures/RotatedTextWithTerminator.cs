using MoreStructures.Utilities;
using System.Collections;

namespace MoreStructures;

/// <summary>
/// A text string with a terminator character which has been rotated leftwards or rightwards, of a number of positions 
/// (0 included).
/// </summary>
/// <param name="RotatedText">
/// The text, defined ad an <see cref="IEnumerable{T}"/> of chars and containing the terminator character once, in any 
/// position of the text.
/// </param>
/// <param name="Terminator">
/// A terminator character, present in the text at most once. If not specified 
/// <see cref="TextWithTerminator.DefaultTerminator"/> is used.
/// </param>
/// <param name="ValidateInput">
/// Whether the input, and in particular <see cref="RotatedText"/> should be validated, while this object is created.
/// Validation takes O(n) time, where n = number of chars in <see cref="RotatedText"/> and can be an heavy operation.
/// </param>
/// <remarks>
/// A terminator-terminated rotated text is required by Burrows-Wheeler Transform operations, such as inversion. 
/// This object provides type safety, as it allows to tell apart rotated terminator-terminated strings from generic 
/// ones.
/// Consistently using <see cref="RotatedTextWithTerminator"/>, rather than <see cref="string"/>, in all library 
/// functionalities ensures that the invariant of a rotated terminator-terminated string is always respected.
/// Most string-related functionalities provided by <see cref="RotatedTextWithTerminator"/>, such as 
/// <see cref="Length"/> and <see cref="this[Index]"/>, as well as <see cref="IEnumerable{T}"/> and 
/// <see cref="IEnumerable"/> support, are delegated to the underlying string.
/// </remarks>
public record RotatedTextWithTerminator(
    IEnumerable<char> RotatedText, 
    char Terminator = TextWithTerminator.DefaultTerminator,
    bool ValidateInput = true)
    : IValueEnumerable<char>
{
    private int? _length = null;

    /// <summary>
    /// <inheritdoc cref="RotatedTextWithTerminator" path="/param[@name='RotatedText']"/>
    /// </summary>
    /// <remarks>
    /// Wrapped into a <see cref="IValueEnumerable{T}"/> to preserve value equality.
    /// </remarks>
    public IEnumerable<char> RotatedText { get; init; } = RotatedText.AsValue();

    /// <summary>
    /// <inheritdoc cref="RotatedTextWithTerminator" path="/param[@name='Terminator']"/>
    /// </summary>
    public char Terminator { get; init; } =
        !ValidateInput || RotatedText.Count(c => c == Terminator) == 1 
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
    /// <returns>An <see cref="IEnumerable{T}"/> of chars containing the selected part.</returns>
    public IEnumerable<char> this[Range range] => RotatedText.Take(range);

    /// <summary>
    /// Select a part of <see cref="RotatedText"/> by the provided index (either w.r.t. the start or to the end of the 
    /// text).
    /// </summary>
    /// <param name="index">The index applied to the underlying string.</param>
    /// <returns>A char containing the selected part.</returns>
    public char this[Index index] => RotatedText.ElementAt(index);

    /// <summary>
    /// The total length of <see cref="RotatedText"/>, including the terminator.
    /// </summary>
    /// <remarks>
    /// After the first call, the value is cached for all subsequent calls.
    /// </remarks>
    public int Length
    {
        get
        {
            if (_length == null)
                _length = RotatedText.Count();
            return _length.Value;
        }
    }

    /// <summary>
    /// Whether this text starts with <paramref name="prefix"/>.
    /// </summary>
    /// <param name="prefix">A terminator-included string.</param>
    /// <returns>True if <see cref="RotatedText"/> starts by <paramref name="prefix"/>.</returns>
    public bool StartsWith(string prefix) => RotatedText.Take(prefix.Length).SequenceEqual(prefix);

    /// <summary>
    /// Whether this text ends with <paramref name="suffix"/>.
    /// </summary>
    /// <param name="suffix">A terminator-included string.</param>
    /// <returns>True if <see cref="RotatedText"/> ends by <paramref name="suffix"/>.</returns>
    public bool EndsWith(string suffix) => RotatedText.TakeLast(suffix.Length).SequenceEqual(suffix);

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
