using System.Collections.Generic;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A implementation of matching of a pattern against a <see cref="RotatedTextWithTerminator"/> containing a
/// Burrows-Wheeler Transform of a text.
/// </summary>
public interface IMatcher
{
    /// <summary>
    /// The Burrows-Wheeler Transform. Also, the last column of the Burrows-Wheeler Matrix.
    /// </summary>
    public RotatedTextWithTerminator BWT { get; }

    /// <summary>
    /// The sorted version of the Burrows-Wheeler Transform. Also, the first column of the Burrows-Wheeler Matrix.
    /// </summary>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// Tries to match the provided <paramref name="pattern"/> against the <see cref="BWT"/> and its sorted
    /// version <see cref="SortedBWT"/>.
    /// </summary>
    /// <param name="pattern">The patter to be matched against the text.</param>
    /// <returns>The result of the pattern matching, successful or not.</returns>
    Match Match(IEnumerable<char> pattern);
}
