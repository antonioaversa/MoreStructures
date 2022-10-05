using System.Collections;

namespace MoreStructures.PriorityQueues.LinkedList;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> implementation based on a <b>sorted linked list</b> of its items. 
/// On top of basic operations it also supports <see cref="IUpdatablePriorityQueue{T}"/>, 
/// <see cref="IPeekKthPriorityQueue{T}"/> and <see cref="IMergeablePriorityQueue{T, TPQTarget}"/>.
/// operations.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IPriorityQueue{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     This represents one of the simplest implementations of a Priority Queue.
///     <br/>
///     It provides O(1) count and amortized extraction, at the cost of all other operations, which are O(n).
///     <br/>
///     Runtime behavior is specular to <see cref="ArrayList.ArrayListPriorityQueue{T}"/>, which can perform an 
///     insertion in constant time, but requires a linear amount of time to extract a value.
///     <br/>
///     If extraction performance is the only highly critical operation, to the point that a constant time performance 
///     is the only acceptable runtime, and not even the logarithmic time extraction of a tree-based solution can be 
///     applied, this implementation may be the best choice.
///     <br/>
///     An advantage over <see cref="ArrayList.ArrayListPriorityQueue{T}"/> is that this solution can also offer
///     constant-time merging and still similar simplicity of implementation.
///     <br/>
///     When data insertion performance is also a concern, or the main concern, a more balanced solution in terms of
///     complexity of its operations should be preferred.
///     </para>
/// </remarks>
public sealed class SortedLinkedListPriorityQueue<T> 
    : IUpdatablePriorityQueue<T>, IPeekKthPriorityQueue<T>, IMergeablePriorityQueue<T, SortedLinkedListPriorityQueue<T>>
    where T : notnull
{
    /// <summary>
    /// A non-negative, zero-based, monotonically strictly increasing counter, incremented at every insertion into this 
    /// data structure by a <see cref="Push(T, int)"/>.
    /// </summary>
    private int CurrentPushTimestamp { get; set; } = 0;

    /// <summary>
    /// The <see cref="LinkedList{T}"/> of <see cref="PrioritizedItem{T}"/> backing the linked list heap.
    /// </summary>
    private LinkedList<PrioritizedItem<T>> Items { get; set; }

    private LinkedListNode<PrioritizedItem<T>>? FindOccurrenceWithHighestPrioritySmallerThan(PrioritizedItem<T> item)
    {
        LinkedListNode<PrioritizedItem<T>>? current = Items.First;
        if (current == null)
            return null;

        LinkedListNode<PrioritizedItem<T>>? previous = null;
        while (current != null && current.Value.CompareTo(item) > 0)
        {
            previous = current;
            current = current.Next;
        }

        return previous;
    }

    private LinkedListNode<PrioritizedItem<T>>? FindOccurrenceWithHighestPriorityOf(T item)
    {
        for (var current = Items.First; current != null; current = current.Next)
            if (Equals(current.Value.Item, item))
                return current;

        return null;
    }

    /// <summary>
    /// Builds a priority queue using the provided linked list as <b>direct</b> backing structure.
    /// </summary>
    /// <param name="items">The <see cref="LinkedList{T}"/> structure to be used as backing structure.</param>
    /// <remarks>
    /// The provided linked list is not copied over: it is used directly as backing structure for the queue.
    /// <br/>
    /// Therefore, operations mutating the queue such as <see cref="Push(T, int)"/> will alter the content of the 
    /// <paramref name="items"/> linked list.
    /// </remarks>
    public SortedLinkedListPriorityQueue(LinkedList<PrioritizedItem<T>> items)
    {
        Items = items;
    }

    /// <summary>
    /// Builds an empty priority queue.
    /// </summary>
    public SortedLinkedListPriorityQueue() : this(new List<PrioritizedItem<T>>())
    {
    }

    /// <summary>
    /// Builds a priority queue using the provided items to populate its backing structure.
    /// </summary>
    /// <param name="items">The items to be added to the queue.</param>
    /// <remarks>
    /// The provided sequence is enumerated, sorted in descending order of priority (taking into account 
    /// <see cref="PrioritizedItem{T}.PushTimestamp"/> to break ties), then copied over onto a dedicated linked list.
    /// <br/>
    /// So, the provided sequence is not used directly as backing structure for the queue.
    /// <br/>
    /// Therefore, operations mutating the queue won't alter the provided <paramref name="items"/> sequence.
    /// </remarks>
    public SortedLinkedListPriorityQueue(IEnumerable<PrioritizedItem<T>> items) 
        : this(new LinkedList<PrioritizedItem<T>>(items.OrderByDescending(i => i)))
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
    /// Returns the items in the underlying linked list, which is already sorted in descending order of priority. 
    /// <br/>
    /// Time Complexity is O(n) (when fully enumerated). Space Complexity is O(1).
    /// </remarks> 
    public IEnumerator<T> GetEnumerator() => Items.Select(n => n.Item).GetEnumerator();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="GetEnumerator"/>
    /// </remarks>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Returns the first item in the underlying linked list, raising <see cref="InvalidOperationException"/> when the 
    /// list is empty.
    /// <br/>
    /// Time Complexity is O(1). Space Complexity is O(1).
    /// </remarks> 
    public PrioritizedItem<T> Peek()
    {
        if (Items.Count == 0)
            throw new InvalidOperationException($"Can't ${nameof(Peek)} on an empty queue.");

        return Items.First();
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <see cref="Peek"/> the item with the highest priority from the underlying linked lists, which is the first item
    /// in the chain.
    /// <br/>
    /// Then removes such an the item from the front of the linked list and returns it as result.
    /// <br/>
    /// Time Complexity is O(1). Space Complexity is O(1).
    /// </remarks> 
    public PrioritizedItem<T> Pop()
    {
        if (Items.Count == 0)
            throw new InvalidOperationException($"Can't ${nameof(Pop)} on an empty queue.");

        var result = Peek();
        Items.RemoveFirst();
        return result;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Finds the item I in the linked list with the highest priority, smaller than the priority of the provided 
    /// <paramref name="priority"/>.
    /// <br/>
    /// Then adds a new <see cref="LinkedListNode{T}"/> instance, having as value a new 
    /// <see cref="PrioritizedItem{T}"/> with the provided <paramref name="item"/> and <paramref name="priority"/>,
    /// just before the item I.
    /// <br/>
    /// If such a I doesn't exist, prepend the new <see cref="LinkedListNode{T}"/> at the front of the linked list.
    /// <br/>
    /// Time Complexity is O(n). Space Complexity is O(1).
    /// <br/>
    /// Remark: while the linked list is sorted by priority, binary search it's not possible, due to lack of direct 
    /// random access support.
    /// </remarks> 
    public void Push(T item, int priority)
    {
        var prioritizedItem = new PrioritizedItem<T>(item, priority, CurrentPushTimestamp++);
        var highestSmallerNode = FindOccurrenceWithHighestPrioritySmallerThan(prioritizedItem);
        if (highestSmallerNode == null)
            Items.AddFirst(prioritizedItem);
        else
            Items.AddAfter(highestSmallerNode, prioritizedItem);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Linearly scans the underlying linked list, looking for <see cref="PrioritizedItem{T}"/> having an item equals 
    /// to the provided <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the 
    /// two items of type <typeparamref name="T"/>).
    /// <br/>
    /// It then selects all priorities found for <paramref name="item"/> and builds a 
    /// <see cref="SortedLinkedListPriorityQueue{T}"/> of <see cref="int"/> values out of them.
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
        var priorityQueue = new SortedLinkedListPriorityQueue<int>();
        foreach (var priority in priorities)
            priorityQueue.Push(priority, priority);
        return priorityQueue;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Removes the occurrence of <paramref name="item"/> with highest priority from the underlying linked list (the 
    /// first encountered while navigating the list).
    /// <br/>
    /// If no occurrence of <paramref name="item"/> is found, an <see cref="InvalidOperationException"/> is thrown.
    /// <br/>
    /// Then pushes the provided <paramref name="item"/> with the given <paramref name="newPriority"/>.
    /// <br/>
    /// Both removal and push operations require linked list traversal and update.
    /// <br/>
    /// Therefore, Time Complexity is O(n). Space Complexity is O(1).
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
    /// Linearly scans the underlying linked list, looking for the first node with the item equals to the provided 
    /// <paramref name="item"/> (<see cref="object.Equals(object?, object?)"/> is used to compare the two items of type
    /// <typeparamref name="T"/>).
    /// <br/>
    /// If multiple occurrences of <paramref name="item"/> are present with the same highest priority, the one with the
    /// lowest <see cref="PrioritizedItem{T}.PushTimestamp"/> is considered of higher priority, and comes before any
    /// other in the list. That guarantees <b>stability</b>.
    /// <br/>
    /// If no such node is found, nothing is changed and <see langword="null"/> is returned. 
    /// <br/>
    /// Otherwise the node is removed from the list and returned as result.
    /// <br/>
    /// Time Complexity is O(n * Teq) and Space Complexity is O(Seq), where Teq and Seq are the time and space cost of
    /// comparing two items of type <typeparamref name="T"/>.
    /// </remarks>
    public PrioritizedItem<T>? Remove(T item)
    {
        var itemNode = FindOccurrenceWithHighestPriorityOf(item);

        if (itemNode == null)
            return null;

        Items.Remove(itemNode);
        return itemNode.Value;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Takes advantage of the fact that the underlying linked list of items is already sorted in descending order by
    /// priority, and returns the <paramref name="k"/>-th item of the list.
    /// <br/>
    /// Time Complexity is O(k). Space Complexity is O(1).
    /// <br/>
    /// The <paramref name="k"/>-th item of the list cannot be accessed in constant time, because linked lists don't
    /// support direct random access.
    /// </remarks>
    public PrioritizedItem<T>? PeekKth(int k)
    {
        if (k < 0)
            throw new ArgumentException("Must be non-negative.", nameof(k));
        if (k >= Items.Count)
            return null;
        if (k == 0)
            return Peek();

        return Items.ElementAt(k);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Does 2-way merging on the underlying linked lists, which are already sorted in descending order of priority.
    /// <br/>
    /// Time and Space Complexity are O(n + m), where n and m are the number of items in this queue and in the target, 
    /// respectively.
    /// </remarks>
    public void MergeFrom(SortedLinkedListPriorityQueue<T> targetPriorityQueue)
    {
        var first = Items.First;
        var second = targetPriorityQueue.Items.First;
        var merged = new LinkedList<PrioritizedItem<T>>();
        while (first != null || second != null)
        {
            PrioritizedItem<T> prioritizedItem;
            if (second == null || (first != null && first.Value.Priority >= second.Value.Priority))
            {
                prioritizedItem = new(first!.Value.Item, first.Value.Priority, CurrentPushTimestamp++);
                first = first.Next;
            }
            else
            {
                prioritizedItem = new(second.Value.Item, second.Value.Priority, CurrentPushTimestamp++);
                second = second.Next;
            }
            merged.AddLast(prioritizedItem);
        }

        Items = merged;
        targetPriorityQueue.Clear();
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Just clears the underlying linked list.
    /// <br/>
    /// Time and Space Complexity is O(1).
    /// </remarks>
    public void Clear()
    {
        Items.Clear();
    }
}
