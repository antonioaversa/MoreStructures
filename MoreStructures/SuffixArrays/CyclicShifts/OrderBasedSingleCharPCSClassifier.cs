using MoreStructures.Strings.Sorting;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// A <see cref="ISingleCharPcsClassifier"/> implementation which uses an externally provided position list
/// <see cref="Order"/> of the 1-char PCS of the <see cref="Input"/>, to calculate equivalence classes.
/// </summary>
/// <remarks>
///     <inheritdoc cref="ISingleCharPcsClassifier"/>
///     <para id="advantages">
///     ADVANTAGES
///     <br/>
///     - Compared to the naive implementation of <see cref="NaiveSingleCharPcsClassifier"/>, it has better runtime and
///       only allocates an array of n elements, where n is the length of the <see cref="Input"/>.
///       <br/>
///     - However, it requires the position list of the <see cref="Input"/> to be provided to the classifier at
///       construction time.
///       <br/>
///     - Calculating the position list is a linear time operation only when some assumptions can be made on the input,
///       such as an alphabet of limited size (which is the main scenario for <see cref="CountingSortCharsSorter"/>).
///       In all other cases, the runtime has a worst case of O(n * log(n)).
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - An array of n elements EC, to accomodate the equivalence classes of the result, is allocated upfront.
///       <br/>
///     - The item of EC with index <c>P[0]</c>, where P is the list of positions, is set to 0. This is because 
///       <c>P[0]</c> is the index in the <see cref="Input"/> T, of the smallest char in T. Therefore, there are no 
///       smaller chars in T and the equivalence class of <c>T[P[0]]</c> is hence the smallest, i.e. 0.
///       <br/>
///     - <c>EC[O[i + 1]]</c> can be calculated from <c>EC[O[i]]</c>: two scenarios are possible.
///       <br/>
///     - If <c>T[O[i + 1]] == T[O[i]]</c>, it means that the two chars <c>T[O[i + 1]]</c> and <c>T[O[i]]</c>, which 
///       come one after the other one in the position list, are the same and should have the same equivalence class.
///       Therefore, the equivalence class of <c>T[O[i + 1]]</c>, <c>EC[O[i + 1]]</c> can be set to the equivalence 
///       class of <c>T[O[i]]</c>, <c>EQ[O[i]]</c>.
///       <br/>
///     - If, on the other hand, <c>T[O[i + 1]] != T[O[i]]</c>, it means that the two chars <c>T[O[i + 1]]</c> and 
///       <c>T[O[i]]</c>, which come one after the other one in the position list, are different and should have
///       different equivalence classes. Therefore, the equivalence class of <c>T[O[i + 1]]</c>, <c>EC[O[i + 1]]</c> 
///       can be set to the successor of the equivalence class of <c>T[O[i]]</c>, <c>EQ[O[i]]</c>.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - There are as many iterations as items of the position list.
///       <br/>
///     - Within each iteration, direct accesses to items of <see cref="Input"/> and <see cref="Order"/> by index are 
///       done in constant time.
///       <br/>
///     - Therefore, Time and Space Complexity are O(n), excluding the cost of calculating the position list, which is
///       externally provided. Check <see cref="ICharsSorter"/> implementations for the complexity of the algorithms
///       calculating the position list of a string.
///     </para>
/// </remarks>
public class OrderBasedSingleCharPcsClassifier : ISingleCharPcsClassifier
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
    ///     <inheritdoc cref="OrderBasedSingleCharPcsClassifier"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="Input" path="/summary"/></param>
    /// <param name="order"><inheritdoc cref="Order" path="/summary"/></param>
    public OrderBasedSingleCharPcsClassifier(string input, IList<int> order)
    {
        if (input.Length != order.Count)
            throw new ArgumentException($"Must have as many items as chars in {nameof(input)}.", nameof(order));

        Input = input;
        Order = order;
    }

    /// <inheritdoc cref="ISingleCharPcsClassifier" path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="OrderBasedSingleCharPcsClassifier"/>
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
