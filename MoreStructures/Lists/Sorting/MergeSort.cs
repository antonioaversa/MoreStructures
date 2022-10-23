using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Lists.Sorting;

/// <summary>
/// An <see cref="IInputMutatingSort"/> implementation based on the merge sort algorithm.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGE
///     <br/>
///     - Compared to <see cref="SelectionSort"/> and <see cref="InsertionSort"/>, it has asymptotically better 
///       runtime, linearithmic instead of quadratic.
///       <br/>
///     - Compared to <see cref="RecursiveQuickSort"/>, it has <b>better worst-case performance</b>: linearithmic, instead of
///       quadratic.
///       <br/>
///     - In general <see cref="MergeSort"/> does a number of comparisons which is independent from the 
///       input, whereas the number of comparisons in <see cref="RecursiveQuickSort"/> depends on the input and in particular on
///       the choice of the pivot.
///       <br/>
///     - Unlike <see cref="SelectionSort"/>, <see cref="RecursiveQuickSort"/> and <see cref="ShellSort"/>, and like 
///       <see cref="InsertionSort"/> it is a <b>stable</b> sorting algorithm, so it preserves the order in the input 
///       of items with the same key.
///       <br/>
///     - A disadvantage over many other sorting algorithms, such as <see cref="SelectionSort"/>, 
///       <see cref="InsertionSort"/>, <see cref="ShellSort"/> and <see cref="RecursiveQuickSort"/>, is that it is <b>not
///       in place</b>, as it requires additional O(n) space to perform the sorting.
///       <br/>
///     - An advantage over many other sorting algorithms, and in particular over <see cref="RecursiveQuickSort"/>, is that it 
///       is <b>easily parallelizable</b>. 
///       <br/>
///     - The reason is that <see cref="MergeSort"/> is based on a "divide and conquer" design, where the partition of 
///       the problem in sub-problems is done in O(1) time (just split the array in two halves) while the combine phase
///       (i.e. the merge) is more complex, taking O(n) time. In <see cref="RecursiveQuickSort"/> it is rather the opposite: the 
///       partition is the O(n) operation, while the combine comes for free.
///       <br/>
///     - Another advantage of <see cref="MergeSort"/> over other comparison-based algorithms is that it performs an
///       <b>optimal number of comparisons</b>: n * log(n). No other comparison-based algorithm can do better. That,
///       however, doesn't mean that <see cref="MergeSort"/> performs better than any other comparison-based algorithm:
///       while <see cref="RecursiveQuickSort"/> performs more comparisons in average (~ 39% more, in practice), it also does
///       sorting in place and does many less operations, resulting in better performance.
///       <br/>
///     - <see cref="MergeSort"/> becomes the best sorting algorithm in scenarios where random access to the input is
///       particular expensive, which can be the case for "sequential implementations" of <see cref="IList{T}"/>. An
///       example would be <see cref="LinkedList{T}"/>: while linked lists cannot provide O(1) random access, and for
///       that reason don't implement the <see cref="IList{T}"/> interface, an adapter of <see cref="LinkedList{T}"/> 
///       to <see cref="IList{T}"/> would benefit from the items access pattern of <see cref="MergeSort"/> over 
///       <see cref="RecursiveQuickSort"/>.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm uses a "divide et conquer" approach, recursively splitting the list to sort of size n in two 
///       halves of size roughly n / 2 and using an auxiliary structure to merge the two sorted halves into a single 
///       sorted list.
///       <br/>
///     - Before starting the recursion over the list to sort L, a temporary array T, of the same size n of L, is 
///       instantiated, to be used for merging of sorted halves.
///       <br/>
///     - Then the recursive merge call is made, passing L, A and lower and upper indexes equal to 0 and n - 1 
///       respectively.
///       <br/>
///     - Each recursive call split its input, which corresponds to the window of L from the lower to the upper index
///       into two halves, which are then recombined using A.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Every recursive call splits the input into two halves of roughly equal size.
///       <br/>
///     - Therefore, the depth of recursion is the logarithm of n in base 2.
///       <br/>
///     - Each recursive call performs an amount of work which is linear in the size of its input, and also uses a 
///       linear amount of additional space in T, which, however, is instantiated once, at top level.
///       <br/>
///     - Therefore, Time Complexity is O(n * log(n)) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class MergeSort : IInputMutatingSort
{
    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the merge sort algorithm with the default comparer for <typeparamref name="T"/>, given by
    ///     <see cref="Comparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="MergeSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list) where T : IComparable<T> =>
        Sort(list, Comparer<T>.Default);

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the merge sort algorithm with the specified <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="MergeSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list, IComparer<T> comparer)
    {
        var temp = list.ToArray();
        RecursiveSort(list, temp, comparer, 0, list.Count - 1);
    }

    private static void RecursiveSort<T>(
        IList<T> list, IList<T> temp, IComparer<T> comparer, int startIndex, int endIndex)
    {
        if (endIndex <= startIndex)
            return;
        
        var middleIndex = startIndex + (endIndex - startIndex) / 2;
        (temp, list) = (list, temp); // Swap roles of list and temp
        RecursiveSort(list, temp, comparer, startIndex, middleIndex);
        RecursiveSort(list, temp, comparer, middleIndex + 1, endIndex);
        Merge(temp, list, comparer, startIndex, middleIndex, endIndex);
    }

    private static void Merge<T>(
        IList<T> targetList, IList<T> sourceList, IComparer<T> comparer, int startIndex, int middleIndex, int endIndex)
    {
        var k1 = startIndex;
        var k2 = middleIndex + 1;
        var k = startIndex;
        while (k <= endIndex)
        {
            if (k1 > middleIndex)
                targetList[k] = sourceList[k2++];
            else if (k2 > endIndex)
                targetList[k] = sourceList[k1++];
            else if (comparer.Compare(sourceList[k1], sourceList[k2]) <= 0)
                targetList[k] = sourceList[k1++];
            else
                targetList[k] = sourceList[k2++];

            k++;
        }
    }
}
