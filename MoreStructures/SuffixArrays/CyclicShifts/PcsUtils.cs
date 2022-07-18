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
        from index in Enumerable.Range(0, input.Length)
        let pcs = ExtractPcsOf(input, index, pcsLength)
        select (pcs, index);

    /// <summary>
    /// Extract the PCS of length <paramref name="pcsLength"/> from the provided <paramref name="input"/> string,
    /// starting at index <paramref name="index"/>.
    /// </summary>
    /// <param name="input">The input string, to extract the PCS of length <paramref name="pcsLength"/> of.</param>
    /// <param name="index">The starting index of the PCS to extract.</param>
    /// <param name="pcsLength">The length of the PCS to extract.</param>
    /// <returns>The string containing the PCS.</returns>
    /// <remarks>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     Time and Space Complexity is O(L).
    ///     </para>
    /// </remarks>
    public static string ExtractPcsOf(string input, int index, int pcsLength) => 
        index + pcsLength <= input.Length
            ? input[index..(index + pcsLength)]
            : input[index..] + input[0..((index + pcsLength - input.Length) % input.Length)];
}
