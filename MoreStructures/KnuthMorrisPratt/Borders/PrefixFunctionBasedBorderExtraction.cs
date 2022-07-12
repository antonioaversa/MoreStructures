using MoreStructures.KnuthMorrisPratt.PrefixFunction;

namespace MoreStructures.KnuthMorrisPratt.Borders;

/// <inheritdoc cref="IBordersExtraction" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// An implementation of <see cref="IBordersExtraction"/> which first calculate the prefix function of the text, and
/// then uses it to calculate the borders efficiently.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The algorithm takes advantage of the following property of borders: any non-longest border of the text is a 
///       also a border of the longest border of the text.
///       <br/>
///     - That is, if T[0..w] is a border of T of length w, and T has longest border T[0..lb] of length lb, where 
///       0 &lt; w &lt; lb, then T[0..w] is also border of T[0..lb]. That is: any non-longest border of T is also 
///       border of the longest border of T.
///       <br/>
///     - Given the Prefix Function s of T, the length of the longest border of T can be calculated as s(n - 1), where
///       n is the length of T. The longest border of the longest border can be calculated as s(s(n - 1) - 1). And so
///       on...
///       <br/>
///     - Therefore, borders are found iteratively: the next border (shorter than the longest) can be calculated as the 
///       longest border of the border found at the previous iteration, by using the Prefix Function on the predecessor
///       of the previous result: w(i) = s(w(i - 1) - 1); w(i + 1) = s(w(i) - 1) = s(s(w(i - 1) - 1) - 1), etc.
///       <br/>
///     - The iteration terminates when the Prefix Function returns 0, i.e. there is no border.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Values of the Prefix Function have to be iterated up to the last, since the longest border of the provided
///       text T has a length equal to the value of the Prefix Function in n - 1, where n is the length of T.
///       <br/>
///     - Then there are as many iterations as the number of borders of T. In the worst case, T can have a number of 
///       borders comparable to the chars in it. For example, the string <c>new string('a', n)</c> has n - 1 borders:
///       <c>new string('a', n - 1)</c>, <c>new string('a', n - 2)</c>, ..., "a".
///       <br/>
///     - For each iteration, the border is returned as new string, and that requires iterating over each char of the
///       border. In the worst case, borders of T can have a length comparable to the length of T. For example the
///       string <c>new string('a', n)</c> has a border of length <c>n - 1</c>.
///       <br/>
///     - Direct accessing of the list of values of the Prefix Function is done in constant time.
///       <br/>
///     - Storing the Prefix Function values requires storing n integers, each of constant size.
///       <br/>
///     - Every iteration the string of a single border is built and returned.
///       <br/>
///     - Therefore, Time Complexity is O(Tpf * n + n ^ 2) and Space Complexity is O(Spf * n + n), where Tpf and Spf
///       are the amortized cost of the Prefix Function values over the n chars of T.
///     </para>
/// </remarks>
public class PrefixFunctionBasedBorderExtraction : IBordersExtraction
{
    /// <summary>
    /// The <see cref="IPrefixFunctionCalculator"/> implementation to be used, to calculate the Prefix Function of
    /// the text.
    /// </summary>
    protected IPrefixFunctionCalculator PrefixFunctionCalculator { get; }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="PrefixFunctionBasedBorderExtraction" path="/remarks"/>
    /// </remarks>
    public PrefixFunctionBasedBorderExtraction(IPrefixFunctionCalculator prefixFunctionCalculator)
    {
        PrefixFunctionCalculator = prefixFunctionCalculator;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="PrefixFunctionBasedBorderExtraction" path="/remarks"/>
    /// </remarks>
    public IEnumerable<string> GetAllBordersByDescLength(string text)
    {
        if (text == string.Empty)
            yield break;

        var prefixFunctionValues = PrefixFunctionCalculator.GetValues(text).ToList();

        var i = prefixFunctionValues[text.Length - 1];
        while (i > 0)
        {
            yield return text[0..i];
            i = prefixFunctionValues[i - 1];
        }
    }
}