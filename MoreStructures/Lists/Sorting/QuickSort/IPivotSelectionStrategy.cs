namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// An algorithm to select the pivot from the provided window of a list, to be used by a 
/// <see cref="IThreeWayPartitionStrategy"/>, to partition the window into three segments.
/// </summary>
/// <remarks>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - For the overall quicksort algorithm to have the expected complexity, pivot selection should not exceed the 
///       linear Time Complexity.
///       <br/>
///     - While a linear time algorithm would ensure best pivot selection and list window partition (e.g. picking the 
///       actual median of all values), and while it would still keep overall quicksort runtime to be linearithmic, the 
///       multiplicative factor of n * log(n) would be much higher than a quicksort based on a O(1) pivot selection, 
///       such as a deterministic choice or a randomized one.
///       <br/>
///     - So, even asymptotically, the advantages of having a smart, O(n), pivot selection would be overwhelmed by the
///       negative impact of the pivot selection cost at every recursive call of the quicksort on a window.
///     </para>
/// </remarks>
public interface IPivotSelectionStrategy
{
    /// <summary>
    /// Selects a pivot from the window of the provided <paramref name="list"/>, defined by <paramref name="start"/>
    /// and <paramref name="end"/> indices.
    /// </summary>
    /// <typeparam name="T">The type of items in <paramref name="list"/>.</typeparam>
    /// <param name="list">The list, whose pivot has to be selected.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> for the items of <paramref name="list"/>.</param>
    /// <param name="start">The left index of the window of <paramref name="list"/>, included.</param>
    /// <param name="end">The right index of the window of <paramref name="list"/>, included.</param>
    /// <returns>The index of the pivot in <paramref name="list"/>.</returns>
    int Select<T>(IList<T> list, IComparer<T> comparer, int start, int end);
}
