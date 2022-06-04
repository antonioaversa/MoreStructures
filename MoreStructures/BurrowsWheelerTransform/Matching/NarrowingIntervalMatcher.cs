using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using MoreStructures.Lists.Searching;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <inheritdoc path="//*[not(self::remarks)]"/>
/// <remarks>
/// This is a basic implementation, narrowing the matching interval at every iteration with two linear scans of BWT.
/// <br/>
/// Time Complexity = O(n^2). Space Complexity = O().
/// </remarks>
public class NarrowingIntervalMatcher : IMatcher
{
    /// <summary>
    /// The <see cref="ISearch"/> implementation to be used when searching for items in lists sorted 
    /// in ascending order.
    /// </summary>
    protected static ISearch OrderedAscListSearch { get; } = new BinarySearch();

    /// <inheritdoc/>
    public RotatedTextWithTerminator BWT { get; }

    /// <inheritdoc/>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// Builds an instance of this matcher, for the provided <paramref name="bwt"/>, using <paramref name="bwtSorter"/>
    /// to calculate <see cref="SortedBWT"/> from it.
    /// </summary>
    /// <param name="bwt">The last column of the Burrows-Wheeler Matrix. Corresponds to the BWT.</param>
    /// <param name="bwtSorter">
    /// A function sorting the provided<see cref="RotatedTextWithTerminator"/> into a sorted
    /// <see cref="RotatedTextWithTerminator"/>, according to the provided <see cref="IComparer{T}"/> of chars.
    /// </param>
    public NarrowingIntervalMatcher(RotatedTextWithTerminator bwt, BWTransform.SortStrategy bwtSorter)
    {
        BWT = bwt;
        SortedBWT = bwtSorter(BWT, CharOrTerminatorComparer.Build(bwt.Terminator));
    }

    /// <summary>
    /// Builds an instance of this finder, for the provided <paramref name="bwt"/> and 
    /// <paramref name="sbwt"/>. Because both BWT and SortedBWT are provided, no sorting happens.
    /// </summary>
    /// <param name="bwt">
    /// The Burrows-Wheeler Transform.
    /// </param>
    /// <param name="sbwt">
    /// The sorted version of the Burrows-Wheeler Transform.
    /// </param>
    public NarrowingIntervalMatcher(RotatedTextWithTerminator bwt, RotatedTextWithTerminator sbwt)
    {
        if (bwt.Terminator != sbwt.Terminator || bwt.Length != sbwt.Length)
            throw new ArgumentException($"{nameof(bwt)} and {nameof(sbwt)} are not consistent with each other.");

        BWT = bwt;
        SortedBWT = sbwt;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// The pattern matching is done via successive narrowing of a interval, defined by a start and an end index.<br/>
    /// 1. At the beginning the interval is as big as the provided <see cref="BWTransform"/> (and its text).
    ///    <br/>
    /// 2. The algorithmn proceeds in reverse: from the last char of the pattern P, P[^1] to the first, P[0].
    ///    <br/>
    /// 3. Search in Sorted BWT for the range of indexes (first1, last1) having value P[^1] via a <see cref="ISearch"/>
    ///    implementation (because the input is sorted, binary search is possible).
    ///    <br/>
    /// 4. The char in BWT at indexes first1 and last1 represent the predecessor of all instances of P[^1] in P. The 
    ///    interval (first1, last1) can then be narrowed down to (first2, last2), taking into account only the chars
    ///    in BWT which match the predecessor of P[^1], P[^2].
    ///    <br/>
    /// 5. By last-first property, new indexes (first3, last3) of the chars in Sorted BWT corresponding to first2 and 
    ///    last2 in BWT, can be found. Those are the first and last of the new narrowed range, ready for step 4.
    /// 6. When all chars of P, up to P[0], have been consumed, all matches have been identified as an interval in
    ///    Sorted BWT.
    /// </remarks>
    public Match Match(IEnumerable<char> pattern)
    {
        if (!pattern.Any())
            throw new ArgumentException("The pattern should be non-empty.", nameof(pattern));
        
        var patternReversed = pattern.Reverse();
        var finder = BuildLastFirstFinder();
        var charComparer = CharOrTerminatorComparer.Build(BWT.Terminator);
        
        var (startIndex, endIndex) = OrderedAscListSearch.Interval(SortedBWT, patternReversed.First(), charComparer);
        if (startIndex < 0)
            return new Match(false, 0, -1, -1);

        var charsMatched = 1;
        foreach (var currentChar in patternReversed.Skip(1))
        {
            (var success, startIndex, endIndex) = NarrowInterval(currentChar, finder, startIndex, endIndex);

            if (!success)
                return new Match(false, charsMatched, startIndex, endIndex);

            charsMatched++;
        }

        return new Match(true, charsMatched, startIndex, endIndex);
    }

    /// <summary>
    /// Builds the <see cref="ILastFirstFinder"/> instance which is then (potentially) used by all iterations of the 
    /// matching algorithm over the pattern to match against <see cref="IMatcher.BWT"/> and 
    /// <see cref="IMatcher.SortedBWT"/>.
    /// </summary>
    /// <returns>An instance of <see cref="ILastFirstFinder"/>.</returns>
    /// <remarks>
    /// The <see cref="ILastFirstFinder"/> implementation used is <see cref="PrecomputedFinder"/>.
    /// </remarks>
    protected virtual ILastFirstFinder BuildLastFirstFinder() => new PrecomputedFinder(BWT, SortedBWT);

    /// <summary>
    /// Narrows the provided (<paramref name="startIndex"/>, <paramref name="endIndex"/>) interval, (possibly) using 
    /// the provided <see cref="ILastFirstFinder"/> <paramref name="finder"/> for last-first matching.
    /// </summary>
    /// <param name="currentChar">The char currently being processed.</param>
    /// <param name="finder">
    /// The finder used to perform last-first matching, if needed. When pattern matching a single instance is shared 
    /// across all iterations over the pattern.
    /// </param>
    /// <param name="startIndex">The lower extreme of the interval to be narrowed.</param>
    /// <param name="endIndex">The higher extreme of the interval to be narrowed.</param>
    /// <returns>
    /// An interval narrower than the one provided as input, or (-1, -1), if narrowing resulted into an empty set
    /// (i.e. overall matching has failed).
    /// </returns>
    /// <remarks>
    /// Narrowing is performed in five sub-steps:
    /// <br/>
    /// 1. a linear scan in BWT from <paramref name="startIndex"/> downwards is done, to identify the narrowed start;
    /// <br/>
    /// 2. a linear scan in BWT from <paramref name="endIndex"/> upwards is done, to identify the narrowed end;
    /// <br/>
    /// 3. a last-first of the narrowed start is done, to find the corresponding narrowed start in the Sorted BWT;
    /// <br/>
    /// 4. a last-first of the narrowed end is done, to find the corresponding narrowed end in the Sorted BWT;
    /// <br/>
    /// 5. the narrowed interval in Sorted BWT is returned.
    /// </remarks>
    protected virtual (bool success, int narrowedStartIndex, int narrowedEndIndex) NarrowInterval(
        char currentChar, ILastFirstFinder finder, int startIndex, int endIndex)
    {
        var startIndexNarrowed = Enumerable
            .Range(startIndex, endIndex - startIndex + 1)
            .FirstOrDefault(i => BWT[i] == currentChar, -1);

        if (startIndexNarrowed == -1)
            return (false, startIndex, endIndex);

        var endIndexNarrowed = Enumerable
            .Range(startIndex, endIndex - startIndex + 1)
            .Reverse()
            .First(i => BWT[i] == currentChar);

        (startIndex, _) = finder.LastToFirst(startIndexNarrowed);
        (endIndex, _) = finder.LastToFirst(endIndexNarrowed);

        return (true, startIndex, endIndex);
    }
}
