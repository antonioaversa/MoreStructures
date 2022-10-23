using MoreStructures.Utilities;

namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// A <see cref="IPivotSelectionStrategy"/> always picking the index of the window in the middle.
/// </summary>
/// <remarks>
/// Time and Space Complexity are O(1).
/// <br/>
/// Like for <see cref="StartIndexPivotSelectionStrategy"/> and <see cref="EndIndexPivotSelectionStrategy"/>, it makes 
/// quicksort run in quadratic time on pathological input configurations.
/// <br/>
/// However, unlike <see cref="StartIndexPivotSelectionStrategy"/> and <see cref="EndIndexPivotSelectionStrategy"/>, 
/// the pathological scenarios are much more unlikely to happen in the wild, and usually have to be manually crafted.
/// <br/>
/// To further reduce the possibility of phatological scenarios, and make sure that the resulting quicksort doesn't 
/// behave consistently bad on the same input, setup a <see cref="IShuffleStrategy"/> in the 
/// <see cref="RecursiveQuickSort"/> instance, different from <see cref="IdentityShuffleStrategy"/>.
/// </remarks>
public class MiddleIndexPivotSelectionStrategy : IPivotSelectionStrategy
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This specific implementation always picks the <paramref name="end"/> index.
    /// </remarks>
    public int Select<T>(IList<T> list, IComparer<T> comparer, int start, int end) => 
        (start, end).Middle();
}
