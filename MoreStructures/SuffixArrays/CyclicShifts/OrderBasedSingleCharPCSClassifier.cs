using MoreStructures.Strings.Sorting;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// A <see cref="ISingleCharPCSClassifier"/> implementation which uses an externally provided position list
/// <see cref="Order"/> of the 1-char PCS of the <see cref="Input"/>, to calculate equivalence classes.
/// </summary>
/// <remarks>
///     <inheritdoc cref="ISingleCharPCSClassifier"/>
///     <para id="advantages">
///     ADVANTAGES
///     <br/>
///     TODO
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     TODO
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     TODO
///     </para>
/// </remarks>
public class OrderBasedSingleCharPCSClassifier : ISingleCharPCSClassifier
{
    /// <inheritdoc/>
    public string Input { get; }

    /// <summary>
    /// The position list of the 1-char PCS of the <see cref="Input"/>.
    /// </summary>
    /// <remarks>
    /// Has as many items as chars in the <see cref="Input"/>.
    /// <br/>
    /// It is specifically required by this implementation of the algorithm and has to be externally provided.
    /// <br/>
    /// It can be calculated with any implementation of <see cref="ICharsSorter.Sort(string)"/>.
    /// </remarks>
    public IList<int> Order { get; }

    /// <summary>
    ///     <inheritdoc cref="OrderBasedSingleCharPCSClassifier"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="Input" path="/summary"/></param>
    /// <param name="order"><inheritdoc cref="Order" path="/summary"/></param>
    public OrderBasedSingleCharPCSClassifier(string input, IList<int> order)
    {
        if (input.Length != order.Count)
            throw new ArgumentException($"Must have as many items as chars in {nameof(input)}.", nameof(order));

        Input = input;
        Order = order;
    }

    /// <inheritdoc cref="ISingleCharPCSClassifier" path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="OrderBasedSingleCharPCSClassifier"/>
    /// </remarks>
    public IList<int> Classify()
    {
        if (Input.Length == 0)
            return Array.Empty<int>();

        var result = new int[Order.Count];
        result[Order[0]] = 0;
        var currentEqClass = 0;
        for (int i = 1; i < Order.Count; i++)
            result[Order[i]] = Input[Order[i]] == Input[Order[i - 1]] ? currentEqClass : ++currentEqClass;
        return result;
    }
}
