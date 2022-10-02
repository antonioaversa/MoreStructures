namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// A <see cref="IPivotSelectionStrategy"/> always picking the lowest index of the window.
/// </summary>
/// <remarks>
/// Time and Space Complexity are O(1).
/// <br/>
/// While one of the simplest way of selecting a pivot, when used in a <see cref="IThreeWayPartitionStrategy"/> of a
/// <see cref="RecursiveQuickSort"/> instance, it makes quicksort run in quadratic time on pathological input 
/// configurations, and in particular when the input window is already sorted in ascending order.
/// <br/>
/// To avoid phatological scenarios with this <see cref="IPivotSelectionStrategy"/>, setup a
/// <see cref="IShuffleStrategy"/> in the <see cref="RecursiveQuickSort"/> instance, different from 
/// <see cref="IdentityShuffleStrategy"/>.
/// </remarks>
public class StartIndexPivotSelectionStrategy : IPivotSelectionStrategy
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This specific implementation always picks the <paramref name="start"/> index.
    /// </remarks>
    public int Select<T>(IList<T> list, IComparer<T> comparer, int start, int end) => start;
}
