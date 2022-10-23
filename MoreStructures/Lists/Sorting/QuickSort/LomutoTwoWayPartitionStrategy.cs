namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// A <see cref="IThreeWayPartitionStrategy"/> implementing the Lomuto partition scheme into three segments, the 
/// middle always being a single pivot.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES
///     <br/>
///     - Compared to <see cref="LomutoThreeWayPartitionStrategy"/> and HoareTwoWayPartitionStrategy, it 
///       is simpler, since it only requires a single pointer.
///       <br/>
///     - However it makes quicksort quadratic when many duplicates are present, because it places all the duplicates 
///       of the pivot in the right segment.
///       <br/>
///     - Notice that the same would happen if the strict inequality on the comparison result done in partitioning 
///       would have been replaced by a non-strict inequality, because in that scenarios all duplicates would have been
///       placed in the left segment.
///       <br/>
///     - Only a randomized strategy, not implemented by this strategy, could have distributed evenly pivots in the two
///       partitions.
///       <br/>
///     - <see cref="LomutoThreeWayPartitionStrategy"/> solves this problem differently, by placing all pivots in the 
///       middle segment. That, however, requires 2 indices and its more complex because it generates three segments 
///       which can contain more than 1 element each, including the middle segment.
///       <br/>
///     - HoareTwoWayPartitionStrategy addresses the problem by still using two pointers, but running 
///       from the two extremes of the window, inwards. That distributes the duplicates evenly (in average, over non 
///       pathological input lists) and only generates two segments, with the middle segment empty.
///       <br/>
///     - Both <see cref="LomutoTwoWayPartitionStrategy"/> and <see cref="LomutoThreeWayPartitionStrategy"/> perform 
///       three times more swaps than HoareTwoWayPartitionStrategy on average.
///       <br/>
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - First, the index ip, of the item of the list acting as pivot, is selected, using the provided 
///       <see cref="PivotSelectionStrategy"/>.
///       <br/>
///     - L[ip] is swapped with L[end], i.e. the pivot is moved at the end of the window.
///       <br/>
///     - Then, the value of the pivot p is taken from the list being sorted L, from the last item of the window, where
///       it has just been placed.
///       <br/>
///     - A pointer i is defined, to point to the frontier between values smaller than p, and more precisely to the 
///       last index of the left segment, which is also the index before the first index of the current middle segment.
///       <br/>
///     - Its initial value is start - 1, since at the beginning the left segment is empty.
///       <br/>
///     - A running pointer j goes from the start index to the index before the end (the end index contains the pivot).
///       <br/>
///     - For each item L[j] smaller than the pivot, i is increased, to augment the left segment of one item, and items
///       at positions i and j are swapped, to accomodate L[j] in the left segment, since it is smaller than the pivot.
///       <br/>
///     - That moves the previous L[i] beyond the frontier, and that is correct, since it is non-smaller than the 
///       pivot. 
///       <br/>
///     - By the time j will have run through all indices except the last, all items except the last will have been 
///       partitioned into a left and right segments.
///       <br/>
///     - By incrementing i one more time and swapping L[i] with L[end], the pivot goes into the right position, and 
///       the three segments are in place: the left going from start to i - 1, the middle from i to i (single item) and
///       the right going from i + 1 to end.
///       <br/>
///     - So, <c>(i, i)</c> is returned as result.
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
///     - At each iteration, at most a swap and two increments are performed (j and possibly i).
///       <br/>
///     - Therefore Time Complexity is O(n) and Space Complexity is O(1), since all swaps happen in-place on L.
///     </para>
/// </remarks>
public class LomutoTwoWayPartitionStrategy : IThreeWayPartitionStrategy
{
    /// <summary>
    /// The strategy to select the pivot.
    /// </summary>
    public IPivotSelectionStrategy PivotSelectionStrategy { get; }

    /// <summary>
    /// Builds a Lomuto 2-way partitioner with the provided <paramref name="pivotSelectionStrategy"/>.
    /// </summary>
    /// <param name="pivotSelectionStrategy">
    ///     <inheritdoc cref="PivotSelectionStrategy"/>
    /// </param>
    public LomutoTwoWayPartitionStrategy(IPivotSelectionStrategy pivotSelectionStrategy)
    {
        PivotSelectionStrategy = pivotSelectionStrategy;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc/>
    ///     <inheritdoc cref="LomutoTwoWayPartitionStrategy"/>
    /// </remarks>
    public (int, int) Partition<T>(IList<T> list, IComparer<T> comparer, int start, int end)
    {
        var pivotIndex = PivotSelectionStrategy.Select(list, comparer, start, end);
        (list[end], list[pivotIndex]) = (list[pivotIndex], list[end]);

        var pivot = list[end];
        var i = start - 1;
        for (var j = start; j < end; j++)
        {
            if (comparer.Compare(list[j], pivot) < 0)
            {
                i++;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        i++;
        (list[end], list[i]) = (list[i], list[end]);
        return (i, i);
    }
}
