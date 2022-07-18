using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// An implementation of <see cref="IDoubleLengthPcsClassifier"/> which uses the externally provided position list of 
/// the PCS of length <see cref="PcsLength"/>, in addition to the input string, to generate equivalence classes.
/// </summary>
/// <remarks>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The externally provided ordered sequence of PCS <see cref="Order"/> is iterated over.
///       <br/>
///     - A new equivalence class is generated (increasing the current counter) every time a PCS differs from the 
///       previous one. The equivalence class is assigned to the output at the index of the PCS.
///       <br/>
///     - Finally, the equivalence class list is returned.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Iterating over the n PCS of length L is an O(n) operation. Each iteration does O(L) work, since, while
///       direct access to the equivalence class list is done in constant time, comparison between the current PCS
///       and the previous one is a comparison of two strings of length L, and requires all L chars to be comparerd in
///       the worst case.
///       <br/>
///     - Instantiating the equivalence class list of output is also an O(n) operation.
///       <br/>
///     - Therefore, Time Complexity, as driven by the iteration over <see cref="Order"/>, is O(n * L). Space 
///       Complexity, driven as well by iteration over <see cref="Order"/>, is O(L).
///     </para>
/// </remarks>
public class OrderBasedDoubleLengthPcsClassifier : IDoubleLengthPcsClassifier
{
    /// <summary>
    /// The input text, whose PCS of length <see cref="PcsLength"/> have to be classified.
    /// </summary>
    public string Input { get; }

    /// <summary>
    /// The length the PCS of <see cref="Input"/> to be classified.
    /// </summary>
    public int PcsLength { get; }

    /// <summary>
    /// The position list of the PCS of length <see cref="PcsLength"/>, to be used, in addition to the 
    /// <see cref="Input"/>, to calculate the equivalence classes.
    /// </summary>
    public IList<int> Order { get; }

    /// <summary>
    ///     <inheritdoc cref="OrderBasedDoubleLengthPcsClassifier"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="Input" path="/summary"/></param>
    /// <param name="pcsLength"><inheritdoc cref="PcsLength" path="/summary"/></param>
    /// <param name="order"><inheritdoc cref="Order" path="/summary"/></param>
    public OrderBasedDoubleLengthPcsClassifier(string input, int pcsLength, IList<int> order)
    {
        if (pcsLength <= 0 || pcsLength > input.Length)
            throw new ArgumentOutOfRangeException(
                nameof(pcsLength), $"Must be positive and at most the length of {nameof(input)}.");
        if (order.Count != input.Length)
            throw new ArgumentException(
                $"Must have the same number of items as chars in {nameof(input)}.", nameof(order));

        Input = input;
        PcsLength = pcsLength;
        Order = order;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="OrderBasedDoubleLengthPcsClassifier" path="/remarks"/>
    /// </remarks>
    public IList<int> Classify()
    {
        var eqClasses = new int[Input.Length];

        var (headList, tail) = Order.EnumerateExactlyFirst(1);
        var head = headList.Single();

        var currentEqClass = 0;
        eqClasses[head] = currentEqClass;

        var previousPcs = PcsUtils.ExtractPcsOf(Input, head, PcsLength);
        foreach (var currentIndex in tail)
        {
            var currentPcs = PcsUtils.ExtractPcsOf(Input, currentIndex, PcsLength);
            eqClasses[currentIndex] = currentPcs == previousPcs ? currentEqClass : ++currentEqClass;
            previousPcs = currentPcs;
        }

        return eqClasses;
    }
}