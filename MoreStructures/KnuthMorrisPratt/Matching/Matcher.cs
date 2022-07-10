using MoreStructures.KnuthMorrisPratt.PrefixFunction;
using MoreStructures.Strings;

namespace MoreStructures.KnuthMorrisPratt.Matching;

/// <summary>
/// Exposes utility methods to match a text against a pattern, using the Knuth-Morris-Pratt algorithm.
/// </summary>
public static class Matcher
{
    private static readonly IPrefixFunctionCalculator PrefixFunctionCalculator = new FastPrefixFunctionCalculator();

    /// <summary>
    /// Tries to match the provided <paramref name="pattern"/> against the provided <paramref name="text"/>, using the
    /// Knuth-Morris-Pratt algorithm. Returns all matches found.
    /// </summary>
    /// <param name="text">The text, to match the pattern against.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="separator">
    /// A special character, to be used as separator in the concatenated string, between <paramref name="pattern"/> and
    /// <paramref name="text"/>. The special character is required by the algorithm and must be absent from both the 
    /// <paramref name="pattern"/> and the <paramref name="text"/>.
    /// </param>
    /// <returns>
    /// A sequence of all matches, from the one starting at the lowest index in <paramref name="text"/> to the one
    /// starting at the highest index.
    /// </returns>
    /// <remarks>
    ///     <para id="algo">
    ///     ALGORITHM
    ///     <br/>
    ///     The Knuth-Morris-Pratt algorithm:
    ///     <br/>
    ///     - First builds a concatenated string in the form "pattern-separator-text".
    ///       <br/>
    ///     - Then calculates the Prefix Function of the concatenated string.
    ///       <br/>
    ///     - Finally, iterates over the Prefix Function, retaining all indexes i such that the value of the Prefix
    ///       Function at i is equal to the length of the pattern. That means that the prefix of the concatenated
    ///       string from 0 to i (included) has a border of length equal to the length of the pattern, which means that
    ///       the prefix contains the pattern.
    ///       <br/>
    ///     - For each retained index i, a successful match is emitted, starting at index i -  2 * m and matching 
    ///       exactly m chars, where m is the length of the pattern.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - The concatenated string has length n + m + 1, where n is the length of the text and m is the length of
    ///       the pattern. Building it requires iterating both over the pattern and the text.
    ///       <br/>
    ///     - Calculating the Prefix Function of the concatenated string via <see cref="FastPrefixFunctionCalculator"/>
    ///       is a O(n + m + 1) operation, both in time and space.
    ///       <br/>
    ///     - Iterating over the indexes of the Prefix Function which refer to the text is an O(n) operation.
    ///       <br/>
    ///     - Each iteration makes a constant-time comparison and yield the result.
    ///       <br/>
    ///     - Therefore, both Time and Space Complexity are O(n + m).
    ///     </para>
    /// </remarks>
    public static IEnumerable<Match<int>> Match(string text, string pattern, char separator)
    {
        var patternAndText = string.Concat(pattern, separator, text);
        var prefixFunction = PrefixFunctionCalculator.GetValues(patternAndText).ToList();
        var m = pattern.Length;

        for (var i = m + 1; i < patternAndText.Length; i++)
            if (prefixFunction[i] == m)
                yield return new Match<int>(true, i - m * 2, m, i);
    }
}
