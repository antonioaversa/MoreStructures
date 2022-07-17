using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An implementation of <see cref="IDoubleLengthPcsClassifier"/> which solely depends on the input string, to generate
/// equivalence classes.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - PCS of length L are generated as actual strings from the input string, then sorted in lexicographic order via
///       the <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/> method.
///       <br/>
///     - The ordered sequence of PCS with corresponding starting index in the input string is then iterated over.
///       <br/>
///     - A new equivalence class is generated (increasing the current counter) every time a PCS differs from the 
///       previous one. The equivalence class is assigned to the output at the index of the PCS.
///       <br/>
///     - Finally, the equivalence class list is returned.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Extracting the n PCS of length L from the input string has O(n * L) Time and Space Complexity.
///       <br/>
///     - Sorting the n PCS of length L via the LINQ-provided QuickSort has O(n^2 * L) Time Complexity and O(n) Space
///       Complexity.
///       <br/>
///     - Iterating over the n PCS of length L is an O(n) operation. Each iteration does O(L) work, since, while
///       direct access to the equivalence class list is done in constant time, comparison between the current PCS
///       and the previous one is a comparison of two strings of length L, and requires all L chars to be comparerd in
///       the worst case.
///       <br/>
///     - Instantiating the equivalence class list of output is also an O(n) operation.
///       <br/>
///     - Therefore, Time Complexity, as driven by sorting, is O(n^2 * L) and Space Complexity, as driven by the PCS
///       generating and iteration, is O(n * L).
///     </para>
/// </remarks>
public class NaiveDoubleLengthPcsClassifier : IDoubleLengthPcsClassifier
{
    /// <summary>
    /// The input text, whose PCS of length <see cref="PcsLength"/> have to be classified.
    /// </summary>
    public string Input { get; }

    /// <summary>
    /// The length of the PCS of <see cref="Input"/> to be classified.
    /// </summary>
    public int PcsLength { get; }

    /// <summary>
    ///     <inheritdoc cref="NaiveDoubleLengthPcsClassifier"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="Input" path="/summary"/></param>
    /// <param name="pcsLength"><inheritdoc cref="PcsLength" path="/summary"/></param>
    public NaiveDoubleLengthPcsClassifier(string input, int pcsLength)
    {
        if (pcsLength <= 0 || pcsLength > input.Length)
            throw new ArgumentOutOfRangeException(
                nameof(pcsLength), $"Must be positive and at most the length of {nameof(input)}.");

        Input = input;
        PcsLength = pcsLength;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaiveDoubleLengthPcsClassifier" path="/remarks"/>
    /// </remarks>
    public IList<int> Classify()
    {
        var (headList, tail) = PcsUtils
            .ExtractPcsOf(Input, PcsLength)
            .OrderBy(s => s)
            .EnumerateExactlyFirst(1);

        var (firstPcs, firstIndex) = headList.Single();
        var eqClasses = new int[Input.Length];

        var currentEqClass = 0;
        eqClasses[firstIndex] = currentEqClass;
        
        var previousPcs = firstPcs;
        foreach (var (pcs, index) in tail)
        {
            eqClasses[index] = pcs == previousPcs ? currentEqClass : ++currentEqClass;
            previousPcs = pcs;
        }

        return eqClasses;
    }
}
