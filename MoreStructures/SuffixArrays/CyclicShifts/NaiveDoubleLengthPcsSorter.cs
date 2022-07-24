using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An implementation of <see cref="IDoubleLengthPcsSorter"/> sorting of partial cyclic shifts (PCS) of length 2 * L 
/// of a string by generating them and sorting with the LINQ-provided QuickSort. Ignores the provided order and 
/// equivalence classes of the PCS of length L.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Unlike other implementations, such as <see cref="CountingSortDoubleLengthPcsSorter"/>, this sorter needs to 
///       know whether the input being provided includes a terminator (i.e. its last char is "special" as it uniquely 
///       identify the end of the string) or not.
///       <br/>
///     - This information is required since this sorter performs sorting by actually comparing PCS.
///       <br/>
///     - Other implementations don't require such information, since they never compare PCS against each other: they
///       use externally provided data structures, from which they infer the order.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - For each index i of the input string, a cyclic shift starting at i and of length 2 * L is generated.
///       <br/>
///     - Cyclic shifts are sorted in ascending lexicographic order, using 
///       <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>.
///       <br/>
///     - Indexes i, corresponding to the sorted cyclic shifts, are returned as a list.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - There are always n cyclic shifts, where n is the length of the input string.
///       <br/>
///     - Each cyclic shift has length 2 * L: building it takes O(L) time and space.
///       <br/>
///     - Sorting the cyclic shifts via QuickSort takes O(n * log(n)) comparisons, between strings of length L.
///       <br/>
///     - Generating the output takes O(n * L) space.
///       <br/>
///     - Therefore, Time Complexity is O(n * log(n) * L) and Space Complexity is O(n * L) space.
///       <br/>
///     - If L is not constant, but rather O(n), Time Complexity is O(n^2 * log(n)) and Space Complexity is O(n^2). 
///     </para>
/// </remarks>
public class NaiveDoubleLengthPcsSorter : IDoubleLengthPcsSorter
{
    private IComparer<string> PcsComparer { get; }

    /// <summary>
    /// The input string, whose PCS have to be sorted.
    /// </summary>
    public string Input { get; }

    /// <inheritdoc/>
    public int PcsLength { get; }

    /// <summary>
    ///     <inheritdoc cref="NaiveDoubleLengthPcsSorter"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="Input" path="/summary"/></param>
    /// <param name="pcsLength"><inheritdoc cref="PcsLength" path="/summary"/></param>
    /// <param name="inputWithTerminator">
    /// Whether <paramref name="input"/> is terminated by a terminator char. If so, the last char of 
    /// <paramref name="input"/> will be treated as a terminator char when comparing PCS of the input.
    /// Otherwise, <see cref="Comparer{T}.Default"/> for <see cref="string"/> will be used.
    /// </param>
    public NaiveDoubleLengthPcsSorter(string input, int pcsLength, bool inputWithTerminator)
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


    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaiveDoubleLengthPcsSorter" path="/remarks"/>
    /// </remarks>
    public IList<int> Sort() =>
        PcsUtils
            .ExtractPcsOf(Input, PcsLength * 2)
            .OrderBy(pcsAndIndex => pcsAndIndex.pcs, PcsComparer)
            .Select(pcsAndIndex => pcsAndIndex.index)
            .ToList();
}