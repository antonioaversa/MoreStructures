using System.Collections;

namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on an unsorted list of its items. On top of basic 
/// operations it also supports <see cref="IUpdatablePriorityQueue{T}"/> and <see cref="IPeekKthPriorityQueue{T}"/>
/// operations.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IPriorityQueue{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     This represents one of the simplest implementations of a Priority Queue.
///     <br/>
///     It provides O(1) count and amortized insertion, at the cost of all other operations, which are O(n).
///     <br/>
///     If insertion performance is the only highly critical operation, to the point that a constant time performance 
///     is the only acceptable runtime, and not even the logarithmic time insertion of a tree-based solution can be 
///     applied, this implementation may be the best choice.
///     <br/>
///     When data extraction performance is also a concern, or the main concern, a more balanced solution in terms of
///     complexity of its operations should be preferred.
///     <br/>
///     This implementation can also be useful as a benchmark baseline in tests, when comparing against more complex 
///     solutions.
///     </para>
/// </remarks>
public class ArrayListBasedPriorityQueue<T> : IUpdatablePriorityQueue<T>, IPeekKthPriorityQueue<T>
    where T : notnull
{
    private int _currentPushTimestamp = 0;
    private List<PrioritizedItem<T>> Items { get; }

    private KeyValuePair<int, PrioritizedItem<T>> FindHighestPriorityOccurrence(T item)
    {
        return MoreLinq.MoreEnumerable
            .Index(Items)
            .Where(kvp => Equals(kvp.Value.Item, item))
            .DefaultIfEmpty(KeyValuePair.Create(-1, new PrioritizedItem<T>()))
            .MaxBy(kvp => kvp.Value);
    }

    /// <summary>
    /// Builds a priority queue using the provided list as <b>direct</b> backing structure.
    /// </summary>
    /// <param name="items">The <see cref="List{T}"/> structure to be used as backing structure.</param>
    /// <remarks>
    /// The provided list is not copied over: it is used directly as backing structure for the queue.
    /// <br/>
    /// Therefore, operations mutating the queue such as <see cref="Push(T, int)"/> will alter the content of the 
    /// <paramref name="items"/> list.
    /// </remarks>
    public ArrayListBasedPriorityQueue(List<PrioritizedItem<T>> items)
    {
        Items = items;
    }

    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    public ArrayListBasedPriorityQueue() : this(new List<PrioritizedItem<T>>())
    {
    }

    /// <summary>
    /// Builds a priority queue using the provided items to populate its backing structure.
    /// </summary>
    /// <param name="items">The items to be added to the queue.</param>
    /// <remarks>
    /// The provided sequence is enumerated and copied over onto a dedicated list: it is not used directly as backing 
    /// structure for the queue.
    /// <br/>
    /// Therefore, operations mutating the queue won't alter the provided <paramref name="items"/> sequence.
    /// </remarks>
    public ArrayListBasedPriorityQueue(IEnumerable<PrioritizedItem<T>> items) : this(items.ToList())
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Calls <see cref="Count"/> on the underlying list. 
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count => Items.Count;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Sorts the underlying list in descending order of priority and selects the items. 
    /// <br/>
    /// Time Complexity is O(n * log(n)) (when fully enumerated). Space Complexity is O(n), as required by sorting.
    /// </remarks> 
    public IEnumerator<T> GetEnumerator() => Items
        .OrderByDescending(itemAndPriority => itemAndPriority.Priority)
        .Select(itemAndPriority => itemAndPriority.Item)
        .GetEnumerator();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="GetEnumerator"/>
    /// </remarks>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the highest priority. 
    /// <br/>
    /// Time Complexity is O(n). Space Complexity is O(1).
    /// </remarks> 
    public PrioritizedItem<T> Peek() => Items.MaxBy(itemAndPriority => itemAndPriority.Priority);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the index with the highest priority.
    /// <br/>
    /// Then removes the item with such index from the underlying list and returns it as result.
    /// <br/>
    /// Time Complexity is O(n). Space Complexity is O(1).
    /// </remarks> 
    public PrioritizedItem<T> Pop()
    {
        if (Items.Count == 0)
            throw new InvalidOperationException($"Can't ${nameof(Pop)} on an empty queue.");

        var maxIndex = 0;
        for (var i = 1; i < Items.Count; i++)
            if (Items[maxIndex].Priority < Items[i].Priority)
                maxIndex = i;

        var result = Items[maxIndex];
        Items.RemoveAt(maxIndex);
        return result;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Appends the provided <paramref name="item"/> with its <paramref name="priority"/> to the end of the underlying
    /// list.
    /// <br/>
    /// Time Complexity is O(n). Space Complexity is O(1) (amortized over multiple executions of 
    /// <see cref="Push(T, int)"/>).
    /// </remarks> 
    public void Push(T item, int priority)
    {
        Items.Add(new(item, priority, _currentPushTimestamp++));
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for <see cref="PrioritizedItem{T}"/> having an item equals to the 
    /// provided <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the two 
    /// items of type <typeparamref name="T"/>).
    /// <br/>
    /// It then selects all priorities found for <paramref name="item"/> and builds a 
    /// <see cref="ArrayListBasedPriorityQueue{T}"/> of <see cref="int"/> values out of them.
    /// <br/>
    /// Such a priority queue is returned as result.
    /// <br/>
    /// Time Complexity is O(n * Teq) and Space Complexity is O(Seq), where Teq and Seq are the time and space cost of
    /// comparing two items of type <typeparamref name="T"/>.
    /// </remarks> 
    public IEnumerable<int> GetPrioritiesOf(T item)
    {
        var priorities = Items
            .Where(prioritizedItem => Equals(prioritizedItem.Item, item))
            .Select(prioritizedItem => prioritizedItem.Priority);
        var priorityQueue = new ArrayListBasedPriorityQueue<int>();
        foreach (var priority in priorities)
            priorityQueue.Push(priority, priority);
        return priorityQueue;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the index of the item equals to the provided 
    /// <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the two items of type
    /// <typeparamref name="T"/>) <b>with the highest priority</b>.
    /// <br/>
    /// If multiple occurrences of <paramref name="item"/> are present with the same highest priority, the one with the
    /// lowest <see cref="PrioritizedItem{T}.PushTimestamp"/> is selected, to guarantee <b>stability</b>.
    /// <br/>
    /// If no occurrence of <paramref name="item"/> is found, a <see cref="InvalidOperationException"/> is raised.
    /// <br/>
    /// Then replaces the <see cref="PrioritizedItem{T}"/> at such index with a new one having same 
    /// <see cref="PrioritizedItem{T}.Item"/> and <see cref="PrioritizedItem{T}.PushTimestamp"/>, but 
    /// <see cref="PrioritizedItem{T}.Priority"/> set to <paramref name="newPriority"/>.
    /// <br/>
    /// Finally returns the previously stored <see cref="PrioritizedItem{T}"/> at that index.
    /// <br/>
    /// Time Complexity is O(n * Teq) and Space Complexity is O(Seq), where Teq and Seq are the time and space cost of
    /// comparing two items of type <typeparamref name="T"/>.
    /// </remarks> 
    public PrioritizedItem<T> UpdatePriority(T item, int newPriority)
    {
        var oldItem = Remove(item);
        if (oldItem == null)
            throw new InvalidOperationException("The specified item is not in the queue.");
        Push(item, newPriority);
        return oldItem.Value;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the index of the item equals to the provided 
    /// <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the two items of type
    /// <typeparamref name="T"/>).
    /// <br/>
    /// If multiple occurrences of <paramref name="item"/> are present with the same highest priority, the one with the
    /// lowest <see cref="PrioritizedItem{T}.PushTimestamp"/> is selected, to guarantee <b>stability</b>.
    /// <br/>
    /// If no such index is found, nothing is changed and <see langword="null"/> is returned. 
    /// <br/>
    /// Otherwise the item at such position is removed from the list and returned as result.
    /// <br/>
    /// Time Complexity is O(n * Teq) and Space Complexity is O(Seq), where Teq and Seq are the time and space cost of
    /// comparing two items of type <typeparamref name="T"/>.
    /// </remarks>
    public PrioritizedItem<T>? Remove(T item)
    {
        var indexAndPrioritizedItem = FindHighestPriorityOccurrence(item);

        if (indexAndPrioritizedItem.Key < 0)
            return null;

        Items.RemoveAt(indexAndPrioritizedItem.Key);
        return indexAndPrioritizedItem.Value;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Uses the Quick Select algorithm to find the <paramref name="k"/>-th largest element by 
    /// <see cref="PrioritizedItem{T}.Priority"/> in the underlying list.
    /// <br/>
    /// Because Quick Select requires at least partial in-place sorting, the entire content of the underlying list is
    /// first copied into a temporary list, which is passed as target to the Quick Select procedure.
    /// <br/>
    /// Selected pivot is always at the end of the range of indexes in which selection is happening.
    /// <br/>
    /// So If input is already sorted in ascending order, Time Complexity is O(n^2), whereas in the average case Time
    /// Complexity is O(n). Space Complexity is always O(n).
    /// </remarks>
    public PrioritizedItem<T>? PeekKth(int k)
    {
        if (k < 0) 
            throw new ArgumentException("Must be non-negative.", nameof(k));
        if (k >= Items.Count)
            return null;
        if (k == 0)
            return Peek();

        var list = Items.ToList();
        var index = QuickFind(list, k, 0, list.Count - 1);
        return index >= 0 ? list[index] : null;
    }

    private static int QuickFind(List<PrioritizedItem<T>> list, int k, int start, int end)
    {
        if (start > end) return -1;
        if (start == end) return start;

        int currentPivotIndex = Partition(list, start, end);

        if (currentPivotIndex == k)
            return currentPivotIndex;
        if (currentPivotIndex > k)
            return QuickFind(list, k, start, currentPivotIndex - 1);
        return QuickFind(list, k, currentPivotIndex + 1, end);
    }

    private static int Partition(List<PrioritizedItem<T>> list, int start, int end)
    {
        var pivotValue = list[end];
        int currentPivotIndex = start - 1;
        for (var j = start; j < end; j++) // Start included, end excluded
        {
            if (list[j].CompareTo(pivotValue) >= 0)
            {
                currentPivotIndex++;
                (list[currentPivotIndex], list[j]) = (list[j], list[currentPivotIndex]);
            }
        }

        currentPivotIndex++;
        (list[currentPivotIndex], list[end]) = (list[end], list[currentPivotIndex]);
        return currentPivotIndex;
    }
}
