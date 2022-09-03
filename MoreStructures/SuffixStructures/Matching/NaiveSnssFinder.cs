using MoreStructures.Utilities;

namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// A <see cref="ISnssFinder"/> implementation which checks for the presence of each substring of the first text in the 
/// second text, from the longest to the shortest.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Unlike <see cref="SuffixStructureBasedSnssFinder"/> derivations, this implementation doesn't require 
///       terminators, since it does not build any auxialiary structure.
///       <br/>
///     - It also doesn't require additional space, except for iterations and index variables. So, it's a good 
///       solution for small input, when space is a hard constraint, much more than time.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - Lazily iterates over all substrings of text1, from the shortest (length 1) to the longest (length - 1).
///       <br/>
///     - As soon as it finds one which is not contained in text2, it yield returns it.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Checking all substrings of text1 of variable length, from 1 to length - 1, is a quadratic with the length of 
///       text1. It doesn't require more than constant space (for iterators and index variables) when using string
///       ranges (which are views of larger strings, optimized thanks to immutability.
///       <br/>
///     - Each check of a substring of text1 in text2 takes O(sl) time, where sl is the length of the substring. Since 
///       the average length of the substring depends linearly on the length of text1 n, the check takes O(n).
///       <br/>
///     - So overall Time Complexity is O(n^3) and Space Complexity is O(1).
///     </para>
/// </remarks>
public class NaiveSnssFinder : ISnssFinder
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaiveSnssFinder" path="/remarks"/>
    /// </remarks>
    public IEnumerable<string> Find(IEnumerable<char> text1, IEnumerable<char> text2)
    {
        var string1 = string.Concat(text1);
        var string2 = string.Concat(text2);

        if (string2.Contains(string1))
            return Enumerable.Empty<string>();

        var results =
            from length in Enumerable.Range(1, string1.Length) // All substrings of non-zero length
            from start in Enumerable.Range(0, string1.Length - length + 1)
            let substringOfString1 = string1[start..(start + length)]
            where !string2.Contains(substringOfString1)
            select substringOfString1;

        var (firstOrEmpty, _) = results.EnumerateAtMostFirst(1);
        var first = firstOrEmpty.Single();
        return results.TakeWhile(s => s.Length == first.Length).Prepend(first);
    }
}
