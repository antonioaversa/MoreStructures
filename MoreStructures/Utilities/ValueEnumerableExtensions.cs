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
    /// </remarks>
    public static IValueEnumerable<T> AsValue<T>(this IEnumerable<T> enumerable) =>
        new ValueEnumerable<T>(enumerable);
}
