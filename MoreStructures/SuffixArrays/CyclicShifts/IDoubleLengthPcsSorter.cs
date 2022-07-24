using MoreStructures.Strings.Sorting;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An algorithm sorting in lexicographic order all the partial cyclic shifts (PCS) of length 2 * L of a string.
/// </summary>
public interface IDoubleLengthPcsSorter
{
    /// <summary>
    /// The length L of the PCS. Remark: sorting is done of PCS of length 2 * L, not L.
    /// </summary>
    public int PcsLength { get; }

    /// <summary>
    /// Sorts in lexicographic order all the partial cyclic shifts (PCS) of the input string of length 2 * L.
    /// </summary>
    /// <returns>The position list of PCS of length 2 * L.</returns>
    /// <remarks>
    /// The bootstrap of this process requires sorting PCS of length 1, i.e. single chars of the input string. 
    /// This can be done by any implementation of <see cref="ICharsSorter"/>, such as
    /// <see cref="CountingSortCharsSorter"/>, which sorts single chars of the input in linear time, in scenarios of 
    /// small alphabets.
    /// </remarks>
    IList<int> Sort();
}
