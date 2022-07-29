using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.LongestCommonPrefix;

/// <summary>
/// An implementation of <see cref="ILcpArrayBuilder"/> which calculates the LCP Array using the definition.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - This implementation is the most straightforward, as the algorithm closely follows the definition.
///       <br/>
///     - As such it is easy to implement and analyze, at the cost of worse Time and Space complexity than smarter
///       implementations, using, for example, specific properties of subsequent LCP items of suffixes from the 
///       Suffix Array.
///       <br/>
///     - The algorithm is also online. Therefore, values can be lazily computed.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The implementation uses the externally provided <see cref="SuffixArray"/>, in combination with the 
///       <see cref="Text"/>.
///       <br/>
///     - It iterates over the first n - 1 items of the <see cref="SuffixArray"/> SA, using 
///       <see cref="StringUtilities.LongestCommonPrefix(IEnumerable{char}, IEnumerable{char})"/> to calculate the 
///       length of the LCP between SA[i] and its successor, SA[i + 1].
///       <br/>
///     - That represents the i-th element of the LCP Array.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The <see cref="SuffixArray"/> is externally provided, so it's building cost is not included in this analysis.
///       <br/>
///     - The algorithm runs n - 1 iterations, each one building two strings (the prefix starting at SA[i] and the one
///       starting at SA[i + 1]), then comparing them char by char, to find the LCP.
///       <br/>
///     - Prefixes have in general O(n) length, and 
///       <see cref="StringUtilities.LongestCommonPrefix(IEnumerable{char}, IEnumerable{char})"/> has Time Complexity 
///       linear in the input, and constant Space Complexity.
///       <br/>
///     - Therefore, Time Complexity is O(n^2) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class NaiveLcpArrayBuilder : ILcpArrayBuilder
{
    /// <summary>
    /// The terminator-terminated string, to calculate the <see cref="LcpArray"/> of.
    /// </summary>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// The Suffix Array of the <see cref="Text"/>, required to calculate the <see cref="LcpArray"/>.
    /// </summary>
    public SuffixArray SuffixArray { get; }

    /// <summary>
    ///     <inheritdoc cref="NaiveLcpArrayBuilder"/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="NaiveLcpArrayBuilder"/>
    /// </remarks>
    /// <param name="text">
    ///     <inheritdoc cref="Text" path="/summary"/>
    /// </param>
    /// <param name="suffixArray">
    ///     <inheritdoc cref="SuffixArray" path="/summary"/>
    /// </param>
    public NaiveLcpArrayBuilder(TextWithTerminator text, SuffixArray suffixArray)
    {
        Text = text;
        SuffixArray = suffixArray;
    }

    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="NaiveLcpArrayBuilder"/>
    /// </remarks>
    public virtual LcpArray Build() => 
        new(
            from suffixIndexStartingAndNext in SuffixArray.Indexes.Zip(SuffixArray.Indexes.Skip(1))
            let suffix = Text[suffixIndexStartingAndNext.First..]
            let nextSuffix = Text[suffixIndexStartingAndNext.Second..]
            select StringUtilities.LongestCommonPrefix(suffix, nextSuffix)
        );
}
