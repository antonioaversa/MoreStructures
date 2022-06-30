using MoreStructures.SuffixArrays;

namespace MoreStructures.BurrowsWheelerTransform.Matching.Comparers;

/// <summary>
/// A <see cref="SuffixAgainstPatternComparer"/> which compares the suffix <see cref="string"/> corresponding to the 
/// i-th element (first <see cref="int"/> value) of the provided <see cref="SuffixArray"/>, against the provided 
/// <see cref="string"/> pattern, and ignores the second <see cref="int"/> value.
/// </summary>
/// <remarks>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - When the provided <see cref="SuffixArray"/> instance has a <see cref="SuffixArray.Indexes"/> sequence which 
///       doesn't implement <see cref="IList{T}"/>, the <see cref="SuffixArray.Indexes"/> enumerable has to be 
///       enumerated, resulting into a list of exactly n elements.
///       <br/>
///     - Because a complete Suffix Array with direct memory access is provided to <see cref="Compare(int, int)"/>, 
///       getting the starting index of the suffix corresponding to the i-th element is a constant-time operation.
///       <br/>
///     - Compare a suffix against the pattern requires comparing at most n chars, where n is the length of 
///       <see cref="SuffixAgainstPatternComparer.Text"/>.
///       <br/>
///     - So Time and Space Complexity are both O(n) in the worst case. Space Complexity is O(1) when 
///       <see cref="SuffixArray.Indexes"/> implements <see cref="IList{T}"/>.
///     </para>
/// </remarks>
public class SuffixArrayAgainstPatternComparer : SuffixAgainstPatternComparer
{
    private readonly IList<int> SuffixArrayIndexes;

    /// <summary>
    ///     <inheritdoc cref="SuffixArrayAgainstPatternComparer"/>
    /// </summary>
    /// <param name="text">The text, to extract suffixes from via <paramref name="suffixArray"/>.</param>
    /// <param name="suffixArray">
    /// The <see cref="SuffixArray"/> of <paramref name="text"/>, to map the first term of comparison 
    /// to the starting index in <paramref name="text"/> of the corresponding suffix.
    /// </param>
    /// <param name="pattern">The pattern, to compare against each suffix of <paramref name="text"/>.</param>
    public SuffixArrayAgainstPatternComparer(
        TextWithTerminator text, SuffixArray suffixArray, IEnumerable<char> pattern)
        : base(text, pattern)
    {
        SuffixArrayIndexes = suffixArray.Indexes is IList<int> list ? list : suffixArray.Indexes.ToList();
    }

    /// <inheritdoc path="//*[not(self::summary)]" />
    /// <summary>
    /// Compares the suffix of text identified by the <paramref name="x"/>-th element of the 
    /// <see cref="SuffixArray"/> against the pattern.
    /// </summary>
    public override int Compare(int x, int y) => 
        CompareSuffixAgainstPattern(x, SuffixArrayIndexes[x]);
}
