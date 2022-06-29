using MoreStructures.Lists.Searching;
using MoreStructures.SuffixArrays;
using MoreStructures.SuffixArrays.Builders;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A <see cref="IMatcher"/> implementationwhich uses a <see cref="SuffixArrays.SuffixArray"/> to perform text pattern
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

    /// <inheritdoc/>
    /// <remarks>
    ///     Unlike <see cref="SortedBWT"/>, <see cref="BWT"/> is not required to perform Pattern Matching against
    ///     <see cref="Text"/>, and is not supported.
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
            var againstPatternComparerEnd = new AgainstPatternComparer(Text, SuffixArray, pattern);
            var endIndex = OrderedAscListSearch.Last(indexes, -1, againstPatternComparerEnd, startIndex);

            return new Match(true, againstPatternComparerStart.LongestMatch, startIndex, endIndex);
        }

        return new Match(false, againstPatternComparerStart.LongestMatch, longestMatchIndex1, longestMatchIndex1);
    }

    /// <summary>
    /// Builds a <see cref="AgainstPatternComparer"/> instance, or a derivation of it, to be used in
    /// <see cref="Match(IEnumerable{char})"/>, to find the start and end indexes in <see cref="SortedBWT"/>,
    /// corresponding to first and last matches of the pattern in <see cref="Text"/>.
    /// </summary>
    /// <param name="pattern"><inheritdoc cref="Match(IEnumerable{char})" path="/param[@name='pattern']"/></param>
    /// <returns>An instance of <see cref="AgainstPatternComparer"/> instance, or a derivation of it.</returns>
    protected virtual AgainstPatternComparer BuildComparer(IEnumerable<char> pattern)
    {
        return new AgainstPatternComparer(Text, SuffixArray, pattern);
    }

    /// <summary>
    /// An <see cref="IComparer{T}"/> of <see cref="int"/>, which compares the suffix <see cref="string"/> 
    /// corresponding to the i-th element (first <see cref="int"/> value) of the provided Suffix Array, against the 
    /// provided <see cref="string"/> pattern, and ignores the second <see cref="int"/> value.
    /// </summary>
    protected sealed class AgainstPatternComparer : IComparer<int>
    {
        private readonly TextWithTerminator Text;
        private readonly IList<int> SuffixArrayList;
        private readonly IEnumerable<char> Pattern;
        private readonly IComparer<char> CharComparer;

        /// <summary>
        /// The value of the first term of comparison, which resulted in <see cref="LongestMatch"/> chars matched,
        /// when comparing the suffix starting at <see cref="LongestMatchFirstValue"/> against the pattern.
        /// </summary>
        /// <remarks>
        /// If multiple values of the first term resulted in the same amount of chars matched, the first value
        /// encountered is kept.
        /// </remarks>
        public int LongestMatchFirstValue { get; private set; } = -1;

        /// <summary>
        /// The maximum amount of chars of the pattern matched, since the instantiation of this comparer.
        /// </summary>
        /// <remarks>
        /// It is never reset. To start over, a new instance of this comparer has to be created.
        /// </remarks>
        public int LongestMatch { get; private set; } = 0;

        /// <summary>
        ///     <inheritdoc cref="AgainstPatternComparer"/>
        /// </summary>
        /// <param name="text">The text, to extract suffixes from via <paramref name="suffixArray"/>.</param>
        /// <param name="suffixArray">
        /// The <see cref="SuffixArrays.SuffixArray"/> of <paramref name="text"/>, to map the first term of comparison 
        /// to the starting index in <paramref name="text"/> of the corresponding suffix.
        /// </param>
        /// <param name="pattern">The pattern, to compare against each suffix of <paramref name="text"/>.</param>
        public AgainstPatternComparer(TextWithTerminator text, SuffixArray suffixArray, IEnumerable<char> pattern)
        {
            Text = text;
            SuffixArrayList = suffixArray.Indexes is IList<int> list ? list : suffixArray.Indexes.ToList();
            Pattern = pattern;
            CharComparer = CharOrTerminatorComparer.Build(text.Terminator);
        }

        /// <summary>
        /// Compares the suffix of text identified by the <paramref name="x"/>-th element of the 
        /// <see cref="SuffixArrays.SuffixArray"/> against the pattern.
        /// </summary>
        /// <param name="x">
        /// The index in the <see cref="SuffixArrays.SuffixArray"/> of the suffix which is first term of comparison.
        /// </param>
        /// <param name="y">Ignored.</param>
        /// <returns>
        /// A positive value if there is mismatch and the suffix is bigger than the pattern lexicographically.
        /// <br/>
        /// A negative value if there is mismatch and the suffix is smaller than the pattern lexicographically.
        /// <br/>
        /// The value 0 if there is full match and pattern and text are of the same length or pattern is shorter.
        /// <br/>
        /// The value -1 if there is full match but the pattern is longer than the suffix.
        /// </returns>
        public int Compare(int x, int y)
        {
            using var firstEnumerator = Text[SuffixArrayList[x]..].GetEnumerator();
            using var secondEnumerator = Pattern.GetEnumerator();

            int numberOfItemsMatched = 0;
            bool firstHasValue, secondHasValue;
            while (true)
            {
                firstHasValue = firstEnumerator.MoveNext();
                secondHasValue = secondEnumerator.MoveNext();

                if (!firstHasValue || !secondHasValue)
                    break;

                var itemsComparison = CharComparer.Compare(firstEnumerator.Current, secondEnumerator.Current);
                if (itemsComparison != 0)
                    return itemsComparison;

                if (++numberOfItemsMatched > LongestMatch)
                {
                    LongestMatch = numberOfItemsMatched;
                    LongestMatchFirstValue = x;
                }
            }

            if (secondHasValue) 
                return -1; // The pattern is longer than the suffix

            // Either the pattern is shorter than the suffix, or pattern and suffix have equal length.
            // In either case, the match is successful.
            return 0; 
        }
    }
}

