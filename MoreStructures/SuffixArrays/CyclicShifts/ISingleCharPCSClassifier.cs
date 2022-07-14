namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An algorithm definining equivalence classes for partial cyclic shifts (PCS) of length 1 (i.e. single chars) of a 
/// string.
/// </summary>
/// <remarks>
///     <para id="definition">
///     The equivalence class of a n-char partial cyclic shift c of the string T is the number of <b>distinct</b> n-char
///     partial cyclic shift of T which strictly precedes c in lexicographic order (i.e. all distinct chars strictly 
///     smaller than c).
///     <br/>
///     That means that two partial cyclic shifts which are the same share the same equivalence class.
///     <br/>
///     The minimum value the equivalence class can have is 0, for the smallest partial cyclic shift, which is preceded by
///     no other partial cyclic shift. The maximum value is the number of distinct chars in the string.
///     </para>
/// </remarks>
public interface ISingleCharPCSClassifier
{
    /// <summary>
    /// The input text, whose 1-char PCS have to be classified.
    /// </summary>
    string Input { get; }

    /// <summary>
    /// Runs the algorithm, calculating the equivalence classes of each 1-char PCS of the <see cref="Input"/>.
    /// </summary>
    /// <returns>
    /// A list of equivalence classes, with as many items as chars in the <see cref="Input"/>.
    /// </returns>
    IList<int> Classify();
}
