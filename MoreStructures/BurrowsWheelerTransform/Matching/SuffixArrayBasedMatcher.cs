using MoreStructures.BurrowsWheelerTransform.Matching.Comparers;
using MoreStructures.Lists.Searching;
using MoreStructures.SuffixArrays;
using MoreStructures.SuffixArrays.Builders;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A <see cref="IMatcher"/> implementation which uses a <see cref="SuffixArrays.SuffixArray"/> to perform text pattern
/// matching.
/// </summary>
/// <remarks>
///     <para id="info">
///     <see cref="Text"/> and its <see cref="SuffixArrays.SuffixArray"/> has to be provided as an additional input. 
///     <br/>
///     If not already available, it can be built via a 
///     <see cref="SuffixStructureBasedSuffixArrayBuilder{TEdge, TNode}"/>, if a Suffix Structure of the provided text 
///     is already available (such as a Suffix Tree or Trie), and via <see cref="NaiveSuffixArrayBuilder"/> otherwise.
///     </para>
///     <para id="algo">
///     The algorithm performs two binary searches over the <see cref="SortedBWT"/> in sequence.
///     <br/>
///     - The first binary search looks for the first index i of the <see cref="SortedBWT"/> such that the provided 
///       pattern matches the i-th element of the <see cref="SuffixArrays.SuffixArray"/> (meaning that the pattern is 
///       fully contained in the i-th suffix of <see cref="Text"/> in <see cref="SuffixArray"/>).
///       <br/>
///     - If i is not found, a failing <see cref="Matching.Match"/> is returned. Otherwise, a second binary search is
///       performed.
///       <br/>
///     - The second binary search looks for the last index j of the <see cref="SortedBWT"/> respecting the same 
///       condition as the first binary search. This search starts from the index found in the first search (included).
///       <br/>
///     - The second search is guaranteed to succeed (as in the worst case the last occurrence corresponds to the first
///       one, found in the first search.     
///     </para>
///     <para id="complexity">
///     - The array of indexes is as long as the length n of <see cref="SortedBWT"/>, which is also the length of the 
///       text and the <see cref="SuffixArrays.SuffixArray"/>.
///       <br/>
///     - Each binary search does a number of comparisons which is logarithmic over n.
///       <br/>
///     - Each comparison is of at most n chars.
///       <br/>
///     - So Time Complexity is O(n * log(n)) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class SuffixArrayBasedMatcher : IMatcher
{
    /// <summary>
    /// The <see cref="ISearch"/> implementation to be used when searching for items in lists sorted in ascending 
    /// order.
    /// </summary>
    protected static ISearch OrderedAscListSearch { get; } = new BinarySearch();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Unlike <see cref="SortedBWT"/>, <see cref="BWT"/> is not required to perform Pattern Matching against
    /// <see cref="Text"/>, and is not supported.
    /// </remarks>
    public RotatedTextWithTerminator BWT => 
        throw new NotSupportedException($"{nameof(BWT)} is not defined for {nameof(SuffixArrayBasedMatcher)}");

    /// <inheritdoc/>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// The <see cref="TextWithTerminator"/>, to do pattern matching against.
    /// </summary>
    /// <remarks>
    /// Requires to get the actual suffix from the i-th element of <see cref="SuffixArray"/>, to be compared against
    /// the pattern.
    /// </remarks>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// The <see cref="SuffixArrays.SuffixArray"/> of <see cref="Text"/>.
    /// </summary>
    public SuffixArray SuffixArray { get; }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="SuffixArrayBasedMatcher" path="/remarks"/>
    /// </remarks>
    public SuffixArrayBasedMatcher(RotatedTextWithTerminator sbwt, TextWithTerminator text, SuffixArray suffixArray)
    {
        SortedBWT = sbwt;
        Text = text;
        SuffixArray = suffixArray;
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// Tries to match the provided <paramref name="pattern"/> against <see cref="Text"/>, via the 
    /// <see cref="SortedBWT"/> and the <see cref="SuffixArray"/> of <see cref="Text"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="SuffixArrayBasedMatcher" path="/remarks"/>
    /// </remarks>
    public Match Match(IEnumerable<char> pattern)
    {
        var indexes = Enumerable.Range(0, SortedBWT.Length).ToList();
        var againstPatternComparerStart = BuildComparer(pattern);
        var startIndex = OrderedAscListSearch.First(indexes, -1, againstPatternComparerStart);
        var longestMatchIndex1 = againstPatternComparerStart.LongestMatchFirstValue;

        if (startIndex >= 0)
        {
            var againstPatternComparerEnd = new SuffixArrayAgainstPatternComparer(Text, SuffixArray, pattern);
            var endIndex = OrderedAscListSearch.Last(indexes, -1, againstPatternComparerEnd, startIndex);

            return new Match(true, againstPatternComparerStart.LongestMatch, startIndex, endIndex);
        }

        return new Match(false, againstPatternComparerStart.LongestMatch, longestMatchIndex1, longestMatchIndex1);
    }

    /// <summary>
    /// Builds a <see cref="SuffixArrayAgainstPatternComparer"/> instance, or a derivation of it, to be used in
    /// <see cref="Match(IEnumerable{char})"/>, to find the start and end indexes in <see cref="SortedBWT"/>,
    /// corresponding to first and last matches of the pattern in <see cref="Text"/>.
    /// </summary>
    /// <param name="pattern"><inheritdoc cref="Match(IEnumerable{char})" path="/param[@name='pattern']"/></param>
    /// <returns>An instance of <see cref="SuffixArrayAgainstPatternComparer"/> instance, or a derivation of it.</returns>
    protected virtual SuffixArrayAgainstPatternComparer BuildComparer(IEnumerable<char> pattern) => 
        new(Text, SuffixArray, pattern);
}
