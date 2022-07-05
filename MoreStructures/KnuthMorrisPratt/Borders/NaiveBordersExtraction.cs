namespace MoreStructures.Tests.KnuthMorrisPratt.Borders;

/// <inheritdoc cref="IBordersExtraction" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// An implementation of <see cref="IBordersExtraction"/> which checks every prefix of the text.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The algorithm checks all the prefixes of the text, which are not the text itself, by decreasing length.
///       <br/>
///     - For each prefix, it checks whether the prefix is also a suffix of the text.
///       <br/>
///     - If so, returns it.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - A text T of length n has n - 1 prefixes which are strictly shorter than T.
///       <br/>
///     - Each of the prefix has a length O(n).
///       <br/>
///     - Checking whether a prefix of T of length w is also a suffix of T requires comparing all w chars.
///       <br/>
///     - Therefore, Time Complexity is O(n^2) and Space Complexity is O(1).
///     </para>
/// </remarks>
public class NaiveBordersExtraction : IBordersExtraction
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaiveBordersExtraction" path="/remarks"/>
    /// </remarks>
    public IEnumerable<string> GetAllBordersByDescLength(string text)
    {
        var n = text.Length;

        if (n == 0)
            return Enumerable.Empty<string>();

        return
            from i in Enumerable.Range(1, n - 1)
            let prefix = text[0..(n - i)]
            where prefix == text[i..]
            select prefix;
    }
}