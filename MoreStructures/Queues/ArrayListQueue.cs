namespace MoreStructures.Queues;

/// <summary>
/// A <see cref="IQueue{T}"/> implementation based on an array list of items.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IQueue{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - The advantages an disadvantages of using an array list over a linked list-based implementation are the same 
///       as the ones described in <see cref="Stacks.LinkedListStack{T}"/>.
///     </para>
/// </remarks>
public class ArrayListQueue<T> : Stacks.ArrayBasedDataStructure<T>, IQueue<T>
{
    private int StartIndex { get; set; } = 0;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Stored and updated in constant time at each <see cref="Enqueue(T)"/> and <see cref="Dequeue"/>.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count { get; private set; } = 0;

    /// <inheritdoc/>
    public ArrayListQueue(int capacity = DefaultInitialCapacity, double increasingFactor = DefaultIncreasingFactor)
        : base(capacity, increasingFactor)
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks and returns the item of the underlying array list at the first index set (which is 
    /// internally stored), if it exists.
    /// <br/>
    /// Throws a <see cref="InvalidOperationException"/> if the underlying array is empty.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Peek()
    {
        if (Count == 0)
            throw new InvalidOperationException($"Cannot {nameof(Peek)} on an empty queue.");
        return Items[IndexOfNthItem(0)]!;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// First, it retrieves the least recent item set into the underlying array list (whose index is internally 
    /// stored).
    /// <br/>
    /// Then, it reset the value at such index and decreases the <see cref="Count"/> by 1.
    /// <br/>
    /// Then, if the new <see cref="Count"/> is smaller than the current capacity by more than twice the 
    /// <see cref="Stacks.ArrayBasedDataStructure{T}.IncreasingFactor"/> compoundly 
    /// (i.e. capacity * increasingFactor^2), the underlying array list is resized to have a new capacity equal to the 
    /// old capacity times the increasing factor.
    /// <br/>
    /// Finally, it returns the retrieved item as result.
    /// <br/>
    /// Raises an <see cref="InvalidOperationException"/> if the array list is empty.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Dequeue()
    {
        if (Count == 0)
            throw new InvalidOperationException($"Cannot {nameof(Dequeue)} on an empty queue.");

        var index = IndexOfNthItem(0);
        var item = Items[index]!;
        Items[index] = default;
        StartIndex = (StartIndex + 1) % Items.Length;
        Count--;

        if (Count <= Items.Length / (IncreasingFactor * IncreasingFactor))
            ResizeItems(1.0 / IncreasingFactor);

        return item;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// If there is available room in the underlying array, the new <paramref name="item"/> is stored in the first 
    /// available location (applying circular arithmetic to indexes) and the <see cref="Count"/> is increased by 1.
    /// <br/>
    /// Otherwise the underlying array is resized by the 
    /// <see cref="Stacks.ArrayBasedDataStructure{T}.IncreasingFactor"/>, to accomodate the new 
    /// <paramref name="item"/>.
    /// <br/>
    /// Time and Space Complexity are O(1) if <see cref="Count"/> before insertion is strictly smaller than the current 
    /// capacity.
    /// <br/>
    /// Time and Space Complexity are O(n) if <see cref="Count"/> before insertion is equal to the current capacity.
    /// <br/>
    /// If the <see cref="Stacks.ArrayBasedDataStructure{T}.IncreasingFactor"/> is set to a sensible value (e.g. 2.0),
    /// the amortized cost over n insertions becomes O(1).
    /// </remarks>
    public void Enqueue(T item)
    {
        if (Count == Items.Length)
            ResizeItems(IncreasingFactor);

        Items[IndexOfNthItem(Count)] = item;
        Count++;
    }

    private int IndexOfNthItem(int nth) => (StartIndex + nth + Items.Length) % Items.Length;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// After resizing, it copies items appropriately to restore circular arithmetic of indexes which is now based on 
    /// a different modulo.
    /// <br/>
    /// If the size of the array has been increased, copy will happen from the front of the backing array to the front
    /// of its second half (introduced by the resizing).
    /// <br/>
    /// If the size of the array has been reduced, copy will simply scan the entire old array, remapping indexes into 
    /// the new array.
    /// </remarks>
    protected override void ResizeItems(double factor)
    {
        var oldSize = Items.Length;

        if (factor > 1)
        {
            base.ResizeItems(factor);

            for (var i = 0; i < StartIndex + Count - oldSize; i++)
            {
                var oldIndex = i % Items.Length;
                var newIndex = (oldSize + i) % Items.Length;
                Items[newIndex] = Items[oldIndex];
                Items[oldIndex] = default;
            }
        }
        else
        {
            var oldItems = Items;
            var newSize = (int)Math.Ceiling(oldItems.Length * factor);
            var newItems = new T?[newSize];

            for (var i = 0; i < Count; i++)
                newItems[(StartIndex + i) % newSize] = oldItems[(StartIndex + i) % oldSize];
        }
    }
}
