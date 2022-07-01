using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using MoreStructures.SuffixArrays;

namespace MoreStructures.BurrowsWheelerTransform.Matching.Comparers;

/// <summary>
/// A <see cref="SuffixAgainstPatternComparer"/> which uses the provided <see cref="IndexModKPartialSuffixArray"/> to
/// find the suffix of the provided <see cref="SuffixAgainstPatternComparer.Text"/>, to compare against the provided 
/// <see cref="SuffixAgainstPatternComparer.Pattern"/>.
/// </summary>
/// <remarks>
///     <para id="info">
///     - This is a generalization of <see cref="SuffixArrayAgainstPatternComparer"/>, which also supports Partial 
///       Suffix Arrays, i.e. Suffix Arrays with incomplete information, and in particular Partial Suffix Arrays of 
///       type <see cref="IndexModKPartialSuffixArray"/>.
///       <br/>
///     - When K = 1, an instance of <see cref="IndexModKPartialSuffixArray"/> contains the index of all suffixes, and
///       is equivalent to a <see cref="SuffixArray"/>.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - Start from the index I(0) = first term of comparison passed to <see cref="Compare(int, int)"/>.
///       <br/>
///     - Perform Last-First mapping from I(i) to I(i+1) from BWT to Sorted BWT, until the index I(i) is present in 
///       the Partial Suffix Array. While mapping, accumulate all indexes I(0), I(1), ... into a list I.
///       <br/>
///     - Once such index I(n) is found, iterate over I in reverse order, updating the Partial Suffix Array, from I(n)
///       all the way back to I(0).
///       <br/>
///     - Finally, invoke <see cref="SuffixAgainstPatternComparer.CompareSuffixAgainstPattern(int, int)"/> on the value
///       of the Partial Suffix Array at index I(0).
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - There are at most K iterations, where K is the modulo of the Partial Suffix Array: at each iteration, the
///       Last-First mapping finds the index I(i+1), in the Sorted BWT, of a suffix which augments the previous suffix
///       by 1 (prepending a single char).
///       <br/>
///     - So there are at most K iterations to update the Partial Suffix Array with I(n), I(n-1), ... I(0). The Partial
///       Suffix Array ends up having at most n elements (1 per suffix of the text).
///       <br/>
///     - Finally, there is <see cref="SuffixAgainstPatternComparer.CompareSuffixAgainstPattern(int, int)"/>.
///       <br/>
///     - Therefore, Time Complexity is O(n * K) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class IndexModKPartialSuffixArrayAgainstPatternComparer : SuffixAgainstPatternComparer
{
    private readonly IDictionary<int, int> PartialSuffixArrayIndexes;
    private readonly ILastFirstFinder LastFirstFinder;

    /// <summary>
    ///     <inheritdoc cref="IndexModKPartialSuffixArrayAgainstPatternComparer"/>
    /// </summary>
    /// <param name="text">The text, to extract suffixes from via <paramref name="partialSuffixArray"/>.</param>
    /// <param name="partialSuffixArray">
    /// The Partial Suffix Array of <paramref name="text"/>, to map (when the Partial Suffix Array contains such item) 
    /// the first term of comparison to the starting index in <paramref name="text"/> of the corresponding suffix.
    /// </param>
    /// <param name="pattern">The pattern, to compare against each suffix of <paramref name="text"/>.</param>
    /// <param name="bwt">
    /// The Burrows-Wheeler Transform of <paramref name="text"/>. Required to instantiate a 
    /// <see cref="ILastFirstFinder"/>, to fill-in the gaps of the <paramref name="partialSuffixArray"/>.
    /// </param>
    /// <param name="sbwt">
    /// The sorted version of <paramref name="bwt"/>. Required to instantiate a <see cref="ILastFirstFinder"/>, to 
    /// fill-in the gaps of the <paramref name="partialSuffixArray"/>.
    /// </param>
    public IndexModKPartialSuffixArrayAgainstPatternComparer(
        TextWithTerminator text, IndexModKPartialSuffixArray partialSuffixArray, IEnumerable<char> pattern,
        RotatedTextWithTerminator bwt, RotatedTextWithTerminator sbwt)
        : base(text, pattern)
    {
        PartialSuffixArrayIndexes = new Dictionary<int, int>(partialSuffixArray.Indexes);
        LastFirstFinder = new PrecomputedFinder(bwt, sbwt);
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]" />
    /// <summary>
    /// Compares the suffix of text identified by the <paramref name="x"/>-th element of the Suffix Array, against the 
    /// pattern. Covers the gap in the provided <see cref="IndexModKPartialSuffixArray"/> by iteratively using the 
    /// Last-First Property between BWT and its Sorted version.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="IndexModKPartialSuffixArrayAgainstPatternComparer" path="/remarks"/>
    /// </remarks>
    public override int Compare(int x, int y)
    {       
        // Apply Last-First property until we get an index of the Sorted BWT for which we have the corresponding
        // item in the Partial Suffix Array.
        var indexes = new List<int> { x };
        int suffixIndexStart;
        while (!PartialSuffixArrayIndexes.TryGetValue(indexes[^1], out suffixIndexStart))
            indexes.Add(LastFirstFinder.LastToFirst(indexes[^1]).indexInSortedBWT);

        for (var i = indexes.Count - 1; i >= 0; i--)
            PartialSuffixArrayIndexes[indexes[i]] = suffixIndexStart + (indexes.Count - 1 - i);

        return CompareSuffixAgainstPattern(x, PartialSuffixArrayIndexes[x]);
    }
}
