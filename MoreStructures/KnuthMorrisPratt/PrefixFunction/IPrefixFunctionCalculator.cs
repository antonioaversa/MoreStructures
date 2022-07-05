namespace MoreStructures.KnuthMorrisPratt.PrefixFunction;

/// <summary>
/// An algorithm which calculates the <b>Prefix Function</b> of the provided text.
/// </summary>
/// <remarks>
///     <para id="definition">
///     The Prefix Function of a text T is a function s such that s(i) is the length of the longest border of the 
///     prefix of T up to the character with index i, included, i.e. T[0..(i+1)].
///     <br/>
///     For example, if <c>T = "aabaabacaabaa"</c>:
///     <br/>
///     - <c>s(0) = 0</c>, since <c>T[0..1] = "a"</c> has no borders;
///       <br/>
///     - <c>s(1) = 1</c>, since <c>T[0..2] = "aa"</c> has a single border <c>"a"</c>, which is of length 1;
///       <br/>
///     - <c>s(2) = 0</c>, since <c>T[0..3] = "aab"</c> has no borders;
///       <br/>
///     - <c>s(3) = 1</c>, since <c>T[0..4] = "aaba"</c> has a single border <c>"a"</c>, which is of length 1;
///       <br/>
///     - <c>s(4) = 1</c>, since <c>T[0..5] = "aabaa"</c> has 2 borders <c>{ "a", "aa" }</c>, and the longest one is of 
///       length 2;
///       <br/>
///     - <c>s(5) = 1</c>, since <c>T[0..6] = "aabaab"</c> has 3 borders <c>{ "a", "aa", "aab" }</c>, and the longest 
///       one of length 3;
///       <br/>
///     - etc.
///     </para>
/// </remarks>
public interface IPrefixFunctionCalculator
{
    /// <summary>
    /// Calculate the values of the Prefix Function of the provided <paramref name="text"/>.
    /// </summary>
    /// <param name="text">The text, to calculate the Prefix Function of.</param>
    /// <returns>The sequence of <see cref="int"/> values of the Prefix Function.</returns>
    IEnumerable<int> GetValues(string text);
}
