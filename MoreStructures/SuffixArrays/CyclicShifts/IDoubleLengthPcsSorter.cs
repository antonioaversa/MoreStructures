using MoreStructures.Strings.Sorting;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An algorithm sorting in lexicographic order all the partial cyclic shifts (PCS) of length 2 * L of a string, 
/// given the order and the equivalence classes of the PCS of length L.
/// </summary>
public interface IDoubleLengthPcsSorter
{
    /// <summary>
    /// Sorts in lexicographic order all the partial cyclic shifts (PCS) of the <paramref name="input"/> of length 
    /// 2 * <paramref name="pcsLength"/>, given the position list of the PCS of the <paramref name="input"/> of length
    /// <paramref name="pcsLength"/> and their equivalence classes.
    /// </summary>
    /// <param name="input">The input string, whose PCS have to be sorted.</param>
    /// <param name="pcsLength">The length of the PCS already sorted in <paramref name="order"/>.</param>
    /// <param name="order">
    /// The position list of the already sorted PCS of length <paramref name="pcsLength"/>.
    /// </param>
    /// <param name="eqClasses">
    /// The equivalence classes of the already sorted PCS of length <paramref name="pcsLength"/>.
    /// </param>
    /// <returns>The position list of PCS of length 2 * L.</returns>
    /// <remarks>
    /// The bootstrap of this process requires sorting PCS of length 1, i.e. single chars of the 
    /// <paramref name="input"/>. This can be done by any implementation of <see cref="ICharsSorter"/>, such as
    /// <see cref="CountingSortCharsSorter"/>, which sorts single chars of the <paramref name="input"/> in linear time,
    /// in scenarios of small alphabets.
    /// </remarks>
    IList<int> Sort(string input, int pcsLength, IList<int> order, IList<int> eqClasses);
}
