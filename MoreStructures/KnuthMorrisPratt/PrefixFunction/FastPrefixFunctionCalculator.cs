using MoreStructures.KnuthMorrisPratt.Borders;

namespace MoreStructures.KnuthMorrisPratt.PrefixFunction;

/// <inheritdoc cref="IPrefixFunctionCalculator" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// An implementation of <see cref="IPrefixFunctionCalculator"/> which takes advantage of the property that any 
/// non-longest border is a also a border of the longest border, to calculate the Prefix Function efficiently.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The algorithm iterates over all the chars of the text, from the first to the last, calculating the Prefix
///       Function s for each index i.
///       <br/>
///     - The Prefix Function at index 0, <c>s(0) = 0</c>, since a 1-char string can't have any border.
///       <br/>
///     - For each any other index, it calculates the value of the Prefix Function based on values of s at lower 
///       indexes and based on T. 
///       <br/>
///     - if <c>T[i] = T[s(i - 1)]</c>, where <c>s(i - 1)</c> has been calculated at the previous step, then 
///       <c>s(i) = s(i - 1) + 1</c>.
///       <br/>
///     - This is because <c>T[0..s(i - 1)]</c> is, by definition of s, a border of <c>T[0..i]</c> and, if 
///       <c>T[i] = T[s(i - 1)]</c>, then <c>T[0..s(i - 1) + 1] = T[i - s(i)..i]</c>. So <c>T[0..s(i - 1) + 1]</c> is a
///       border of <c>T[0..(i + 1)]</c> and there can't be any longer border, since <c>s(i + 1) &lt;= s(i) + 1</c> 
///       (the Prefix Function can grow of at most 1 at each index).
///       <br/>
///     - if <c>T[i] != T[s(i - 1)]</c>, <c>s(i)</c> will be smaller than <c>s(i - 1) + 1</c>. While s can increase of
///       at most 1 at each index, it can decrease to 0 in a single step (if T[i] is such that there are no borders of
///       T[0..(i + 1)]).
///       <br/>
///     - Because a border of T[0..(i + 1)] (prefix of T including up to the i-th char) is also a border of T[0..i]
///       (prefix of T at previous step, i.e. including up to the (i - 1)-th char), EXCEPT for the char T[i], which 
///       would come just after the border of T[0..i], the longest border of T[0..(i + 1)] (whose length is s(i)), 
///       can be found by looking for the longest border of T[0..i] followed by T[i].
///       <br/>
///     - If such a border of length w is found, then there is a border of T[0..i], T[0..w], followed by T[w] = T[i].
///       <br/>
///     - Which means that <c>T[0..(w + 1)] == T[(i - w)..(i + 1)]</c>.
///       <br/>
///     - Therefore, T[0..(w + 1)] is a border of T[0..(i + 1)] and <c>s(i + 1) = w</c>.
///       <br/>
///     - If no borders are found, the Prefix Function at index i is zero.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - There are as many top-level iterations as indexes of the Prefix Function and chars of the text.
///       <br/>
///     - For each top-level iteration, there are at most as many sub-iterations as borders of the prefix, each one
///       checking a single char of the text. In the worst case there are O(n) borders, meaning that each sub-iteration
///       requires O(n) char comparisons.
///       <br/>
///     - That would seem to imply that the number of sub-iterations in total, throughout all top-level iterations,
///       would be quadratic.
///       <br/>
///     - However, the length of the border calculated by the inner loop is always non-negative and is increased of at
///       most n times (since it is increased of at most 1 per char of text).
///       <br/>
///     - Moreover, it is progressively shorten by the inner loop, decreased of at least 1 per sub-iteration. This is 
///       because, when the char following the current border is different than the current char (which is the 
///       condition of the inner loop), the next border is the longest border of the current border, which is strictly
///       shorter than the current border.
///       <br/>
///     - Therefore the length of the border can reach at most n thoughout all top-level iterations, and is always
///       non-negative and decreased by 1 at each sub-iteration, which means that the total number of inner loop
///       iterations through all top-level iterations is O(n).
///       <br/>
///     - All accesses to values calculated in previous steps and to chars of the text are done in constant time.
///       <br/>
///     - An array of integers, as big as the numbers of chars in the text, is allocated at the beginning, to memoize
///       the value calculated at each step, since that value may be used in following iterations.
///       <br/>
///     - No other data structure is allocated by the algorithm.
///       <br/>
///     - Therefore, Time Complexity is O(n) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class FastPrefixFunctionCalculator : IPrefixFunctionCalculator
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="FastPrefixFunctionCalculator" path="/remarks"/>
    /// </remarks>
    public IEnumerable<int> GetValues(string text)
    {
        if (text.Length == 0)
            yield break;

        var s = new int[text.Length];
        var i = 0;

        // The 1-char prefix of text cannot have any border => s[0] = 0
        s[0] = 0;
        yield return s[i++];

        // All indexes > 0 are calculated based on results from previous steps
        while (i < text.Length)
        {
            var w = s[i - 1];
            var found = text[w] == text[i];
            while (w > 0 && !found)
            {
                w = s[w - 1];
                found = text[w] == text[i];
            }

            yield return s[i] = (found ? w + 1 : 0);

            i++;
        }
    }
}
