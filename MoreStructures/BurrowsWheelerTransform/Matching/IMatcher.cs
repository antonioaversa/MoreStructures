namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// An implementation of matching of a pattern against <see cref="RotatedTextWithTerminator"/> instances, containing
/// the <see cref="BWT"/> (Burrows-Wheeler Transform) of a text and its sorted version <see cref="SortedBWT"/>.
/// </summary>
public interface IMatcher
{
    /// <summary>
    /// The Burrows-Wheeler Transform. Also, the last column of the Burrows-Wheeler Matrix.
    /// </summary>
    /// <example>
    /// The Burrows-Wheeler Transform of "mississippi$" is "ipssm$pissii".
    /// </example>
    public RotatedTextWithTerminator BWT { get; }

    /// <summary>
    /// The sorted version of the Burrows-Wheeler Transform. Also, the first column of the Burrows-Wheeler Matrix.
    /// </summary>
    /// <example>
    /// The Sorted Burrows-Wheeler Transform of "mississippi$" is "$iiiimppssss".
    /// </example>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// Tries to match the provided <paramref name="pattern"/> against the text, via the <see cref="BWT"/> and its 
    /// sorted version <see cref="SortedBWT"/>.
    /// </summary>
    /// <param name="pattern">The patter to be matched against the text.</param>
    /// <returns>The result of the pattern matching, successful or not.</returns>
    /// <example>
    ///     <code>
    ///     var matcher = ... { BWT = "ipssm$pissii", SortedBWT = "$iiiimppssss" };
    ///     var match = matcher.Match("issi");
    ///     // match: Success == true, MatchedChars = 4, StartIndex = 3, EndIndex = 4
    ///     // 0: $
    ///     // 1: i$
    ///     // 2: ippi$
    ///     // 3: issippi$      &lt;- StartIndex 
    ///     // 4: ississippi$   &lt;- EndIndex
    ///     // 5: mississippi$
    ///     // ...
    ///     </code>
    /// </example>
    Match Match(IEnumerable<char> pattern);
}
