using MoreStructures.Strings.Sorting;
using MoreStructures.SuffixArrays.CyclicShifts;
using MoreStructures.SuffixStructures;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building the <see cref="SuffixArray"/> based on fast PCS comparison.
/// TODO
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     The algorithm is based on two lists, position list and equivalence class list, which are iteratively 
///     recalculated, over and over again, for longer and longer PCS of <see cref="Text"/>, until the full content of
///     <see cref="Text"/> is covered by the PCS.
///     <br/>
///     More in detail:
///     <br/>
///     - Position and equivalence class lists are first calculated for 1-char PCS, i.e. for single chars, using
///       <see cref="CountingSortCharsSorter"/> and <see cref="OrderBasedSingleCharPcsClassifier"/> respectively.
///       <br/>
///     - Position and equivalence class lists are then calculated for PCS of length double than the current one, using
///       <see cref="CountingSortDoubleLengthPcsSorter"/> and <see cref="EqClassesBasedDoubleLengthPcsClassifier"/>
///       respectively.
///       <br/>
///     - The operation is repeated until the current PCS length becomes bigger or equal than the length of 
///       <see cref="Text"/>.
///       <br/>
///     - At this point, the last calculated order is the order of PCS which are at least as long as the full
///       <see cref="Text"/>.
///       <br/>
///     - Because all such strings include a full suffixes of <see cref="Text"/> terminated by a terminator, which is a
///       unique char, the resulting order is the order of all suffixes of <see cref="Text"/>, which is by definition
///       the Suffix Array of <see cref="Text"/>.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The algorithm has a bootstrap part (1-char PCS) and an iterative part (doubling length PCS).
///       <br/>
///     - It first calculates position and equivalence lists for 1-char PCS, which are respectively 
///       O(n + sigma) and O(n) operations, both in Time and Space Complexity, where n is the length of 
///       <see cref="Text"/> and sigma is the size of the <see cref="Alphabet"/> of <see cref="Text"/>.
///       <br/>
///     - Then the iterative part is executed. The number of top-level iterations is logarithmic with n, because the
///       PCS length doubles at every iteration and the termination condition is that the PCS length is at least as big
///       as the length of <see cref="Text"/>.
///       <br/>
///     - The two operations done in the iteration loop both have linear Time and Space Complexity.
///       <br/>
///     - Therefore, both Time and Space Complexity are O(n * log(n) + sigma).
///       <br/>
///     - If sigma is O(n), Time and Space Complexity are O(n * log(n)).
///     </para>
/// </remarks>
public class PcsBasedSuffixArrayBuilder : ISuffixArrayBuilder
{
    /// <summary>
    /// The <see cref="TextWithTerminator"/>, to build the <see cref="SuffixArray"/> of.
    /// </summary>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// The alphabet of <see cref="Text"/>, i.e. the list of all <see cref="char"/> potentially appearing in 
    /// <see cref="Text"/>, mapped to an alphabet index.
    /// </summary>
    /// <remarks>
    /// Required by the Counting Sort algorithm, which builds the histogram of occurrences in the input, of all chars
    /// of the alphabet of the input.
    /// </remarks>
    public IDictionary<char, int> Alphabet { get; }

    /// <summary>
    /// <inheritdoc cref="NaiveSuffixArrayBuilder" path="/summary"/>
    /// </summary>
    /// <param name="text"><inheritdoc cref="Text" path="/summary"/></param>
    /// <param name="alphabet"><inheritdoc cref="Text" path="/summary"/></param>
    public PcsBasedSuffixArrayBuilder(TextWithTerminator text, IDictionary<char, int> alphabet)
    {
        Text = text;
        Alphabet = alphabet;
    }

    /// <summary>
    /// <inheritdoc cref="PcsBasedSuffixArrayBuilder" path="/summary"/>
    /// </summary>
    public SuffixArray Build()
    {
        var input = string.Concat(Text);

        var singleCharPcsSorter = new CountingSortCharsSorter(Alphabet);        
        var order = singleCharPcsSorter.Sort(input);

        var singleCharEqClassClassifier = new OrderBasedSingleCharPcsClassifier(input, order);
        var eqClasses = singleCharEqClassClassifier.Classify();

        var pcsLength = 1;
        while (pcsLength < input.Length)
        {
            pcsLength *= 2;

            var doubleLengthPcsSorter = new CountingSortDoubleLengthPcsSorter();
            order = doubleLengthPcsSorter.Sort(input, pcsLength / 2, order, eqClasses);
            var doubleLengthPcsClassifier = new EqClassesBasedDoubleLengthPcsClassifier(pcsLength, eqClasses, order);
            eqClasses = doubleLengthPcsClassifier.Classify();
        }

        return new SuffixArray(order);
    }
}