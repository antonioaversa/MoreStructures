using MoreStructures.Strings.Sorting;
using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building the <see cref="SuffixArray"/> based on fast PCS comparison.
/// </summary>
/// <remarks>
///     <para id="assumptions">
///     <b>Remark: the following analysis is based on the default implementations used for each of the four steps of
///     the Suffix Array building algorithm, as set by 
///     <see cref="PcsBasedSuffixArrayBuilder(TextWithTerminator, IDictionary{char, int})"/>. Using a builder different
///     from the default for any of the step would result into a different runtime and complexity.</b>
///     </para>
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
///       <see cref="Text"/> and sigma is the size of the alphabet of <see cref="Text"/>.
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
    /// The builder of the <see cref="ICharsSorter"/> implementation used to sort 1-char PCS in the Suffix Array 
    /// building algorithm.
    /// </summary>
    /// <remarks>
    /// Takes as input a single <see cref="string"/> parameter, containing the input text, whose chars have to be 
    /// sorted.
    /// <br/>
    /// Returns a suitable <see cref="ICharsSorter"/> implementation.
    /// </remarks>
    public Func<string, ICharsSorter> SingleCharPcsSorterBuilder { get; }

    /// <summary>
    /// The builder of the <see cref="ISingleCharPcsClassifier"/> implementation used to find equivalence classes of
    /// 1-char PCS in the Suffix Array building algorithm.
    /// </summary>
    /// <remarks>
    /// Takes as first input parameter a <see cref="string"/>, containing the input text, whose chars have to be 
    /// classified.
    /// <br/>
    /// Takes as second input parameter a <see cref="IList{T}"/> of <see cref="int"/>, containing the order of the 
    /// 1-char PCS, previously calculated.
    /// <br/>
    /// Returns a suitable <see cref="ISingleCharPcsClassifier"/> implementation.
    /// </remarks>
    public Func<string, IList<int>, ISingleCharPcsClassifier> SingleCharPcsClassifierBuilder { get; }

    /// <summary>
    /// The builder of the <see cref="IDoubleLengthPcsSorter"/> implementation used in the Suffix Array building 
    /// algorithm, to sort PCS of length 2 * L, once PCS of length L have been sorted and classified.
    /// </summary>
    /// <remarks>
    /// Takes as first input parameter a <see cref="int"/>, containing the length L of the PCS.
    /// <br/>
    /// Takes as second input parameter a <see cref="IList{T}"/> of <see cref="int"/>, containing the order of the 
    /// PCS of length L, previously calculated and potentially needed to calculate the order of PCS of length 2 * L.
    /// <br/>
    /// Takes as third input parameter a <see cref="IList{T}"/> of <see cref="int"/>, containing the equivalence 
    /// classes of the PCS of length L, previously calculated and potentially needed to calculate the order of PCS of 
    /// length 2 * L.
    /// <br/>
    /// Returns a suitable <see cref="IDoubleLengthPcsSorter"/> implementation.
    /// </remarks>
    public Func<int, IList<int>, IList<int>, IDoubleLengthPcsSorter> DoubleLengthPcsSorterBuilder { get; }

    /// <summary>
    /// The builder of the <see cref="IDoubleLengthPcsClassifier"/> implementation used in the Suffix Array building 
    /// algorithm, to classify PCS of length 2 * L, once PCS of length L have been sorted and classified and PCS of 
    /// length 2 * L have been sorted.
    /// </summary>
    /// <remarks>
    /// Takes as first input parameter a <see cref="int"/>, containing the length L of the PCS.
    /// <br/>
    /// Takes as second input parameter a <see cref="IList{T}"/> of <see cref="int"/>, containing the equivalence 
    /// classes of the PCS of length L, previously calculated and potentially needed to calculate the equivalence 
    /// classes of PCS of length 2 * L.
    /// <br/>
    /// Takes as third input parameter a <see cref="IList{T}"/> of <see cref="int"/>, containing the order of the 
    /// PCS of length L, previously calculated and potentially needed to calculate the equivalence classes of PCS of 
    /// length 2 * L.
    /// <br/>
    /// Returns a suitable <see cref="IDoubleLengthPcsClassifier"/> implementation.
    /// </remarks>
    public Func<int, IList<int>, IList<int>, IDoubleLengthPcsClassifier> DoubleLengthPcsClassifierBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="NaiveSuffixArrayBuilder" path="/summary"/>
    /// </summary>
    /// <remarks>
    ///     Allows to specify the algorithm to be used for each of the four steps of the Suffix Array building 
    ///     algorithm, each one via a dedicated builder.
    /// </remarks>
    /// <param name="text">
    ///     <inheritdoc cref="Text" path="/summary"/>
    /// </param>
    /// <param name="singleCharPcsSorterBuilder">
    ///     <inheritdoc cref="SingleCharPcsSorterBuilder" path="/summary"/>
    /// </param>
    /// <param name="singleCharPcsClassifierBuilder">
    ///     <inheritdoc cref="SingleCharPcsClassifierBuilder" path="/summary"/>
    /// </param>
    /// <param name="doubleLengthPcsSorterBuilder">
    ///     <inheritdoc cref="DoubleLengthPcsSorterBuilder" path="/summary"/>
    /// </param>
    /// <param name="doubleLengthPcsClassifierBuilder">
    ///     <inheritdoc cref="DoubleLengthPcsClassifierBuilder" path="/summary"/>
    /// </param>
    public PcsBasedSuffixArrayBuilder(
        TextWithTerminator text, 
        Func<string, ICharsSorter> singleCharPcsSorterBuilder, 
        Func<string, IList<int>, ISingleCharPcsClassifier> singleCharPcsClassifierBuilder,
        Func<int, IList<int>, IList<int>, IDoubleLengthPcsSorter> doubleLengthPcsSorterBuilder,
        Func<int, IList<int>, IList<int>, IDoubleLengthPcsClassifier> doubleLengthPcsClassifierBuilder)
    {
        Text = text;
        SingleCharPcsSorterBuilder = singleCharPcsSorterBuilder;
        SingleCharPcsClassifierBuilder = singleCharPcsClassifierBuilder;
        DoubleLengthPcsSorterBuilder = doubleLengthPcsSorterBuilder;
        DoubleLengthPcsClassifierBuilder = doubleLengthPcsClassifierBuilder;
    }

    /// <summary>
    ///     <inheritdoc cref="NaiveSuffixArrayBuilder" path="/summary"/>
    /// </summary>
    /// <remarks>
    ///     Uses the best implementations for each of the four steps of the Suffix Array building algorithm, resulting
    ///     in an overall linear Time and Space Complexity.
    /// </remarks>
    /// <param name="text">
    ///     <inheritdoc cref="Text" path="/summary"/>
    /// </param>
    /// <param name="alphabet">
    /// The alphabet of <see cref="Text"/>, i.e. the list of all <see cref="char"/> potentially appearing in 
    /// <see cref="Text"/>, mapped to an alphabet index. Required by the Counting Sort algorithm, which builds the 
    /// histogram of occurrences in the input, of all chars of the alphabet of the input.
    /// </param>
    public PcsBasedSuffixArrayBuilder(TextWithTerminator text, IDictionary<char, int> alphabet)
    {
        Text = text;
        SingleCharPcsSorterBuilder = 
            input => 
                new CountingSortCharsSorter(alphabet);
        SingleCharPcsClassifierBuilder = 
            (input, order) => 
                new OrderBasedSingleCharPcsClassifier(input, order);
        DoubleLengthPcsSorterBuilder = 
            (pcsLength, order, eqClasses) => 
                new CountingSortDoubleLengthPcsSorter(pcsLength, order, eqClasses);
        DoubleLengthPcsClassifierBuilder =
            (pcsLength, eqClassesPcsHalfLength, order) => 
                new EqClassesBasedDoubleLengthPcsClassifier(pcsLength, eqClassesPcsHalfLength, order);
    }

    /// <summary>
    /// <inheritdoc cref="PcsBasedSuffixArrayBuilder" path="/summary"/>
    /// </summary>
    public SuffixArray Build()
    {
        var input = string.Concat(Text);

        var singleCharPcsSorter = SingleCharPcsSorterBuilder(input);        
        var order = singleCharPcsSorter.Sort(input);

        var singleCharEqClassClassifier = SingleCharPcsClassifierBuilder(input, order);
        var eqClasses = singleCharEqClassClassifier.Classify();

        var pcsLength = 1;
        while (pcsLength < input.Length)
        {
            pcsLength *= 2;

            var doubleLengthPcsSorter = DoubleLengthPcsSorterBuilder(pcsLength / 2, order, eqClasses);
            order = doubleLengthPcsSorter.Sort();
            var doubleLengthPcsClassifier = DoubleLengthPcsClassifierBuilder(pcsLength, eqClasses, order);
            eqClasses = doubleLengthPcsClassifier.Classify();
        }

        return new SuffixArray(order);
    }
}