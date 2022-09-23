namespace MoreStructures.Stacks;

/// <summary>
/// A <see cref="IStack{T}"/> implementation based on an array list of items.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IStack{T}"/></typeparam>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to an implementation based on a linked list, such as <see cref="LinkedListStack{T}"/>, it has a 
///       lower memory footprint and a lower cost of memory management, due to the fact that a single object made of a
///       contiguous area in memory is used, to store all items.
///       <br/>
///     - In particular, no need for per-item additional space is required, as it happens for "next" references and
///       objects overhead in linked list-based solutions.
///       <br/>
///     - On the flip side, the requirement for a potentially large contiguous chunk of memory in the heap may produce
///       <see cref="OutOfMemoryException"/> when trying to instantiate or resize stacks of large capacity, especially
///       when the memory is heavily fragmented.
///       <br/>
///     - Moreover, each insert which happens when the underlying array has been fully occupied requires work 
///       proportional to the current size of the stack, since the underlying array cannot be extended without 
///       instantiating a larger array and performing a full copy from the old to the new one.
///       <br/>
///     - That means that insertion cost won't be O(1) all the time, but only in average, as amortized complexity over
///       n operations (assuming that <see cref="IncreasingFactor"/> is left to its default value 
///       <see cref="DefaultIncreasingFactor"/>, or chosen sensibly). 
///       <br/>
///     - That can be a significant drawback in realtime systems where insertion cost has to be highly predictable and 
///       also very low.
///     </para>
/// </remarks>
public class ArrayListStack<T> : IStack<T>
{
    /// <summary>
    /// The default initial size of the array backing the stack.
    /// </summary>
    /// <remarks>
    /// In an array initialized with capacity x, up to x executions of <see cref="Push(T)"/> can be done in constant 
    /// time, with no need for array resizing.
    /// </remarks>
    public const int DefaultInitialCapacity = 16;

    /// <summary>
    /// The default value for <see cref="IncreasingFactor"/>.
    /// </summary>
    public const double DefaultIncreasingFactor = 2;

    private T?[] Items { get; set; }

    /// <summary>
    /// The multiplicative factor used to resize the underlying array, every time it gets full.
    /// </summary>
    /// <remarks>
    /// Required to be bigger than 1.0.
    /// </remarks>
    public double IncreasingFactor { get; }

    /// <summary>
    /// Initializes a stack with an array list of initial capacity equals to the provided <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">
    /// The initial capacity of the backing array. If not specified, <see cref="DefaultInitialCapacity"/> is used.
    /// </param>
    /// <param name="increasingFactor">
    ///     <inheritdoc cref="IncreasingFactor" path="/summary"/>
    /// </param>
    public ArrayListStack(int capacity = DefaultInitialCapacity, double increasingFactor = DefaultIncreasingFactor)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Must be positive.");

        if (increasingFactor <= 1)
            throw new ArgumentOutOfRangeException(nameof(increasingFactor), "Must be strictly bigger than 1.");

        Items = new T[capacity];
        IncreasingFactor = increasingFactor;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Stored and updated in constant time at each <see cref="Push(T)"/> and <see cref="Pop"/>.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count { get; private set; } = 0;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Checks and returns the item of the underlying array list at the last index set (which is 
    /// <see cref="Count"/> - 1), if it exists.
    /// <br/>
    /// Throws a <see cref="InvalidOperationException"/> if the underlying array is empty.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Peek()
    {
        if (Count == 0)
            throw new InvalidOperationException($"Cannot {nameof(Peek)} on an empty stack.");
        return Items[Count - 1]!;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// First, it retrieves the item of the last item set in the underlying array list (which is the one at index 
    /// <see cref="Count"/> - 1).
    /// <br/>
    /// Then, it reset the value of the last item and decreases the <see cref="Count"/> by 1.
    /// <br/>
    /// Then, if the new <see cref="Count"/> is smaller than the current capacity by more than twice the 
    /// <see cref="IncreasingFactor"/> compoundly (i.e. capacity * increasingFactor^2), the underlying array list is
    /// resized to have a new capacity equal to the old capacity times the increasing factor.
    /// <br/>
    /// Finally, it returns the retrieved item as result.
    /// <br/>
    /// Raises an <see cref="InvalidOperationException"/> if the array list is empty.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public T Pop()
    {
        if (Count == 0)
            throw new InvalidOperationException($"Cannot {nameof(Pop)} on an empty stack.");
        var item = Items[Count - 1]!;
        Items[Count - 1] = default;
        Count--;

        if (Count <= Items.Length / (IncreasingFactor * IncreasingFactor))
            ResizeItems(1.0 / IncreasingFactor);

        return item;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// If there is available room in the underlying array, the new <paramref name="item"/> is stored in the first 
    /// available location and the <see cref="Count"/> is increased by 1.
    /// <br/>
    /// Otherwise the underlying array is resized by the <see cref="IncreasingFactor"/>, as many times as needed to
    /// accomodate the new <paramref name="item"/>.
    /// <br/>
    /// Time and Space Complexity are O(1) if <see cref="Count"/> before insertion is strictly smaller than the current 
    /// capacity.
    /// <br/>
    /// Time and Space Complexity are O(n) if <see cref="Count"/> before insertion is equal to the current capacity.
    /// <br/>
    /// If the <see cref="IncreasingFactor"/> is set to a sensible value (e.g. 2.0), the amortized cost over n 
    /// insertions becomes O(1).
    /// </remarks>
    public void Push(T item)
    {
        if (Count == Items.Length)
            ResizeItems(IncreasingFactor);

        Items[Count] = item;
        Count++;
    }

    private void ResizeItems(double factor)
    {
        var oldItems = Items;
        var newSize = oldItems.Length;
        
        do
        {
            newSize = (int)Math.Ceiling(newSize * factor);
        }
        while (Count > newSize);

        Array.Resize(ref oldItems, newSize);
        Items = oldItems;
    }
}
