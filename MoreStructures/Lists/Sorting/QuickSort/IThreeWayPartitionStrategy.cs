namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// An algorithm partitioning the specified window of the provided list "3-way", i.e. into three adjacent segments: 
/// <br/>
/// - the first, named left segment, containing all items smaller or equal than the pivot,
///   <br/>
/// - the second, named middle segment, containing all items equals to the pivot,
///   <br/>
/// - and the third, named right segment, containing all items bigger or equal than the pivot.
/// </summary>
/// <remarks>
/// The definition is "loose" enough to allow for overlapping: values equal to the pivot can be in either of the three
/// segments.
/// <br/>
/// Some partition strategies may be stricter, and only place pivot values in the middle segments.
/// <br/>
/// Others may not do that, and just have an empty middle segments and pivot values split, evenly or not, in left and 
/// right segments.
/// <br/>
/// A 3-way partition strategy returning an empty middle segment is actually a 2-way partition strategy.
/// <br/>
/// This interface provide generality for partitioning schemes. However, the specific quicksort implementation may or 
/// may not support 2-way or 3-way partitions, empty partitions, etc.
/// </remarks>
public interface IThreeWayPartitionStrategy
{
    /// <summary>
    /// Partition the window of the provided <paramref name="list"/>, delimited by the indexes <paramref name="start"/>
    /// and <paramref name="end"/> included, into three segments, of items non-bigger, equal and non-smaller than the 
    /// pivot, respectively.
    /// </summary>
    /// <typeparam name="T">The type of items in <paramref name="list"/>.</typeparam>
    /// <param name="list">The list whose window has to be partitioned.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> for the items of <paramref name="list"/>.</param>
    /// <param name="start">The left index of the window of <paramref name="list"/>, included.</param>
    /// <param name="end">The right index of the window of <paramref name="list"/>, included.</param>
    /// <returns>
    /// Two indices: the left and right indices of the middle segment, included.
    /// <br/>
    /// If left and right index are equal, the middle segment is composed of a single value.
    /// <br/>
    /// If the right index is strictly smaller than the left index, the middle segment is empty.
    /// </returns>
    /// <remarks>
    /// If <c>Partition(L, C, s, e)</c> is equal to <c>(l, r)</c>, the three partitions are identified by:
    /// <br/>
    /// - <c>[s, l - 1]</c>
    ///   <br/>
    /// - <c>[l, r]</c>
    ///   <br/>
    /// - <c>[r + 1, e]</c>
    ///   <br/>
    /// where each of the partitions is empty if its left index is greater than its right index.
    /// </remarks>
    (int, int) Partition<T>(IList<T> list, IComparer<T> comparer, int start, int end);
}
