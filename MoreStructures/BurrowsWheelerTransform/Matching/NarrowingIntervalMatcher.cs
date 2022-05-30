using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using MoreStructures.Lists.Searching;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Matching;

/// <inheritdoc path="//*[not(self::remarks)]"/>
/// <remarks>
/// This is a basic implementation, using a narrowing interval.
/// </remarks>
public class NarrowingIntervalMatcher : IMatcher
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// The pattern matching is done via successive narrowing of a interval, defined by a start and an end index.<br/>
    /// 1. At the beginning the interval is as big as the provided <see cref="BWTransform"/> (and its text).
    ///    <br/>
    /// 2. The algorithmn proceeds in reverse: from the last char of the pattern P, P[^1] to the first, P[0].
    ///    <br/>
    /// 3. Binary search in Sorted BWT for the range of indexes (first1, last1) having value P[^1].
    ///    <br/>
    /// 4. The char in BWT at indexes first1 and last1 represent the predecessor of all instances of P[^1] in P. The 
    ///    interval (first1, last1) can then be narrowed down to (first2, last2), taking into account only the chars
    ///    in BWT which match the predecessor of P[^1], P[^2].
    ///    <br/>
    /// 5. By last-first property, new indexes (first3, last3) of the chars in Sorted BWT corresponding to first2 and 
    ///    last2 in BWT, can be found. Those are the first and last of the new narrowed range, ready for step 4.
    /// 6. When all chars of P, up to P[0], have been consumed, all matches have been identified.
    /// </remarks>
    public Match Match(RotatedTextWithTerminator sbwt, RotatedTextWithTerminator bwt, IEnumerable<char> pattern)
    {
        if (bwt.Terminator != sbwt.Terminator || bwt.Length != sbwt.Length)
            throw new ArgumentException($"{nameof(bwt)} and {nameof(sbwt)} are not consistent with each other.");
        if (!pattern.Any())
            throw new ArgumentException("The pattern should be non-empty.", nameof(pattern));
        
        var patternReversed = pattern.Reverse();
        var finder = new PrecomputedFinder(sbwt, bwt);
        var charComparer = CharOrTerminatorComparer.Build(bwt.Terminator);
        
        var (startIndex, endIndex) = Search.BinarySearchInterval(sbwt, patternReversed.First(), charComparer);
        if (startIndex < 0)
            return new Match(false, 0);

        var occurrenceStart = 0; // Occurrence is 0-based: 0 is 1st occurrence, 1 is 2nd occurrence, ...
        var occurrenceEnd = endIndex - startIndex;
        var charsMatched = 1;
        foreach (var c in patternReversed.Skip(1))
        {
            var startIndex1 = Enumerable
                .Range(startIndex, endIndex - startIndex + 1)
                .FirstOrDefault(i => bwt[i] == c, -1);
            if (startIndex1 == -1)
                return new Match(false, charsMatched);

            var endIndex1 = Enumerable
                .Range(startIndex, endIndex - startIndex + 1)
                .Reverse()
                .First(i => bwt[i] == c);

            startIndex = finder.FindIndexOfNthOccurrenceInBWT(c, occurrenceStart);
            endIndex = finder.FindIndexOfNthOccurrenceInBWT(c, occurrenceEnd);
            occurrenceStart = finder.FindOccurrenceOfCharInSortedBWT(startIndex);
            occurrenceEnd = finder.FindOccurrenceOfCharInSortedBWT(endIndex);
            charsMatched++;
        }

        return new Match(true, charsMatched);
    }
}
