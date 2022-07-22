using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An implementation of <see cref="IDoubleLengthPcsClassifier"/> which uses the externally provided list of 
/// equivalence classes <see cref="EqClassesPcsHalfLength"/>, of the PCS of length <see cref="PcsLength"/> / 2, as well
/// as the position list of the PCS of length <see cref="PcsLength"/>, and doesn't require the input string, to 
/// generate equivalence classes of the PCS of length <see cref="PcsLength"/>.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     - Compared to <see cref="NaiveDoubleLengthPcsClassifier"/>, it has way better runtime (linear time, instead of
///       cubic).
///       <br/>
///     - Compared <see cref="OrderBasedDoubleLengthPcsClassifier"/>, it still has better runtime (linear time, instead
///       of quadratic).
///       <br/>
///     - However, it requires both the position list <see cref="Order"/> and the equivalence class list 
///       <see cref="EqClassesPcsHalfLength"/>, to be externally provided.
///       <br/>
///     - Compared to other implementations, <b>this algorithm requires <see cref="PcsLength"/> to be even.</b>
///       <br/>
///     - Unlike <see cref="NaiveDoubleLengthPcsClassifier"/>, this implementation requires specific data structures
///       to be provided, in alternative to the input, to calculate equivalence classes.
///       <br/>
///     - However, unlike <see cref="NaiveDoubleLengthPcsClassifier"/>, it does not require to know whether 
///       input contains a terminator or not.
///       <br/>
///     - This is because such piece of information would only be needed when running comparisons between PCS.
///       <br/>
///     - This classifier, on the other hand, uses an externally provided lists precisely in order to avoid the
///       need for costly PCS comparisons.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The externally provided ordered sequence of PCS <see cref="Order"/> is iterated over.
///       <br/>
///     - A new equivalence class is generated (increasing the current counter) every time a PCS differs from the 
///       previous one. The equivalence class is assigned to the output at the index of the PCS being checked.
///       <br/>
///     - The comparison between a PCS and the previous one (both of length <see cref="PcsLength"/>), is not done by 
///       comparing chars, rather by comparing equivalence classes of the PCS of length <see cref="PcsLength"/> / 2,
///       defined in the externally provided <see cref="EqClassesPcsHalfLength"/>.
///       <br/>
///     - More in detail: to compare the PCS of even length L = <see cref="PcsLength"/> at index j1 = 
///       <see cref="Order"/>[i] with the one at index j2 = <see cref="Order"/>[i - 1], the following two comparisons 
///       are done: <c><see cref="EqClassesPcsHalfLength"/>[j1] == <see cref="EqClassesPcsHalfLength"/>[j2]</c> and
///       <c><see cref="EqClassesPcsHalfLength"/>[j1 + L / 2] == <see cref="EqClassesPcsHalfLength"/>[j2 + L / 2]</c>.
///       <br/>
///     - Finally, the equivalence class list is returned.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Iterating over the n PCS of length L is an O(n) operation. Each iteration does O(1) work, since
///       direct access to the equivalence class list of PCS of half length is done in constant time and comparison 
///       between the current PCS and the previous one is a comparison of the equivalence classes of the first and 
///       second halves of both PCS, and requires two pairs of integers to be compared.
///       <br/>
///     - Instantiating the equivalence class list of output is also an O(n) operation.
///       <br/>
///     - Therefore, Time and Space Complexity are O(n).
///     </para>
/// </remarks>
public class EqClassesBasedDoubleLengthPcsClassifier : IDoubleLengthPcsClassifier
{
    /// <summary>
    /// The length the PCS of input string to be classified.
    /// </summary>
    public int PcsLength { get; }

    /// <summary>
    /// The list of equivalence classes of the PCS of length <see cref="PcsLength"/> / 2, to be used to calculate the 
    /// equivalence classes of the PCS of double the length (<see cref="PcsLength"/>).
    /// </summary>
    public IList<int> EqClassesPcsHalfLength { get; }

    /// <summary>
    /// The position list of the PCS of length <see cref="PcsLength"/>, to be used, in addition to the 
    /// <see cref="EqClassesPcsHalfLength"/>, to calculate the equivalence classes of length <see cref="PcsLength"/>.
    /// </summary>
    public IList<int> Order { get; }

    /// <summary>
    ///     <inheritdoc cref="EqClassesBasedDoubleLengthPcsClassifier"/>
    /// </summary>
    /// <param name="pcsLength"><inheritdoc cref="PcsLength" path="/summary"/></param>
    /// <param name="eqClassesPcsHalfLength"><inheritdoc cref="EqClassesPcsHalfLength" path="/summary"/></param>
    /// <param name="order"><inheritdoc cref="Order" path="/summary"/></param>
    public EqClassesBasedDoubleLengthPcsClassifier(int pcsLength, IList<int> eqClassesPcsHalfLength, IList<int> order)
    {
        if (pcsLength % 2 != 0)
            throw new ArgumentException(
                $"Must be even.", nameof(pcsLength));
        if (pcsLength <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(pcsLength), $"Must be positive.");
        if (order.Count != eqClassesPcsHalfLength.Count)
            throw new ArgumentException(
                $"Must have the same number of items as {nameof(eqClassesPcsHalfLength)}.", nameof(order));

        PcsLength = pcsLength;
        EqClassesPcsHalfLength = eqClassesPcsHalfLength;
        Order = order;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="EqClassesBasedDoubleLengthPcsClassifier" path="/remarks"/>
    /// </remarks>
    public IList<int> Classify()
    {
        var n = EqClassesPcsHalfLength.Count;
        var eqClasses = new int[n];

        var (headList, tail) = Order.EnumerateExactlyFirst(1);
        var head = headList.Single();

        var currentEqClass = 0;
        eqClasses[head] = currentEqClass;

        var previousFirstHalfIndex = head;
        var previousSecondHalfIndex = (previousFirstHalfIndex + PcsLength / 2) % n;
        foreach (var firstHalfIndex in tail)
        {
            var secondHalfIndex = (firstHalfIndex + PcsLength / 2) % n;
            eqClasses[firstHalfIndex] =
                EqClassesPcsHalfLength[firstHalfIndex] == EqClassesPcsHalfLength[previousFirstHalfIndex] &&
                EqClassesPcsHalfLength[secondHalfIndex] == EqClassesPcsHalfLength[previousSecondHalfIndex] 
                ? currentEqClass 
                : ++currentEqClass;

            previousFirstHalfIndex = firstHalfIndex;
            previousSecondHalfIndex = secondHalfIndex;
        }

        return eqClasses;
    }
}