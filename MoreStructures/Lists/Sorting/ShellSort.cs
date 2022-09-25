namespace MoreStructures.Lists.Sorting;

/// <summary>
/// An <see cref="IInPlaceSorting"/> implementation based on Shell sort.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to <see cref="InsertionSort"/>, it has better runtime, because it reduces the total number of 
///       swapping operations performed.
///       <br/>
///     - That comes the cost of its complexity, which is higher than <see cref="InsertionSort"/> since Shell sort 
///       requires execution of <see cref="InsertionSort"/> on multiple interleaved lists.
///       <br/>
///     - However, its performance heavily depends on the chosen gap sequence, with best cases being O(n * log(n)) and 
///       worst cases being O(n^2), depending on the gap sequence.
///       <br/>
///     - Its average complexity characterization is to this day an open problem, and its lack of predictability is
///       one of its main drawbacks. 
///       <br/>
///     - Unlike <see cref="InsertionSort"/>, it is not stable, i.e. it doesn't preserve the order of items in the 
///       input with the same sorting key. This is due to the fact that the algorithm performs sorting of the subset of
///       the input with the highest gap first, potentially skipping first occurrences of some of the duplicated items.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - This sorting algorithm runs the basic insertion sort algorithm, multiple times and with multiple steps.
///       <br/>
///     - In a standard insertion sort, comparisons and swapping at the iteration i of the main loop of the algorithm 
///       are always made between items at consecutive locations, going back until the i-th item is placed in order
///       w.r.t. all preceding items.
///       <br/>
///     - That means that large items appearing early in the input will be swapped many times, before reaching their 
///       final position in the input.
///       <br/>
///     - For example the items 5 to 9 in the input [5, 6, 7, 8, 9, 0, 1, 2, 3, 4] are swapped to make room for the
///       items 0 to 4. After 4 iterations, the iteration with i = 5 places 0 correctly by shifting from 9 all the way 
///       down to 5. Same with all following iterations, with a total of 5 * 5 = 25 swapping operations before the 
///       value 9 gets into its final place.
///       <br/>
///     - The idea behind Shell sort is that it gives the chance to bigger values to be placed closer to their final 
///       position, faster than insertion sort. 
///       <br/>
///     - It does that by sorting via insertion sort not just once and for the entire input, but multiple times and on
///       different sub-sets of it. Sub-sets are appropriately chosen, in a way that bigger values require less 
///       swapping before getting to their final place.
///       <br/>
///     - The first sub-set of indexes sorted is defined by all locations of the input which are at a relative distance
///       equal to the gap. The gap is first set at its highest value and then decreased at every iteration.
///       <br/>
///     - Gap sequence is externally provided. For example, if the sequence provided is the powers of 2, the actual gap
///       sequence used for an input of 10 items is [8, 4, 2, 1] (8 is the highest power of 2 smaller than 10).
///       <br/>
///     - After the 1st run of insertion sort (gap = 8), the list is [2, 6, 7, 8, 9, 0, 1, 5, 3, 4].
///       <br/>
///     - After the 2nd run of insertion sort (gap = 4), the list is [2, 6, 7, 8, 4, 0, 1, 5, 3, 9].
///       <br/>
///     - As visible from the example, after only 3 comparisons, both the smallest and biggest items are already in 
///       place.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Space Complexity is always O(1), since the algorithm runs in place.
///       <br/>
///     - Time Complexity, however, is strictly connected to the gap sequence selected.
///       <br/>
///     - Worst-case performance is O(n^2), assuming worst-case gap sequence, and O(n * log^2(n)), assuming best-case
///       gap sequence.
///       <br/>
///     - Best-case performance is O(n * log^2(n)), assuming worst-case gap sequence, and O(n * log(n)), assuming
///       best-case gap sequence.
///       <br/>
///     - Best-case performance is still O(n * log(n)) for most gap sequences in general.
///     </para>
/// </remarks>
public class ShellSort : IInPlaceSorting
{
    /// <summary>
    /// A generator of a monotonically strictly increasing sequence of <see cref="int"/>, each representing a gap 
    /// between locations of the input.
    /// </summary>
    protected IEnumerable<int> GapGenerator { get; }

    /// <inheritdoc cref="ShellSort"/>
    /// <param name="gapGenerator">
    ///     <inheritdoc cref="GapGenerator" path="/summary"/>
    /// </param>
    public ShellSort(IEnumerable<int> gapGenerator)
    {
        GapGenerator = gapGenerator;
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the Shell sort algorithm with the default comparer for <typeparamref name="T"/>, given by
    ///     <see cref="Comparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="ShellSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list) where T : IComparable<T> =>
        Sort(list, Comparer<T>.Default);

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the Shell sort algorithm with the specified <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="ShellSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list, IComparer<T> comparer)
    {
        var count = list.Count;
        var gaps = GapGenerator
            .TakeWhile(gap => gap < count)
            .Reverse();
        foreach (var gap in gaps)
        {
            InsertionSortHelpers.InsertionSortOnHthIndexes(list, comparer, gap);
        }
    }
}
