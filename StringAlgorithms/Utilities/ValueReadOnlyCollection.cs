using System.Collections.ObjectModel;

namespace StringAlgorithms.Utilities;

/// <summary>
/// A readonly immutable generic collection of non-null items which performs equality by value.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
/// <remarks>
/// Immutability can be guaranteed by cloning the provided enumerable and exposing a readonly view of it, but only 
/// if immutability of underlying <typeparamref name="T"/> is provided, for example, by using immutable records.
/// </remarks>
public class ValueReadOnlyCollection<T> : ReadOnlyCollection<T>
    where T : notnull
{
    /// <summary>
    /// Creates value readonly collection out of the provided enumerable, and independent from it.
    /// </summary>
    /// <param name="enumerable">The enumerable to be used to build the readonly collection.</param>
    public ValueReadOnlyCollection(IEnumerable<T> enumerable) 
        : base(new List<T>(enumerable))
    {
    }

    /// <summary>
    /// <inheritdoc/> Equality is calculated by value, i.e. on the collections items directly.
    /// </summary>
    /// <param name="obj"><inheritdoc cref="object.Equals(object?)" path="/param[@name='obj']"/></param>
    /// <returns>
    /// True if the specified object is equal to the current collection by value; otherwise, false.
    /// </returns>
    public override bool Equals(object? obj) =>
        obj is ValueReadOnlyCollection<T> other && this.SequenceEqual(other);

    /// <summary>
    /// <inheritdoc/> The hash code is calculated by value, as an aggregate of the hash codes of its items.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => 
        Items.Aggregate(0.GetHashCode(), (acc, item) => acc ^ item.GetHashCode());

    /// <summary>
    /// <inheritdoc/> Format: "[v1, v2, ...]".
    /// </summary>
    public override string ToString() => 
        $"[{string.Join(", ", Items)}]";

    /// <summary>
    /// Compare the two provided value read-only collections for equality by value.
    /// </summary>
    /// <param name="left">The first term of comparison.</param>
    /// <param name="right">The second term of comparison.</param>
    /// <returns>True if the two collections are equal by their items, false otherwise.</returns>
    public static bool operator ==(ValueReadOnlyCollection<T> left, ValueReadOnlyCollection<T> right) =>
        Equals(left, right);

    /// <summary>
    /// Compare the two provided value read-only collections for inequality by value.
    /// </summary>
    /// <param name="left">The first term of comparison.</param>
    /// <param name="right">The second term of comparison.</param>
    /// <returns>True if the two collections are different by their items, false otherwise.</returns>
    public static bool operator !=(ValueReadOnlyCollection<T> left, ValueReadOnlyCollection<T> right) =>
        !Equals(left, right);
}
