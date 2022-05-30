using System.Collections.Generic;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A implementation of matching of a pattern against a <see cref="RotatedTextWithTerminator"/> containing a
/// Burrows-Wheeler Transform of a text.
/// </summary>
public interface IMatcher
{
    /// <summary>
    /// Tries to match the provided <paramref name="pattern"/> using the provided <paramref name="bwt"/> and its sorted
    /// version <paramref name="sbwt"/>.
    /// </summary>
    /// <param name="sbwt">The Sorted Burrows-Wheeler Transform of the text</param>
    /// <param name="bwt">The Burrows-Wheeler Transform of the text.</param>
    /// <param name="pattern">The patter to be matched against the text.</param>
    /// <returns>The result of the pattern matching, successful or not.</returns>
    Match Match(RotatedTextWithTerminator sbwt, RotatedTextWithTerminator bwt, IEnumerable<char> pattern);
}
