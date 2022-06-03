using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using MoreStructures.Lists.Searching;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <summary>
/// A <see cref="NarrowingIntervalMatcher"/> refinement which precalculate the count of occurrences of each item at 
/// index i in the sub-sequence of items from 0 to i - 1, for both the BWT and its sorted version, and later uses it 
/// to perform operations in constant time.
/// </summary>
public class CountBasedNarrowingIntervalMatcher : NarrowingIntervalMatcher
{
    
}

/// <inheritdoc path="//*[not(self::remarks)]"/>
/// <remarks>
/// This is a basic implementation, using a narrowing interval.
/// </remarks>
public class NarrowingIntervalMatcher : IMatcher
{
    /// <summary>
    /// The <see cref="ISearch"/> implementation to be used when searching for items in lists sorted 
    /// in ascending order.
    /// </summary>
    protected static ISearch OrderedAscListSearch { get; } = new Lists.Searching.BinarySearch();

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
    public Match Match(RotatedTextWithTerminator bwt, RotatedTextWithTerminator sbwt, IEnumerable<char> pattern)
    {
        if (bwt.Terminator != sbwt.Terminator || bwt.Length != sbwt.Length)
            throw new ArgumentException($"{nameof(bwt)} and {nameof(sbwt)} are not consistent with each other.");
        if (!pattern.Any())
            throw new ArgumentException("The pattern should be non-empty.", nameof(pattern));
        
        var patternReversed = pattern.Reverse();
        var finder = new PrecomputedFinder(bwt, sbwt);
        var charComparer = CharOrTerminatorComparer.Build(bwt.Terminator);
        
        var (startIndex, endIndex) = OrderedAscListSearch.Interval(sbwt, patternReversed.First(), charComparer);
        if (startIndex < 0)
            return new Match(false, 0, -1, -1);

        var charsMatched = 1;
        foreach (var currentChar in patternReversed.Skip(1))
        {
            (var success, startIndex, endIndex) = NarrowInterval(bwt, finder, startIndex, endIndex, currentChar);

            if (!success)
                return new Match(false, charsMatched, startIndex, endIndex);

            charsMatched++;
        }

        return new Match(true, charsMatched, startIndex, endIndex);
    }

    /// <summary>
    /// Narrows the provided (<paramref name="startIndex"/>, <paramref name="endIndex"/>) interval, using the provided
    /// <see cref="ILastFirstFinder"/>.
    /// </summary>
    /// <param name="bwt">The Burrows-Wheeler Transform of the text.</param>
    /// <param name="finder">The finder used to perform last-first matching.</param>
    /// <param name="startIndex">The lower extreme of the interval to be narrowed.</param>
    /// <param name="endIndex">The higher extreme of the interval to be narrowed.</param>
    /// <param name="currentChar">The char currently being processed.</param>
    /// <returns>
    /// An interval narrower than the one provided as input, or (-1, -1), if narrowing resulted into an empty set.
    /// </returns>
    protected virtual (bool success, int narrowedStartIndex, int narrowedEndIndex) NarrowInterval(
        RotatedTextWithTerminator bwt, ILastFirstFinder finder, int startIndex, int endIndex, char currentChar)
    {
        var startIndexNarrowed = Enumerable
            .Range(startIndex, endIndex - startIndex + 1)
            .FirstOrDefault(i => bwt[i] == currentChar, -1);

        if (startIndexNarrowed == -1)
            return (false, startIndex, endIndex);

        var endIndexNarrowed = Enumerable
            .Range(startIndex, endIndex - startIndex + 1)
            .Reverse()
            .First(i => bwt[i] == currentChar);

        (startIndex, _) = finder.LastToFirst(startIndexNarrowed);
        (endIndex, _) = finder.LastToFirst(endIndexNarrowed);

        return (true, startIndex, endIndex);
    }
}
