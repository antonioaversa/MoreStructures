namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// A <see cref="ISnssFinder"/> implementation which checks for the presence of each substring of the first text in the 
/// second text, from the longest to the shortest.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - It doesn't require additional space, except for iterations and index variables. So, it's a good 
///       solution for small input, when space is a hard constraint, much more than time.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - Lazily iterates over all substrings of text1, from the shortest (length 1) to the longest (length - 1).
///       <br/>
///     - As soon as it finds one which is not contained in text2, it returns it.
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
    public string? Find(IEnumerable<char> text1, IEnumerable<char> text2)
    {
        var string1 = string.Concat(text1);
        var string2 = string.Concat(text2);

        if (string2.Contains(string1))
            return null;

        return (
            from i in Enumerable.Range(1, string1.Length - 1)
            from j in Enumerable.Range(0, string1.Length - i + 1)
            let substringOfString1 = string1[j..(j + i)]
            where !string2.Contains(substringOfString1)
            select substringOfString1)
            .FirstOrDefault();
    }
}
