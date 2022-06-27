using MoreStructures.Lists.Searching;
using MoreStructures.SuffixArrays.Builders;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A <see cref="IMatcher"/> implementationwhich uses a Suffix Array to perform text pattern matching.
/// </summary>
/// <remarks>
///     <para id="info">
///     Suffix Array has to be provided as an additional input. 
///     <br/>
///     If not already available, it can be built via a 
///     <see cref="SuffixStructureBasedSuffixArrayBuilder{TEdge, TNode}"/>, if a Suffix Structure of the provided text 
///     is already available (such as a Suffix Tree or Trie), and via <see cref="NaiveSuffixArrayBuilder"/> otherwise.
///     </para>
///     <para id="algo">
///     The algorithm performs two binary searches over the <see cref="SortedBWT"/> in sequence.
///     <br/>
///     - The first binary search looks for the first index i of the <see cref="SortedBWT"/> such that the provided 
///       pattern matches the i-th element of the <see cref="SuffixArray"/> (meaning that the pattern is fully 
///       contained in the i-th suffix of <see cref="Text"/> in <see cref="SuffixArray"/>).
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
///       text and the Suffix Array.
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
    /// The <see cref="ISearch"/> implementation to be used when searching for items in lists sorted 
    /// in ascending order.
    /// </summary>
    protected static ISearch OrderedAscListSearch { get; } = new BinarySearch();

    /// <inheritdoc/>
    public RotatedTextWithTerminator BWT => 
        throw new NotSupportedException($"{nameof(BWT)} is not defined for {nameof(SuffixArrayBasedMatcher)}");

    /// <inheritdoc/>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// The <see cref="TextWithTerminator"/>, to do pattern matching against.
    /// </summary>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// The Suffix Array of <see cref="Text"/>.
    /// </summary>
    public IList<int> SuffixArray { get; }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="SuffixArrayBasedMatcher" path="/remarks"/>
    /// </remarks>
    public SuffixArrayBasedMatcher(RotatedTextWithTerminator sbwt, TextWithTerminator text, IList<int> suffixArray)
    {
        SortedBWT = sbwt;
        Text = text;
        SuffixArray = suffixArray;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Match Match(IEnumerable<char> pattern)
    {
        var indexes = Enumerable.Range(0, SortedBWT.Length).ToList();

        var againstPatternComparerStart = new AgainstPatternComparer(Text, SuffixArray, pattern);
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

    private sealed class AgainstPatternComparer : IComparer<int>
    {
        private readonly TextWithTerminator Text;
        private readonly IList<int> SuffixArray;
        private readonly IEnumerable<char> Pattern;
        private readonly IComparer<char> CharComparer;

        public int LongestMatchFirstValue { get; private set; } = -1;
        public int LongestMatch { get; private set; } = 0;

        public AgainstPatternComparer(TextWithTerminator text, IList<int> suffixArray, IEnumerable<char> pattern)
        {
            Text = text;
            SuffixArray = suffixArray;
            Pattern = pattern;
            CharComparer = CharOrTerminatorComparer.Build(text.Terminator);
        }

        public int Compare(int x, int y)
        {
            using var firstEnumerator = Text[SuffixArray[x]..].GetEnumerator();
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
                return -1; // The patter is longer than the suffix
            return 0;
        }
    }
}
