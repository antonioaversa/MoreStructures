namespace MoreStructures.PriorityQueues;

/// <summary>
/// A refinement of <see cref="HeapBasedPriorityQueue{T}"/> which supports <see cref="IUpdatablePriorityQueue{T}"/>
/// operations, such as retrieval and update of priorities and removal of items.
/// </summary>
/// <typeparam name="T"><inheritdoc/></typeparam>
/// <remarks>
/// In order to support updates and deletions, two additional data structures are introduced:
/// <br/>
/// - a <see cref="Dictionary{TKey, TValue}"/> Item2PT, mapping items <c>I</c> of type <typeparamref name="T"/> to 
///   <see cref="HeapBasedPriorityQueue{T}"/> instances, containing <see cref="PrioritizedItem{T}.PushTimestamp"/>
///   values of type <see cref="int"/>, of <see cref="PrioritizedItem{T}"/> instances containing <c>I</c>.
///   <br/>
/// - a <see cref="Dictionary{TKey, TValue}"/> PT2Idx from <see cref="PrioritizedItem{T}.PushTimestamp"/> values to
///   indexes of type <see cref="int"/>, into the backing list of the Binary Max Heap of this priority queue.
///   <br/>
/// <br/>
/// Every time a request to remove or update an item <c>I</c> from the priority queue is made, Item2PT is used to
/// retrieve all the <see cref="PrioritizedItem{T}.PushTimestamp"/> values of <see cref="PrioritizedItem{T}"/>
/// instances with item.
/// <br/>
/// Those push timestamps can be multiple because the same item can be added multiple times to the queue.
/// <br/>
/// The push timestamps are organized themselves in per-item priority queues, with the same priority as the items
/// in the main priority queue.
/// <br/>
/// This way, the push timestamp of highest priority for a given item can be peeked in constant time and extracted in 
/// logarithmic time.
/// <br/>
/// Once the timestamp of highest priority has been found, the corresponding index (if any) in the backing list of the 
/// Binary Max Heap of this priority queue can be found in constant time via the PT2Idx dictionary.
/// <br/>
/// Once the index is found, operations such as <see cref="Remove(T)"/> and <see cref="UpdatePriority(T, int)"/> can
/// be executed in logarithmic time, since restoring heap properties after the modification of a single item of the
/// heap requires a logarithmic number of sift down and/or sift up operations.
/// <br/>
/// <br/>
/// The two dictionaries are kept up-to-date by implementing the extensibility points provided by 
/// <see cref="HeapBasedPriorityQueue{T}"/>, after pushing, before popping and on items swapping.
/// </remarks> 
public class UpdatableHeapBasedPriorityQueue<T> : HeapBasedPriorityQueue<T>, IUpdatablePriorityQueue<T>
    where T : notnull
{
    private Dictionary<T, HeapBasedPriorityQueue<int>> ItemToPushTimestamps { get; } = new();
    private Dictionary<int, int> PushTimestampToIndex { get; } = new();

    /// <inheritdoc/>
    protected override void RaiseItemPushed()
    {
        var index = Items.Count - 1;
        var prioritizedItem = Items[index];
        PushTimestampToIndex[prioritizedItem.PushTimestamp] = index;
        if (!ItemToPushTimestamps.ContainsKey(prioritizedItem.Item))
            ItemToPushTimestamps[prioritizedItem.Item] = new HeapBasedPriorityQueue<int>();
        ItemToPushTimestamps[prioritizedItem.Item].Push(prioritizedItem.PushTimestamp, prioritizedItem.Priority);
    }

    /// <inheritdoc/>
    protected override void RaiseItemPopping()
    {
        PushTimestampToIndex[Items[^1].PushTimestamp] = 0;
        PushTimestampToIndex.Remove(Items[0].PushTimestamp);
    }

    /// <inheritdoc/>
    protected override void RaiseItemsSwapped(int index1, int index2)
    {
        PushTimestampToIndex[Items[index1].PushTimestamp] = index1;
        PushTimestampToIndex[Items[index2].PushTimestamp] = index2;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - First, the priority queue of push timestamps for the provided <paramref name="item"/> is retrieved from 
    ///       the dictionary of per-item queues of push timestamps.
    ///       <br/>
    ///     - If such a queue is not found, <paramref name="item"/> is not present in the main priority queue, and an 
    ///       empty sequence is returned.
    ///       <br/>
    ///     - Otherwise, the queue is iterated over, getting the index corresponding to each timestamp extracted from 
    ///       the queue (where such index exists).
    ///       <br/>
    ///     - The index is used to make a direct access to the corresponding <see cref="PrioritizedItem{T}"/> in the 
    ///       list backing the main priority queue. The priority is taken from 
    ///       <see cref="PrioritizedItem{T}.Priority"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Retrieving the priority queue of push timestamps from the dictionary of per-item priority queues is a 
    ///       O(1) operation.
    ///       <br/>
    ///     - Iterating such a priority queue requires duplicating the underlying data structure, which is a 
    ///       O(dup_factor) operation, where dup_factor is the average number of occurrences of an item in the data 
    ///       structure (1 means no duplicates, 2 means the item appears twice, etc.).
    ///       <br/>
    ///     - Retrieving the index from the push timestamp and the priority from the <see cref="PrioritizedItem{T}"/>
    ///       instance are both constant-time operations.
    ///       <br/>
    ///     - Therefore Time and Space Complexity are O(dup_factor).
    ///     </para>
    /// </remarks>
    public IEnumerable<int> GetPrioritiesOf(T item)
    {
        if (!ItemToPushTimestamps.TryGetValue(item, out var pushTimestamps))
            return Enumerable.Empty<int>();

        return 
            from pushTimestamp in pushTimestamps
            where PushTimestampToIndex.ContainsKey(pushTimestamp)
            let index = PushTimestampToIndex[pushTimestamp]
            select Items[index].Priority;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - It first removes the provided <paramref name="item"/> from the queue via <see cref="Remove(T)"/>.
    ///       <br/>
    ///     - Then, it pushes the same <paramref name="item"/> with <paramref name="newPriority"/> via 
    ///       <see cref="HeapBasedPriorityQueue{T}.Push(T, int)"/>.
    ///       <br/>
    ///     - Finally it returns the <see cref="PrioritizedItem{T}"/> removed in the first step.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Both <see cref="Remove(T)"/> and <see cref="HeapBasedPriorityQueue{T}.Push(T, int)"/> have logarithmic 
    ///       Time Complexity and constant Space Complexity.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n) + dup_factor) and Space Complexity is O(1), where dup_factor is 
    ///       the average number of occurrences of an item in the data structure (1 means no duplicates, 2 means the 
    ///       item appears twice, etc.).
    ///     </para>
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
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The priority queue of push timestamps for the provided <paramref name="item"/> is retrieved from the
    ///       dictionary of per-item queues of push timestamps.
    ///       <br/>
    ///     - If such a priority queue is not found, it means that <paramref name="item"/> is not present in the main
    ///       priority queue either. So, <see langword="null"/> is returned.
    ///       <br/>
    ///     - If the priority queue is found, push timestamps are popped from it until the root of the priority queue 
    ///       contains a valid timestamp ts, i.e. a timestamp present in the dictionary mapping timestamps to indexes.
    ///       <br/>
    ///     - If such a timestamp is not found, it means that that <paramref name="item"/> used to be present in the 
    ///       main priority, but it is not anymore. So, <see langword="null"/> is returned.
    ///       <br/>
    ///     - If such a timestamp is found, the backing list L for the main priority queue can be accessed via the 
    ///       index i, corresponding to the timestamp ts, simply by <c>L[i]</c>. <c>L[i]</c> represents the item to be
    ///       removed.
    ///       <br/>
    ///     - The priority of <c>L[i]</c> is set to the highest priority in the queue + 1 and the item is made sift up
    ///       to the root, due to its new priority being the highest in the heap.
    ///       <br/>
    ///     - Finally, the item, now at the root of the heap, is removed via a 
    ///       <see cref="HeapBasedPriorityQueue{T}.Pop"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Retrieving the priority queue associated with the <paramref name="item"/> is a O(1) operation.
    ///       <br/>
    ///     - Finding the right push timestamp may require a number of <see cref="HeapBasedPriorityQueue{T}.Pop"/>
    ///       proportional to the number of times the priority of <paramref name="item"/> has been changed.
    ///       <br/>
    ///     - In the worst case, such number is equal to the number of insertion of <paramref name="item"/>.
    ///       <br/>
    ///     - Changing the priority of the <see cref="PrioritizedItem{T}"/> to remove requires constant work.
    ///       <br/>
    ///     - Sifting it up to the root and then popping it are both logarithmic in time and constant in space.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n) + dup_factor) and Space Complexity is O(1), where dup_factor is 
    ///       the average number of occurrences of an item in the data structure (1 means no duplicates, 2 means the 
    ///       item appears twice, etc.).
    ///     </para>
    /// </remarks>
    public PrioritizedItem<T>? Remove(T item)
    {
        if (!ItemToPushTimestamps.TryGetValue(item, out var pushTimestamps))
            return null;

        int index = -1;
        while (pushTimestamps.Count > 0 && !PushTimestampToIndex.TryGetValue(pushTimestamps.Peek().Item, out index))
        {
            pushTimestamps.Pop();
        }

        if (pushTimestamps.Count == 0)
            return null;

        var oldItem = Items[index];

        // Because we only change priority, and both timestamp and index remain unchanged, there is no need to invoke
        // RaiseItemX methods.
        Items[index] = Items[index] with { Priority = Peek().Priority + 1 };
        SiftUp(index);
        Pop();

        return oldItem;
    }
}
