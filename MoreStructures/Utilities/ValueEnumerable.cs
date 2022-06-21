using System.Collections;

namespace MoreStructures.Utilities;

/// <summary>
/// A <see cref="IValueEnumerable{T}"/> implementation, wrapping a generic <see cref="IEnumerable{T}"/>.
/// </summary>
/// <typeparam name="T">The type of objects of the wrapped enumerable.</typeparam>
/// <remarks>
///     Useful to preserve equality by value in records and other value structures which contain enumerable objects.
///     <br/>
///     It doesn't cache nor enumerate the underlying <see cref="Enumerable"/> unless <see cref="GetEnumerator"/> or a 
///     method requiring <see cref="Enumerable"/> items is called.
/// </remarks>
public class ValueEnumerable<T> : IValueEnumerable<T>
{
    private IEnumerable<T> Enumerable { get; }

    /// <summary>
    /// Builds a <see cref="ValueEnumerable{T}"/> around the provided <paramref name="enumerable"/>.
    /// </summary>
    /// <param name="enumerable">The enumerable to wrap.</param>
    /// <remarks>
    /// Time and Space Complexity is O(1), as this constructor doesn't enumerate <paramref name="enumerable"/>.
    /// </remarks>
    public ValueEnumerable(IEnumerable<T> enumerable)
    {
        Enumerable = enumerable is ValueEnumerable<T> { Enumerable: var underlyingEnumerable}
            ? underlyingEnumerable
            : enumerable;
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => 
        Enumerable.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => 
        (Enumerable as IEnumerable).GetEnumerator();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     In the specific case, equality is based on the 
    ///     <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/> of the wrapped
    ///     <see cref="IEnumerable{T}"/> objects.
    ///     <br/>
    ///     Therefore, Time Complexity is O(n * Te) and Space Complexity is O(Se), where n is the number of elements of
    ///     the longest <see cref="Enumerable"/> and Te/Se are the time and space costs of 
    ///     <see cref="object.Equals(object?)"/> of two <typeparamref name="T"/> instances.
    /// </remarks>
    public override bool Equals(object? obj) => 
        obj is ValueEnumerable<T> other && Enumerable.SequenceEqual(other.Enumerable);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     In the specific case, the hash is calculated as an aggregation of the hash codes of the items of the 
    ///     wrapped <see cref="Enumerable"/> object.
    ///     <br/>
    ///     Therefore, Time Complexity is O(n * Te) and Space Complexity is O(Se), where n is the number of elements of
    ///     the longest <see cref="Enumerable"/> and Te/Se are the time and space costs of 
    ///     <see cref="object.GetHashCode()"/> of a <typeparamref name="T"/> instance.
    /// </remarks>
    public override int GetHashCode() =>
        Enumerable.Aggregate(0.GetHashCode(), (acc, item) => acc ^ (item?.GetHashCode() ?? 0));

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     In the specific case, the string calculation is delegated to the wrapped <see cref="IEnumerable{T}"/> 
    ///     object.
    ///     <br/>
    ///     Therefore, Time and Space Complexity are the ones of the specific implementation of 
    ///     <see cref="object.ToString()"/> of the underlying <see cref="Enumerable"/>.
    /// </remarks>
    public override string ToString() =>
        Enumerable.ToString()!;
}
