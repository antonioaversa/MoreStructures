namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// Static class of utilities for partial cyclic shifts (PCS) of input strings.
/// </summary>
public static class PcsUtils
{
    /// <summary>
    /// Extracts the PCS of length <paramref name="pcsLength"/> from the provided <paramref name="input"/> string, 
    /// together with their starting index in <paramref name="input"/>.
    /// </summary>
    /// <param name="input">The input string, to extract the PCS of length <paramref name="pcsLength"/> of.</param>
    /// <param name="pcsLength">The length of PCS to extract.</param>
    /// <returns>
    /// A sequence of strings, each one being a PCS. As many as the number of chars in the <paramref name="input"/>.
    /// </returns>
    /// <remarks>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     Time and Space Complexity is O(n * L).
    ///     </para>
    /// </remarks>
    public static IEnumerable<(string pcs, int index)> ExtractPcsOf(string input, int pcsLength) => 
        from i in Enumerable.Range(0, input.Length)
        let j = i + pcsLength
        let cyclicShift = j <= input.Length ? input[i..j] : input[i..] + input[0..((j - input.Length) % input.Length)]
        select (cyclicShift, i);
}
