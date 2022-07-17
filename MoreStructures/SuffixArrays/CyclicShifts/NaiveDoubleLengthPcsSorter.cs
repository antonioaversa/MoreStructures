namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An implementation of <see cref="IDoubleLengthPcsSorter"/> sorting of partial cyclic shifts (PCS) of length 2 * L 
/// of a string by generating them and sorting with the LINQ-provided QuickSort. Ignores the provided order and 
/// equivalence classes of the PCS of length L.
/// </summary>
/// <remarks>
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
///     </para>
/// </remarks>
public class NaiveDoubleLengthPcsSorter : IDoubleLengthPcsSorter
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaiveDoubleLengthPcsSorter" path="/remarks"/>
    /// </remarks>
    public IList<int> Sort(string input, int pcsLength, IList<int> order, IList<int> eqClasses) => (
        from pcsAndIndex in PcsUtils.ExtractPcsOf(input, pcsLength * 2)
        orderby pcsAndIndex.pcs ascending
        select pcsAndIndex.index)
        .ToList();
}