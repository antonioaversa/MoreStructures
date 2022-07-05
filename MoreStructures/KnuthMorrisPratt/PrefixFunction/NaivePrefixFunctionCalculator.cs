using MoreStructures.KnuthMorrisPratt.Borders;

namespace MoreStructures.KnuthMorrisPratt.PrefixFunction;

/// <inheritdoc cref="IPrefixFunctionCalculator" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// An implementation of <see cref="IPrefixFunctionCalculator"/> which retrieves the longest border, then checks its 
/// length.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The algorithm iterates over all the chars of the text, from the first to the last.
///       <br/>
///     - For each char at index i, it retrieves the longest border of the prefix, up to char at index i included, by 
///       using the <see cref="IBordersExtraction"/> implementation provided at construction time.
///       <br/>
///     - <see cref="IBordersExtraction.GetAllBordersByDescLength(string)"/> retrieves the borders from the longest to
///       the shortest, whatever the implementation. Therefore, only the first border needs to be enumerated.
///       <br/>
///     - If no borders are found, the Prefix Function at index i is zero. Otherwise, it's the length of the longest.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - There are as many prefixes of the text as chars.
///       <br/>
///     - The time and space cost of retrieving the longest border depends on the actual implementation of 
///       <see cref="IBordersExtraction"/> used, to retrieve the longest border, which, when it exists, it is the first
///       in the sequence.
///       <br/>
///     - Checking whether a border has been found and, in case, its length are both constant time operations.
///       <br/>
///     - Therefore, Time Complexity is O(n * Tlb) and Space Complexity is O(n * Slb), where Tlb and Slb are the Time 
///       and Space Complexity of finding the longest border (1st item) via the provided 
///       <see cref="BordersExtraction"/>.
///     </para>
/// </remarks>
public class NaivePrefixFunctionCalculator : IPrefixFunctionCalculator
{
    /// <summary>
    /// The <see cref="IBordersExtraction"/> implementation to be used, to retrieve the longest border of the text.
    /// </summary>
    protected IBordersExtraction BordersExtraction { get; }

    /// <summary>
    ///     <inheritdoc cref="NaivePrefixFunctionCalculator"/>
    /// </summary>
    /// <param name="bordersExtraction"><inheritdoc cref="BordersExtraction" path="/summary"/></param>
    public NaivePrefixFunctionCalculator(IBordersExtraction bordersExtraction)
    {
        BordersExtraction = bordersExtraction;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaivePrefixFunctionCalculator" path="/remarks"/>
    /// </remarks>
    public IEnumerable<int> GetValues(string text) => 
        from i in Enumerable.Range(0, text.Length)
        let prefix = text[0..(i + 1)]
        let longestBorderMaybe = BordersExtraction.GetAllBordersByDescLength(prefix).FirstOrDefault()
        select longestBorderMaybe != null ? longestBorderMaybe.Length : 0;
}
