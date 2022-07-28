namespace MoreStructures.SuffixArrays.LongestCommonPrefix;

/// <summary>
/// An algorithm finding the Longest Common Prefix (LCP) Array of a string.
/// </summary>
/// <remarks>
/// Check <see cref="LcpArray"/> for further information about LCP Arrays.
/// </remarks>
/// <example>
///     <code>
///     TextWithTerminator text = new("mississippi");
///     ILcpArrayBuilder lcpArrayBuilder = ... // Some implementation of ILcpArrayBuilder
///     var lcpArray = lcpArrayBuilder.Build().ToList(); // ToList necessary if Build is lazy
///     lcpArray is new List&lt;int&gt; { 0, 1, 1, ... }
///     </code>
/// </example>
public interface ILcpArrayBuilder
{
    /// <summary>
    /// Builds the <see cref="LcpArray"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="LcpArray"/>, wrapping the length of prefixes in common in a <see cref="IEnumerable{T}"/> of
    /// <see cref="int"/>.
    /// </returns>
    LcpArray Build();
}
