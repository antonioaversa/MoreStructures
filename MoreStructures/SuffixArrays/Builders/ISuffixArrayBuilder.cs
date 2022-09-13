namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building the <see cref="SuffixArray"/> of a text.
/// </summary>
/// <example>
///     <code>
///     TextWithTerminator text = new("mississippi");
///     ISuffixArrayBuilder suffixArrayBuilder = ... // Some implementation of ISuffixArrayBuilder
///     var suffixArray = suffixArrayBuilder.Build().ToList(); // ToList necessary if Build is lazy
///     // suffixArray is new List&lt;int&gt; { 11, 10, 7, 4, ... }
///     </code>
/// </example>
public interface ISuffixArrayBuilder
{
    /// <summary>
    /// Builds the <see cref="SuffixArray"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="SuffixArray"/>, wrapping the indexes of the suffixes in a <see cref="IEnumerable{T}"/> of
    /// <see cref="int"/>.
    /// </returns>
    SuffixArray Build();
}
