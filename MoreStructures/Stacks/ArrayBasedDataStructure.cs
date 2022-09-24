namespace MoreStructures.Stacks;

/// <summary>
/// Exposes properties shared by data structures based on a backing array of items of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items, the data structure is composed of.</typeparam>
public abstract class ArrayBasedDataStructure<T>
{
    /// <summary>
    /// The default initial size of the array backing the data structure.
    /// </summary>
    /// <remarks>
    /// In an array initialized with capacity x, up to x insertions can be done in constant time, with no need for 
    /// array resizing.
    /// </remarks>
    public const int DefaultInitialCapacity = 16;

    /// <summary>
    /// The default value for <see cref="IncreasingFactor"/>.
    /// </summary>
    public const double DefaultIncreasingFactor = 2;

    /// <summary>
    /// The array of items, backing this data structure.
    /// </summary>
    protected T?[] Items { get; set; }

    /// <summary>
    /// The multiplicative factor used to resize the underlying array, every time it gets full.
    /// </summary>
    /// <remarks>
    /// Required to be bigger than 1.0.
    /// </remarks>
    public double IncreasingFactor { get; }

    /// <summary>
    /// Initializes the data structure with an array list of initial capacity equals to the provided 
    /// <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">
    /// The initial capacity of the backing array. If not specified, <see cref="DefaultInitialCapacity"/> is used.
    /// </param>
    /// <param name="increasingFactor">
    ///     <inheritdoc cref="IncreasingFactor" path="/summary"/>
    /// </param>
    protected ArrayBasedDataStructure(
        int capacity = DefaultInitialCapacity, double increasingFactor = DefaultIncreasingFactor)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Must be positive.");

        if (increasingFactor <= 1)
            throw new ArgumentOutOfRangeException(nameof(increasingFactor), "Must be strictly bigger than 1.");

        Items = new T[capacity];
        IncreasingFactor = increasingFactor;
    }

    /// <summary>
    /// Resizes the <see cref="Items"/> array, applying the provided <paramref name="factor"/> to its length.
    /// </summary>
    /// <param name="factor">The multiplicative factor to be applied to the length of the array.</param>
    protected virtual void ResizeItems(double factor)
    {
        var oldItems = Items;
        var newSize = (int)Math.Ceiling(oldItems.Length * factor);
        Array.Resize(ref oldItems, newSize);
        Items = oldItems;
    }
}
