namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An algorithm defining equivalence classes for partial cyclic shifts (PCS) of length 2 * L in the input string.
/// </summary>
/// <remarks>
/// The method <see cref="Classify"/> has no input parameters because the input of the algorithm depends on the 
/// specific implementation.
/// </remarks>
public interface IDoubleLengthPcsClassifier
{
    /// <summary>
    /// Runs the algorithm, calculating the equivalence classes of all PCS of length L in the input string.
    /// </summary>
    /// <returns>
    /// A list of equivalence classes, with as many items as PCS of length L in the input string.
    /// </returns>
    IList<int> Classify();
}
