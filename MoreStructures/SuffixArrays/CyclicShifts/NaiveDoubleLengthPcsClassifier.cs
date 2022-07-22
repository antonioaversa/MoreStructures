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
    private IComparer<string> PcsComparer { get; }

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
    /// <param name="inputWithTerminator">
    /// Whether <paramref name="input"/> is terminated by a terminator char. If so, the last char of 
    /// <paramref name="input"/> will be treated as a terminator char when comparing PCS of the input.
    /// Otherwise, <see cref="Comparer{T}.Default"/> for <see cref="string"/> will be used.
    /// </param>
    public NaiveDoubleLengthPcsClassifier(string input, int pcsLength, bool inputWithTerminator)
    {
        if (pcsLength <= 0 || pcsLength > input.Length)
            throw new ArgumentOutOfRangeException(
                nameof(pcsLength), $"Must be positive and at most the length of {nameof(input)}.");

        Input = input;
        PcsLength = pcsLength;
        PcsComparer = inputWithTerminator 
            ? StringIncludingTerminatorComparer.Build(input[^1])
            : Comparer<string>.Default;
    }

    private class PcsAndIndexComparer : IComparer<(string pcs, int index)>
    {
        private readonly IComparer<string> PcsComparer;
        private readonly IComparer<int> IndexComparer;

        public PcsAndIndexComparer(IComparer<string> pcsComparer)
        {
            PcsComparer = pcsComparer;
            IndexComparer = Comparer<int>.Default;
        }

        public int Compare((string pcs, int index) x, (string pcs, int index) y)
        {
            var pcsComparison = PcsComparer.Compare(x.pcs, y.pcs);
            if (pcsComparison != 0)
                return pcsComparison;
            return IndexComparer.Compare(x.index, y.index);
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaiveDoubleLengthPcsClassifier" path="/remarks"/>
    /// </remarks>
    public IList<int> Classify()
    {
        var (headList, tail) = PcsUtils
            .ExtractPcsOf(Input, PcsLength)
            .OrderBy(s => s, new PcsAndIndexComparer(PcsComparer))
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
