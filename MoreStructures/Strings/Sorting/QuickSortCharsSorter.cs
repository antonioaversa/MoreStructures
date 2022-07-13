using MoreLinq;

namespace MoreStructures.Strings.Sorting;

/// <summary>
/// An implementation of <see cref="ICharsSorter"/> which uses QuickSort to sort the input.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES
///     <br/>
///     The algorithm uses the QuickSort implementation of LINQ to sort strings, seen as lists of chars.
///     <br/>
///     Being based on a comparison-based sorting algorithm, unlike the Counting Sort implementation, it doesn't 
///     leverage a linear runtime.
///     <br/>
///     However, its runtime only depends on the size of the input, and it doesn't depend on the size of the alphabet.
///     <br/>
///     Because of that, it is suitable for scenarios of large alphabets, or where there is no upper bound on the 
///     number of distinct chars in the input, other than the size of the alphabet.
///     <br/>
///     For example: when the input string to be sorted is an unconstrained Unicode string.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - It uses the LINQ-provided QuickSort, to sort the <see cref="KeyValuePair"/> couples defined by (index, char),
///       for each char of the text, in ascending order.
///       <br/>
///     - Then, it selects the indexes from the sorted sequence of <see cref="KeyValuePair"/> instances, and runs a
///       second QuickSort, to get the position list.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Each execution of the QuickSort has a runtime of O(n * log(n)).
///       <br/>
///     - Output is not produced in-place, but a new list of n items is created.
///       <br/>
///     - Therefore, Time Complexity is O(n * log(n)) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class QuickSortCharsSorter : ICharsSorter
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="QuickSortCharsSorter" path="/remarks"/>
    /// </remarks>
    public IList<int> Sort(string input)
    {
        return input
            .Index()
            .OrderBy(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .Index()
            .OrderBy(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .ToList();
    }
}
