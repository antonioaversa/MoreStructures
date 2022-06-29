using MoreStructures.SuffixStructures;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building the <see cref="SuffixArray"/> directly from a <see cref="TextWithTerminator"/>, listing 
/// and sorting all suffixes of <see cref="Text"/>.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     The following steps are performed, lazily.
///     <br/>
///     - First all suffixes of the input <see cref="TextWithTerminator"/> are generated.
///       <br/>
///     - Then the suffixes are sorted in ascending order.
///       <br/>
///     - Finally, the 1st char of each suffix is taken.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - There are n suffixes, where n is the length of the input text (including the terminator).
///       <br/>
///     - Sorting n strings requires n * log(n) comparisons, each comparing at most n chars.
///       <br/>
///     - Taking the first char of each of the suffixes takes O(1) time, and there are n of them.
///       <br/>
///     - Therefore, Time Complexity is O(n^2 * log(n)) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class NaiveSuffixArrayBuilder : ISuffixArrayBuilder
{
    private readonly IComparer<string> Comparer;

    /// <summary>
    /// The <see cref="TextWithTerminator"/>, to build the <see cref="SuffixArray"/> of.
    /// </summary>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// <inheritdoc cref="NaiveSuffixArrayBuilder" path="/summary"/>
    /// </summary>
    /// <param name="text"><inheritdoc cref="Text" path="/summary"/></param>
    public NaiveSuffixArrayBuilder(TextWithTerminator text)
    {
        Text = text;
        Comparer = StringIncludingTerminatorComparer.Build(text.Terminator);
    }

    /// <summary>
    /// <inheritdoc cref="NaiveSuffixArrayBuilder" path="/summary"/>
    /// </summary>
    public SuffixArray Build() => new(Enumerable
        .Range(0, Text.Length)
        .Select(i => string.Concat(Text[i..]))
        .OrderBy(s => s, Comparer)
        .Select(s => Text.Length - s.Length));
}