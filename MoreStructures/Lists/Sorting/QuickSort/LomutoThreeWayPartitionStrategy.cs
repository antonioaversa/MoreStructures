namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// A <see cref="IThreeWayPartitionStrategy"/> implementing the Lomuto partition scheme into three segments, the 
/// middle containing only and all the items equals to the pivot.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES
///     <br/>
///     - Compared to <see cref="LomutoTwoWayPartitionStrategy"/> it has the advantage of not making quicksort 
///       quadratic when many duplicates are present, since it places all the duplicates in the middle segment,
///       which is not recursed over.
///       <br/>
///     - This advantage comes, however, at the cost of its complexity, since two pointers and three non-empty segments
///       have to be setup and kept coherent throughout the entire execution of the partitioning algorithm.
///       <br/>
///     - Other advantages and disadvantages over other partitioning schemes, such as 
///       HoareTwoWayPartitionStrategy are described in the documentation of 
///       <see cref="LomutoTwoWayPartitionStrategy"/>.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm closely resembles the one defined in <see cref="LomutoTwoWayPartitionStrategy"/>:
///       pivot selection, using the provided <see cref="PivotSelectionStrategy"/> and swapping with L[end] happen in
///       exactly the same way.
///       <br/>
///     - Instead of a single pointer, two pointers i1 and i2 are defined: to the last index of the left segment, and 
///       to the first index of the right segment, set to start - 1 and start respectively.
///       <br/>
///     - A running pointer j goes from the start index to the index before the end (the end index contains the pivot).
///       <br/>
///     - Each item L[j] smaller than the pivot is added after the end of the left segment, which is increased by 1.
///       <br/>
///     - In order to make room for L[j] in the left segment, the middle segment is "shifted" by one index on the 
///       right, more precisely by moving the first item of the middle segment after its end, and placing the first 
///       item of the right segment in position j.
///       <br/>
///     - Each item L[j] equal to the pivot is added after the end of the middle segment, which is increased by 1.
///       <br/>
///     - In this case there is no need to touch the left segment, and it's enough to swap L[i2] with L[j], and then 
///       increase i2.
///       <br/>
///     - By the time j will have run through all indices except the last, all items except the last will have been 
///       partitioned into a left, middle and right segments.
///       <br/>
///     - By incrementing i one more time and swapping L[i] with L[end], the pivot goes into the right position, and 
///       the three segments are in place: the left going from start to i1, the middle from i1 + 1 to i2 (at least an
///       item, possibly more than one) and the right going from i2 + 1 to end.
///       <br/>
///     - So, <c>(i1 + 1, i2)</c> is returned as result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Pivot selection complexity depends on the specific <see cref="IPivotSelectionStrategy"/> strategy 
///       used, and is not included in this analysis (i.e. it is assumed to be O(1)).
///       <br/>
///     - Pivot swapping, before and after the main loop, are both constant-time operations.
///       <br/>
///     - The main loop goes through n - 1 items, where n is the number of items in the window.
///       <br/>
///     - At each iteration, at most two swap and three increments are performed (j and possibly i1 and i2).
///       <br/>
///     - Therefore Time Complexity is O(n) and Space Complexity is O(1), since all swaps happen in-place on L.
///     </para>
/// </remarks>
public class LomutoThreeWayPartitionStrategy : IThreeWayPartitionStrategy
{
    /// <summary>
    /// The strategy to select the pivot.
    /// </summary>
    public IPivotSelectionStrategy PivotSelectionStrategy { get; }

    /// <summary>
    /// Builds a Lomuto 3-way partitioner with the provided <paramref name="pivotSelectionStrategy"/>.
    /// </summary>
    /// <param name="pivotSelectionStrategy">
    ///     <inheritdoc cref="PivotSelectionStrategy"/>
    /// </param>
    public LomutoThreeWayPartitionStrategy(IPivotSelectionStrategy pivotSelectionStrategy)
    {
        PivotSelectionStrategy = pivotSelectionStrategy;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc/>
    ///     <inheritdoc cref="LomutoThreeWayPartitionStrategy"/>
    /// </remarks>
    public (int, int) Partition<T>(IList<T> list, IComparer<T> comparer, int start, int end)
    {
        var pivotIndex = PivotSelectionStrategy.Select(list, comparer, start, end);
        (list[end], list[pivotIndex]) = (list[pivotIndex], list[end]);

        var pivot = list[end];
        var i1 = start - 1;
        var i2 = start;
        for (var j = start; j < end; j++)
        {
            var comparisonResult = comparer.Compare(list[j], pivot);
            if (comparisonResult < 0)
            {
                i1++;
                (list[i2], list[j]) = (list[j], list[i2]);
                (list[i1], list[i2]) = (list[i2], list[i1]);
                i2++;
            }
            else if (comparisonResult == 0)
            {
                (list[i2], list[j]) = (list[j], list[i2]);
                i2++;
            }
        }

        (list[i2], list[end]) = (list[end], list[i2]);
        return (i1 + 1, i2);
    }
}
