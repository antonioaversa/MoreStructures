using MoreStructures.Lists.Searching;
using MoreStructures.SuffixArrays.Builders;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A <see cref="IMatcher"/> implementationwhich uses a Suffix Array to perform text pattern matching.
/// </summary>
/// <remarks>
/// Suffix Array has to be provided as an additional input. 
/// <br/>
/// If not already available, It can be built via a <see cref="SuffixStructureBasedSuffixArrayBuilder{TEdge, TNode}"/>, 
/// if a Suffix Structure of the provided text is already available (such as a Suffix Tree or Trie), and via 
/// <see cref="NaiveSuffixArrayBuilder"/> otherwise.
/// <br/>
/// TODO: continue
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
    /// TODO
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
        var againstPatternComparerStart = new AgainstPatternComparer(Text, SuffixArray, pattern);
        var indexes = Enumerable.Range(0, SortedBWT.Length);
        var startIndex = OrderedAscListSearch.First(indexes, -1, againstPatternComparerStart);
        var longestMatchIndex1 = againstPatternComparerStart.LongestMatchFirstValue;

        var againstPatternComparerEnd = new AgainstPatternComparer(Text, SuffixArray, pattern);
        var endIndex = OrderedAscListSearch.Last(indexes, -1, againstPatternComparerEnd, startIndex);
        var longestMatchIndex2 = againstPatternComparerEnd.LongestMatchFirstValue;
        if (startIndex >= 0)
            return new Match(true, againstPatternComparerStart.LongestMatch, startIndex, endIndex);
        return new Match(false, againstPatternComparerStart.LongestMatch, longestMatchIndex1, longestMatchIndex2);
    }

    private class AgainstPatternComparer : IComparer<int>
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

            if (secondHasValue) return -1;
            return 0;
        }
    }
}
