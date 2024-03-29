﻿using System.Collections.ObjectModel;

namespace MoreStructures.Utilities;

/// <summary>
/// A readonly immutable generic collection of non-null items which performs equality by value.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
/// <remarks>
/// Immutability can be guaranteed by cloning the provided enumerable and exposing a readonly view of it, but only 
/// if immutability of underlying <typeparamref name="T"/> is provided, for example, by using immutable records.
/// </remarks>
public class ValueReadOnlyCollection<T> : ReadOnlyCollection<T>, IEquatable<ValueReadOnlyCollection<T>>
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

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Equality is calculated by value, i.e. on the collections items directly.
    /// </summary>
    public override bool Equals(object? obj) =>
        obj is ValueReadOnlyCollection<T> other && Equals(other);

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Equality is calculated by value, i.e. on the collections items directly.
    /// </summary>
    public virtual bool Equals(ValueReadOnlyCollection<T>? other) => other is not null && this.SequenceEqual(other);

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     The hash code is calculated by value, as an aggregate of the hash codes of its items.
    /// </summary>
    public override int GetHashCode() => 
        Items.Aggregate(0.GetHashCode(), (acc, item) => acc ^ item.GetHashCode());

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Format: "[v1, v2, ...]".
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
