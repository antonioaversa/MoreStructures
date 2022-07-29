using MoreLinq;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.LongestCommonPrefix;

/// <summary>
/// An <see cref="ILcpArrayBuilder"/> implementation which uses the Kasai's algorithm (2001) to compute the LCP Array
/// in linear time.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to the naive implementation following the definition of LCP Array, implemented by 
///       <see cref="NaiveLcpArrayBuilder"/>, this algorithm has better runtime and equal Space Complexity.
///       <br/>
///     - The better runtime comes at the cost of the complexity of the algorithm itself, which is harder to analyze.
///       <br/>
///     - Moreover, unlike in the naive implementation, the construction of the array doesn't proceed in order, i.e.
///       it doesn't proceed from the first element to the last element of the array.
///       <br/>
///     - That is, the Kasai's algorithm is not online and, in its base form, cannot be easily made lazy.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm requires 3 data structures to operate: the text T, its Suffix Array SA and the Inverted Suffix
///       Array ISA. The three data structures have n items (chars or integers).
///       <br/>
///     - T and SA are externally provided, whereas ISA is calculated scanning linearly SA and populating a dictionary
///       of positions of SA: <c>ISA[v] = i such that SA[i] = v</c>. Because there is no better general algorithm than
///       a single linear scan of the input, ISA is internally calculated, rather than externally provided.
///       <br/>
///     - The current LCP length, named here CLCP, is initialized to 0, and the index of the current suffix in T, named
///       here CS, is initialized to the first item of SA: <c>CLCP = 0; CS = SA[0]</c>. This is the setup before the
///       main loop.
///       <br/>
///     - Then n iterations are performed, filling in the resulting LPC array, named here LCPA, initialized to an empty
///       array of length n - 1.
///       <br/>
///     - At each iteration the index in SA of the current suffix (starting in T at index CS), is calculated via ISA:
///       <c>k = ISA[CS]</c>.
///       <br/>
///     - Then, the next suffix in T, named here NS, is calculated as the successor of k in SA: <c>NS = SA[k + 1]</c>.
///       <br/>
///     - Important: if <c>ISA[CS]</c> is the last index of SA (i.e. n - 1), then CLCP is reset and the current 
///       iteration skipped.
///       <br/>
///     - A new value of CLCP is calculated as the longest common prefix between T[CS..] and T[NS..], skipping the 
///       first CLCP - 1 chars, which are equal by construction.
///       <br/>
///     - Such new value of CLCP is then assigned to LCPA at index 
///       <br/>
///     - Finally, the index of the current suffix in T, CS, is incremented modulo n and the current iteration 
///       terminates.
///       <br/>
///     - After the loop, LCPA is fully populated and can be returned as result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Text and Suffix Array evaluation (required to have direct access by index) are O(n) operations, both in time
///       and space.
///       <br/>
///     - Inversion of the Suffix Array is also a O(n) operation.
///       <br/>
///     - Initialization of the current LCP and the current prefix are constant-time operations.
///       <br/>
///     - The main loop of the algorithm runs n iterations.
///       <br/>
///     - The only operation which doesn't take constant time in the body of the loop is the LCP between the current
///       suffix, initialized at the first item of the Suffix Array and then incremented by 1 modulo n at every
///       iteration, and the next suffix, calculated via Suffix Array and its inverted version.
///       <br/>
///     - While the above is true, it should be noted that the current LCP is increased once per match and decreased 
///       once per iteration. Because the current LCP cannot be bigger than n (that's the biggest number of chars a
///       prefix can have, hence the bigger number of chars in common between two prefixes), the total number of 
///       comparisons across all iterations is O(n).
///       <br/>
///     - Therefore, Time and Space Complexity are O(n).
///     </para>
/// </remarks>
public class KasaiLcpArrayBuilder : NaiveLcpArrayBuilder
{
    /// <summary>
    ///     <inheritdoc cref="KasaiLcpArrayBuilder"/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="KasaiLcpArrayBuilder"/>
    /// </remarks>
    /// <param name="text">
    ///     <inheritdoc cref="NaiveLcpArrayBuilder.Text" path="/summary"/>
    /// </param>
    /// <param name="suffixArray">
    ///     <inheritdoc cref="NaiveLcpArrayBuilder.SuffixArray" path="/summary"/>
    /// </param>
    public KasaiLcpArrayBuilder(TextWithTerminator text, SuffixArray suffixArray) : base(text, suffixArray)
    {
    }

    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="KasaiLcpArrayBuilder"/>
    /// </remarks>
    public override LcpArray Build()
    {
        var text = string.Concat(Text);
        var lcpArray = new int[Text.Length - 1];
        var suffixArray = SuffixArray.Indexes.ToList();
        var invertedSuffixArray = suffixArray.Index().ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        var currentLcp = 0;
        var currentSuffix = suffixArray[0];
        for (var i = 0; i < text.Length; i++)
        {
            var indexOfCurrentSuffixInSuffixArray = invertedSuffixArray[currentSuffix];
            if (indexOfCurrentSuffixInSuffixArray == text.Length - 1)
            {
                currentLcp = 0;
            }
            else
            {
                var nextSuffix = suffixArray[indexOfCurrentSuffixInSuffixArray + 1];
                var delta = Math.Max(currentLcp - 1, 0);
                currentLcp = StringUtilities.LongestCommonPrefix(
                    text, currentSuffix + delta, text, nextSuffix + delta) + delta;

                lcpArray[indexOfCurrentSuffixInSuffixArray] = currentLcp;
            }

            currentSuffix = (currentSuffix + 1) % text.Length;
        }

        return new LcpArray(lcpArray);
    }
}