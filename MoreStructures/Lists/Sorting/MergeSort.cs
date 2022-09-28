namespace MoreStructures.Lists.Sorting;

/// <summary>
/// An <see cref="IInPlaceSorting"/> implementation based on merge sort.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm uses a "divide et impera" approach, recursively splitting the list to sort of size n in two 
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
public class MergeSort : IInPlaceSorting
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
        var temp = new T[list.Count];
        RecursiveSort(list, temp, comparer, 0, list.Count - 1);
    }

    private static void RecursiveSort<T>(
        IList<T> list, IList<T> temp, IComparer<T> comparer, int startIndex, int endIndex)
    {
        if (endIndex - startIndex <= 0)
            return;

        var middleIndex = (startIndex + endIndex) / 2;
        RecursiveSort(list, temp, comparer, startIndex, middleIndex);
        RecursiveSort(list, temp, comparer, middleIndex + 1, endIndex);

        var k1 = startIndex;
        var k2 = middleIndex + 1;
        var k = startIndex;
        while (k <= endIndex)
        {
            if (k1 > middleIndex)
                temp[k] = list[k2++];
            else if (k2 > endIndex)
                temp[k] = list[k1++];
            else if (comparer.Compare(list[k1], list[k2]) <= 0)
                temp[k] = list[k1++];
            else
                temp[k] = list[k2++];

            k++;
        }

        for (var i = startIndex; i <= endIndex; i++)
            list[i] = temp[i];
    }
}
