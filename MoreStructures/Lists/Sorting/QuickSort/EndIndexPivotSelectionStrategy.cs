namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// A <see cref="IPivotSelectionStrategy"/> always picking the highest index of the window.
/// </summary>
/// <remarks>
/// Time and Space Complexity are O(1).
/// <br/>
/// Like for <see cref="StartIndexPivotSelectionStrategy"/>, it makes quicksort run in quadratic time on pathological 
/// input configurations, e.g. when window is already sorted in descending order.
/// <br/>
/// To avoid phatological scenarios with this <see cref="IPivotSelectionStrategy"/>, setup a
/// <see cref="IShuffleStrategy"/> in the <see cref="RecursiveQuickSort"/> instance, different from 
/// <see cref="IdentityShuffleStrategy"/>.
/// </remarks>
public class EndIndexPivotSelectionStrategy : IPivotSelectionStrategy
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This specific implementation always picks the <paramref name="end"/> index.
    /// </remarks>
    public int Select<T>(IList<T> list, IComparer<T> comparer, int start, int end) => end;
}
