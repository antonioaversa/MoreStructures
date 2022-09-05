using System.Collections;

namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on an unsorted list of its items.
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
public class ArrayListBasedPriorityQueue<T> : IUpdatablePriorityQueue<T>
    where T : notnull
{
    private List<ItemAndPriority<T>> Items { get; }

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
    public ArrayListBasedPriorityQueue(List<ItemAndPriority<T>> items)
    {
        Items = items;
    }

    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    public ArrayListBasedPriorityQueue() : this(new List<ItemAndPriority<T>>())
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
    public ArrayListBasedPriorityQueue(IEnumerable<ItemAndPriority<T>> items) : this(items.ToList())
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
    public ItemAndPriority<T> Peek() => Items.MaxBy(itemAndPriority => itemAndPriority.Priority);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the index with the highest priority.
    /// <br/>
    /// Then removes the item with such index from the underlying list and returns it as result.
    /// <br/>
    /// Time Complexity is O(n). Space Complexity is O(1).
    /// </remarks> 
    public ItemAndPriority<T> Pop()
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
        Items.Add(new(item, priority));
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the index of the item equals to the provided 
    /// <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the two items of type
    /// <typeparamref name="T"/>) and having highest priority.
    /// <br/>
    /// If such an index is found, it is returned. Otherwise <see langword="null"/> is returned.
    /// <br/>
    /// Time Complexity is O(n * Teq) and Space Complexity is O(Seq), where Teq and Seq are the time and space cost of
    /// comparing two items of type <typeparamref name="T"/>.
    /// </remarks> 
    public int? GetHighestPriorityOf(T item)
    {
        var indexedItemAndPriority = MoreLinq.MoreEnumerable
            .Index(Items)
            .Where(itemAndPriority => Equals(itemAndPriority.Value.Item, item))
            .DefaultIfEmpty(KeyValuePair.Create(-1, new ItemAndPriority<T>()))
            .MaxBy(itemAndPriority => itemAndPriority.Value.Priority);

        return indexedItemAndPriority.Key >= 0 ? indexedItemAndPriority.Value.Priority : null;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for <see cref="ItemAndPriority{T}"/> having an item equals to the 
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
    public IPriorityQueue<int> GetPrioritiesOf(T item)
    {
        var occurrencesOfItem =
            from itemAndPriority in Items
            where Equals(itemAndPriority.Item, item)
            select new ItemAndPriority<int>(itemAndPriority.Priority, itemAndPriority.Priority);

        return new ArrayListBasedPriorityQueue<int>(occurrencesOfItem);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the index of the item equals to the provided 
    /// <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the two items of type
    /// <typeparamref name="T"/>).
    /// <br/>
    /// Then replaces the item at such index with the one passed as input and its <paramref name="newPriority"/>.
    /// <br/>
    /// Finally returns the priority of the replaced item.
    /// <br/>
    /// Time Complexity is O(n * Teq) and Space Complexity is O(Seq), where Teq and Seq are the time and space cost of
    /// comparing two items of type <typeparamref name="T"/>.
    /// </remarks> 
    public int UpdatePriority(T item, int newPriority)
    {
        var indexedItemAndPriority = MoreLinq.MoreEnumerable
            .Index(Items)
            .First(itemAndPriority => Equals(itemAndPriority.Value.Item, item));

        Items[indexedItemAndPriority.Key] = new(item, newPriority);
        return indexedItemAndPriority.Value.Priority;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying list looking for the index of the item equals to the provided 
    /// <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the two items of type
    /// <typeparamref name="T"/>).
    /// <br/>
    /// If no such index is found, nothing is changed and <see langword="null"/> is returned. 
    /// <br/>
    /// Otherwise the item at such position is removed from the list and returned as result.
    /// <br/>
    /// Time Complexity is O(n * Teq) and Space Complexity is O(Seq), where Teq and Seq are the time and space cost of
    /// comparing two items of type <typeparamref name="T"/>.
    /// </remarks>
    public ItemAndPriority<T>? Remove(T item)
    {
        var indexedItemAndPriority = MoreLinq.MoreEnumerable
            .Index(Items)
            .Where(itemAndPriority => Equals(itemAndPriority.Value.Item, item))
            .DefaultIfEmpty(KeyValuePair.Create(-1, new ItemAndPriority<T>()))
            .MaxBy(itemAndPriority => itemAndPriority.Value.Priority);

        if (indexedItemAndPriority.Key < 0)
            return null;

        Items.RemoveAt(indexedItemAndPriority.Key);
        return indexedItemAndPriority.Value;
    }
}
