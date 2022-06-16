using System.Collections.ObjectModel;

namespace MoreStructures.Utilities;

/// <summary>
/// A readonly immutable generic dictionary of non-null keys and values which performs equality by value.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
/// <remarks>
/// Immutability can be guaranteed by cloning the provided dictionary and exposing a readonly view of it, but only 
/// if immutability of underlying <typeparamref name="TKey"/> and <typeparamref name="TValue"/> is provided, for 
/// example, by using immutable records.
/// </remarks>
public class ValueReadOnlyDictionary<TKey, TValue> : ReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Creates value readonly dictionary out of the provided dictionary, and independent from it.
    /// </summary>
    /// <param name="dictionary">The dictionary to be used to build the readonly dictionary.</param>
    public ValueReadOnlyDictionary(IDictionary<TKey, TValue> dictionary) 
        : base(new Dictionary<TKey, TValue>(dictionary))
    {
    }

    /// <summary>
    /// Creates value readonly dictionary out of the provided entries, and independent from the provided
    /// enumerable of them.
    /// </summary>
    /// <param name="entries">The enumerable of entries to be used to build the readonly dictionary.</param>

    public ValueReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> entries)
        : base(new Dictionary<TKey, TValue>(entries))
    { 
    }

    /// <summary>
    /// <inheritdoc/> Equality is calculated by value, i.e. on the dictionary key-value pairs directly.
    /// </summary>
    /// <param name="obj"><inheritdoc cref="object.Equals(object?)" path="/param[@name='obj']"/></param>
    /// <returns>
    /// True if the specified object is equal to the current dictionary by value; otherwise, false.
    /// Two dictionaries are considered equal by value if they have the same set of keys and the value associated
    /// with each of the key by the two dictionaries are equal with each other.
    /// </returns>
    public override bool Equals(object? obj) => 
        obj is ValueReadOnlyDictionary<TKey, TValue> other &&
        Count == other.Count &&
        Keys.ToHashSet().SetEquals(other.Keys.ToHashSet()) &&
        Keys.All(k => Equals(this[k], other[k]));

    /// <summary>
    /// <inheritdoc/> The hash code is calculated by value, as an aggregate of the hash codes of its key value 
    /// pairs.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() =>
        this.Select(kvp => kvp.GetHashCode()).Aggregate(0.GetHashCode(), (acc, v) => acc ^ v);

    /// <summary>
    /// <inheritdoc/> Format: "{[k1] = v1, [k2] = v2, ...}".
    /// </summary>
    public override string ToString() => 
        $"{{{string.Join(", ", this.Select(kvp => $"[{kvp.Key}] = {kvp.Value}"))}}}";

    /// <summary>
    /// Compare the two provided value read-only dictionaries for equality by value.
    /// </summary>
    /// <param name="left">The first term of comparison.</param>
    /// <param name="right">The second term of comparison.</param>
    /// <returns>True if the two dictionaries are equal by their items, false otherwise.</returns>
    public static bool operator ==(
        ValueReadOnlyDictionary<TKey, TValue> left, ValueReadOnlyDictionary<TKey, TValue> right) =>
        Equals(left, right);

    /// <summary>
    /// Compare the two provided value read-only dictionaries for inequality by value.
    /// </summary>
    /// <param name="left">The first term of comparison.</param>
    /// <param name="right">The second term of comparison.</param>
    /// <returns>True if the two dictionaries are different by their items, false otherwise.</returns>
    public static bool operator !=(
        ValueReadOnlyDictionary<TKey, TValue> left, ValueReadOnlyDictionary<TKey, TValue> right) =>
        !Equals(left, right);
}
