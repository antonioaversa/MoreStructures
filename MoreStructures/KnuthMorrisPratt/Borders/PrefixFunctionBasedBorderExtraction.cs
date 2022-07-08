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