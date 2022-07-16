namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An implementation of <see cref="IDoubleLengthPcsSorter"/> using Counting Sort, to perform sorting of partial cyclic
/// shifts (PCS) of length 2 * L of a string, given the order and the equivalence classes of the PCS of length L.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The algorithm takes advantage of the fact that PCS of length L have been already ordered, and their order
///       from the smallest to the biggest, is defined in the provided position list O, of the PCS of the input string
///       of length L.
///       <br/>
///     - The PCS of length 2 * L which the algorithm sorts are the ones starting at index i - L, for each i in O. 
///       Let's call O' this extension of O to the the L chars preceding each index of O.
///       <br/>
///     - PCS of length 2 * L starting at index i - L are made of two halves: the first, from index i - L to index i 
///       excluded, which is unsorted; and the second, from index i to i + L excluded, which is already sorted and
///       whose order is the one defined in O.
///       <br/>
///     - The algorithm uses a stable implementation of the Counting Sort to sort in linear time O', under Counting 
///       Sort assumption of a small alphabet.
///       <br/>
///     - Moreover, the algorithm only looks at the first half of each PCS of length 2 * L, because the second half is 
///       already sorted in O, hence in O', and the implementation of the sorting is stable, i.e. it preserves order of 
///       suffixes, as they appear in the input being sorted, to the output, among the strings sharing the same prefix.
///       <br/>
///     - Counting Sort is adapted in the following way: instead of directly building an histogram of occurrences of 
///       the first halves (PCS of length L), it builds an histogram of their equivalence classes, which are integer 
///       simple to use as a key in Counting Sort and limited in number by the number of chars in the input string.
///       <br/>
///     - The order list of the PCS of length 2 * L is calculated by iterating in reverse order over the equivalence
///       classes of the second halves, which are sorted in lexicographic ascending order in the provided position 
///       list. 
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The algorithm is just a variation of the Counting Sort, where, instead of sorting by PCS (which would require
///       comparing O(n) strings, or having a much larger alphabet), a sorting by equivalence classes is done.
///       <br/>
///     - Direct access to the position list, as well as to the list of equivalence class is done in constant time.
///       <br/>
///     - Therefore, Time and Space Complexity are both O(n + sigma), where n is the length of the input string, and
///       also of the position and equivalence classes lists and sigma is the length of the alphabet, i.e. the number
///       of distinct equivalence classes of PCS of length L.
///       <br/>
///     - Because there are n PCS of length L in the input string, there are at most n distinct equivalent classes.
///       <br/>
///     - Therefore, Time and Space Complexity are actually O(n).
///     </para>
/// </remarks>
public class CountingSortDoubleLengthPcsSorter : IDoubleLengthPcsSorter
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="CountingSortDoubleLengthPcsSorter" path="/remarks"/>
    /// </remarks>
    public IList<int> Sort(string input, int pcsLength, IList<int> order, IList<int> eqClasses)
    {
        var n = input.Length;
        var sigma = eqClasses[order[^1]] + 1;

        // Build histogram
        var counts = new int[sigma];

        for (var i = 0; i < n; i++)
            counts[eqClasses[i]]++;

        // Make histogram cumulative
        for (var i = 1; i < sigma; i++)
            counts[i] += counts[i - 1];

        // Calculate order list of double PCS
        var doublePcsOrder = new int[n];
        for (var i = n - 1; i >= 0; i--)
        {
            var inputIndex = (n + order[i] - pcsLength) % n;
            var alphabetIndex = eqClasses[inputIndex];
            var position = --counts[alphabetIndex];
            doublePcsOrder[position] = inputIndex;
        }

        return doublePcsOrder;
    }
}