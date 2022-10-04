namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// An <see cref="IInputMutatingSort"/> implementation based on the quicksort algorithm.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Like <see cref="MergeSort"/>, it belongs to the category of linearithmic comparison-based sorting algorithm
///       (when setup correctly, see the complexity analysis section below).
///       <br/>
///     - Compared to <see cref="MergeSort"/>, it is <b>in-place</b>, having O(1) Space Complexity.
///       <br/>
///     - However, it is <b>not stable</b> and it's <b>not as easy parallelizable</b> as <see cref="MergeSort"/>.
///       <br/>
///     - While it is not optimal in the exact number of comparisons (it does in average 39% more comparisons than
///       <see cref="MergeSort"/>, which is optimal on the number of comparisons), it does way less swapping and moving
///       operations, resulting in a visibly better performance, especially when cost of moving items is high.
///       <br/>
///     - For that reason is tipically the <b>algorithm of choice when sorting primitive values or value types</b>, 
///       such as struct instances, where cost of comparison is low and cost of swapping can be high, depending on the 
///       size of the struct.
///       <br/>
///     - When sorting reference types, such as class instances, <see cref="MergeSort"/> is sometimes preferred, since
///       swapping items in the list means swapping their references, which are of fixed and small size, and instead
///       the cost of comparison can be quite high, depending on the <see cref="IComparable{T}"/> or on the 
///       <see cref="IComparer{T}"/> implementation used.
///       <br/>
///     - Compared to most other comparison-based algorithms, a disadvantage of quicksort is that, for it to have 
///       consistently good performances, it has to be randomized. In such setup, it is <b>not deterministic</b>.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - First, it shuffles the input to sort, by using the <see cref="IShuffleStrategy"/> provided in the 
///       constructor.
///       <br/>
///     - Then, it partitions the shuffled input into three segments: left, middle and right.
///       <br/>
///     - Partitioning is done via the <see cref="IThreeWayPartitionStrategy"/> provided in the constructor. The way in
///       which partitions are done, and in particular whether pivot values only appear in the middle segment, only in 
///       left and/or right segments or in all three segments, depends on the specific strategy used.
///       <br/>
///     - In either case, the left contains items non-bigger than the pivot (possibly including pivot values), the 
///       middle items equal to the pivot (and no other values) and the right items non-smaller than the pivot 
///       (possibly including pivot values).
///       <br/>
///     - Items in the middle segments have already been placed by the algorithm in their final position. Therefore,
///       the middle segment doesn't have to be sorted any further. Left and rights segments, instead, are not sorted.
///       <br/>
///     - So the algorithm calls recursively the quicksort on the left and right segments, to sort them.
///       <br/>
///     - When the recursion terminates, the entire list is sorted.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Worst-case Time Complexity is O(n^2) and Space Complexity is O(1), since sorting happens entirely in place,
///       by repeated swapping.
///       <br/>
///     - Expected Time Complexity heavily depends on the choice of <see cref="IShuffleStrategy"/> and 
///       <see cref="IThreeWayPartitionStrategy"/>, which in turns depends on the choice of 
///       <see cref="IPivotSelectionStrategy"/>.
///       <br/>
///     - For example, using <see cref="IdentityShuffleStrategy"/> and <see cref="StartIndexPivotSelectionStrategy"/>
///       with inputs already sorted in ascending order, makes quicksort selects the worst pivot at every iteration,
///       partitioning each window of the input list in the most unbalanced way (left segment empty and right segment
///       containing all items but the pivot), and making quicksort behaving quadratically.
///       <br/>
///     - The same happens when using <see cref="IdentityShuffleStrategy"/> and 
///       <see cref="EndIndexPivotSelectionStrategy"/>, with inputs already sorted in descending order.
///       <br/>
///     - A tipical setup, yielding good runtime results, is to use a RandomizedShuffleStrategy and a "smart" 3-way 
///       <see cref="IThreeWayPartitionStrategy"/>, placing all pivot values in the middle segment. That variation of
///       the quicksort has expected worst-case Time Complexity linearithmic, i.e. has O(n * log(n)) Time Complexity
///       with high probability, and behaves almost linearly with many real case input configurations.
///     </para>
/// </remarks>
public class RecursiveQuickSort : IInputMutatingSort
{
    private IShuffleStrategy ShuffleStrategy { get; }
    private IThreeWayPartitionStrategy PartitionStrategy { get; }

    /// <summary>
    /// Builds a sorter running quicksort with the provided <paramref name="partitionStrategy"/>.
    /// </summary>
    /// <param name="shuffleStrategy">
    /// The strategy used to shuffle the input list, before running the actual quicksort algorithm.
    /// </param>
    /// <param name="partitionStrategy">
    /// The strategy used to partition the sublist of the provided list within the provided start and end indices.
    /// </param>
    public RecursiveQuickSort(IShuffleStrategy shuffleStrategy, IThreeWayPartitionStrategy partitionStrategy)
    {
        ShuffleStrategy = shuffleStrategy;
        PartitionStrategy = partitionStrategy;
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the quicksort algorithm with the default comparer for <typeparamref name="T"/>, given by
    ///     <see cref="Comparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="RecursiveQuickSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list) where T : IComparable<T> =>
        Sort(list, Comparer<T>.Default);

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses the quicksort algorithm with the specified <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="RecursiveQuickSort"/>
    /// </remarks>
    public void Sort<T>(IList<T> list, IComparer<T> comparer)
    {
        ShuffleStrategy.Shuffle(list, 0, list.Count - 1);
        RecursiveSort(list, comparer, 0, list.Count - 1);
    }

    private void RecursiveSort<T>(IList<T> list, IComparer<T> comparer, int start, int end)
    {
        if (end - start <= 0) return;

        var (pivotLowIndex, pivotHighIndex) = PartitionStrategy.Partition(list, comparer, start, end);
        RecursiveSort(list, comparer, start, pivotLowIndex - 1);
        RecursiveSort(list, comparer, pivotHighIndex + 1, end);
    }
}
