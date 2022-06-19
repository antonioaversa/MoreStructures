namespace MoreStructures.Utilities;

/// <summary>
/// Extension methods for <see cref="IValueEnumerable{T}"/>.
/// </summary>
public static class ValueEnumerableExtensions
{
    /// <summary>
    /// Wraps the provided <paramref name="enumerable"/> into a <see cref="IValueEnumerable{T}"/>, which uses
    /// <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/> to check
    /// for equality.
    /// </summary>
    /// <typeparam name="T">The type of objects of <paramref name="enumerable"/>.</typeparam>
    /// <param name="enumerable">The enumerable to wrap.</param>
    /// <returns>A <see cref="IValueEnumerable{T}"/> wrapping the provided <see cref="IEnumerable{T}"/>.</returns>
    /// <remarks>
    /// Useful to preserve equality by value in records and other value structures which contain enumerable objects.
    /// <br/>
    /// Passing a <see cref="string"/> as <paramref name="enumerable"/> will result in the instantiation of a 
    /// specialized concretion of <see cref="IValueEnumerable{T}"/> which handles strings specifically:
    /// <see cref="StringValueEnumerable"/>.
    /// <br/>
    /// Time and Space Complexity are O(1), as this method doesn't iterate over <paramref name="enumerable"/>.
    /// </remarks>
    public static IValueEnumerable<T> AsValue<T>(this IEnumerable<T> enumerable) =>
        enumerable is string str
        ? (IValueEnumerable<T>)new StringValueEnumerable(str)
        : new ValueEnumerable<T>(enumerable);
}
