namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// The result of a pattern matching done by a 
/// <see cref="IMatcher.Match(RotatedTextWithTerminator, RotatedTextWithTerminator, IEnumerable{char})"/>.
/// </summary>
/// <param name="Success">Whether the pattern matching was successful or not.</param>
/// <param name="MatchedChars">
/// The number of chars matched from the pattern. If <see cref="Success"/> is <see langword="false"/>, the value will
/// be strictly smaller than the length of the pattern. Otherwise, it will be equal to the length of the pattern.
/// </param>
/// <param name="StartIndex">
/// The 0-based index, in the Sorted Burrows-Wheeler Transform, of the first char matching.
/// Negative if no matching has happened.
/// </param>
/// <param name="EndIndex">
/// The 0-based index, in the Sorted Burrows-Wheeler Transform, of the last char matching. 
/// Negative if no matching has happened.
/// </param>
public record Match(bool Success, int MatchedChars, int StartIndex, int EndIndex);
