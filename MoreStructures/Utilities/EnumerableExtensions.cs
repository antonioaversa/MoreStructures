using System.Collections;

namespace MoreStructures.Utilities;

/// <summary>
/// Extension methods for all <see cref="IEnumerable{T}"/> concretions.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Optimized version of <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/>, which runs in constant time
    /// on <paramref name="source"/> of type <see cref="string"/>, <see cref="IDictionary"/>, <see cref="IList{T}"/>
    /// and <see cref="IList"/>, and calls <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> for any 
    /// <paramref name="source"/> which cannot be assigned to either of these types.
    /// </summary>
    /// <typeparam name="TSource">
    ///     <inheritdoc cref="Enumerable.Count{TSource}(IEnumerable{TSource})" path="/typeparam[@name='TSource']"/>
    /// </typeparam>
    /// <param name="source">
    ///     <inheritdoc cref="Enumerable.Count{TSource}(IEnumerable{TSource})" path="/param[@name='source']"/>
    /// </param>
    /// <returns>
    ///     <inheritdoc cref="Enumerable.Count{TSource}(IEnumerable{TSource})" path="/returns"/>
    /// </returns>
    public static int CountO1<TSource>(this IEnumerable<TSource> source)
    {
        if (source is string str)
            return str.Length;
        if (source is IDictionary nonGenericDict)
            return nonGenericDict.Count;
        if (source is IList<TSource> genericList)
            return genericList.Count;
        if (source is IList nonGenericList)
            return nonGenericList.Count;
        return source.Count();
    }

    /// <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)" path="//*[not(self::summary)]"/>
    /// <summary>
    /// Optimized version of <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>, which runs in 
    /// constant time on <paramref name="source"/> of type <see cref="string"/>, <see cref="IList{T}"/>
    /// and <see cref="IList"/>, and calls <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/> 
    /// for any <paramref name="source"/> which cannot be assigned to either of these types.
    /// </summary>
    public static TSource ElementAtO1<TSource>(this IEnumerable<TSource> source, int index)
    {
        if (source is string str)
        {
            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return (TSource)(str[index] as object);
        }

        if (source is IList<TSource> genericList)
        {
            if (index < 0 || index >= genericList.Count)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return genericList[index];
        }

        if (source is IList nonGenericList)
        {
            if (index < 0 || index >= nonGenericList.Count)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return (TSource)nonGenericList[index]!;
        }

        return source.ElementAt(index);
    }

    /// <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)" path="//*[not(self::summary)]"/>
    /// <summary>
    /// Optimized version of <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, Index)"/>, which runs in 
    /// constant time on <paramref name="source"/> of type <see cref="string"/>, <see cref="IList{T}"/>
    /// and <see cref="IList"/>, and calls <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/> 
    /// for any <paramref name="source"/> which cannot be assigned to either of these types.
    /// </summary>
    public static TSource ElementAtO1<TSource>(this IEnumerable<TSource> source, Index index)
    {
        if (source is string str)
        {
            var indexValue = index.GetOffset(str.Length);
            if (indexValue < 0 || indexValue >= str.Length)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return (TSource)(str[index] as object);
        }

        if (source is IList<TSource> genericList)
        {
            var indexValue = index.GetOffset(genericList.Count);
            if (indexValue < 0 || indexValue >= genericList.Count)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return genericList[index];
        }

        if (source is IList nonGenericList)
        {
            var indexValue = index.GetOffset(nonGenericList.Count);
            if (indexValue < 0 || indexValue >= nonGenericList.Count)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return (TSource)nonGenericList[index]!;
        }

        return source.ElementAt(index);
    }

    /// <summary>
    /// Optimized version of <see cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>, which 
    /// runs in constant time on <paramref name="source"/> of type <see cref="string"/>, <see cref="IList{T}"/>
    /// and <see cref="IList"/>, and calls 
    /// <see cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/> 
    /// for any <paramref name="source"/> which cannot be assigned to either of these types.
    /// </summary>
    /// <typeparam name="TSource">
    ///     <inheritdoc cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/typeparam[@name='TSource']"/>
    /// </typeparam>
    /// <param name="source">
    ///     <inheritdoc cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/param[@name='source']"/>
    /// </param>
    /// <param name="index">
    ///     <inheritdoc cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/param[@name='index']"/>
    /// </param>
    /// <returns>
    ///     <inheritdoc cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/returns"/>
    /// </returns>
    public static TSource? ElementAtO1OrDefault<TSource>(this IEnumerable<TSource> source, int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException($"Invalid {nameof(index)}: {index}");

        if (source is string str)
            return index >= 0 && index < str.Length ? (TSource)(str[index] as object) : default;
        if (source is IList<TSource> genericList)
            return index >= 0 && index < genericList.Count ? genericList[index] : default;
        if (source is IList nonGenericList)
            return index >= 0 && index < nonGenericList.Count ? (TSource)nonGenericList[index]! : default;
        return source.ElementAtOrDefault(index);
    }

    /// <summary>
    /// Eagerly enumerates the first <paramref name="count"/> items of <paramref name="source"/>, returning an
    /// <see cref="IList{T}"/> of them and the reminder, as a lazily evaluated <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to split into two pieces.</param>
    /// <param name="count">The number of items of <paramref name="source"/> to eagerly evaluate and return.</param>
    /// <returns>
    /// A couple of an <see cref="IList{T}"/> instance and an <see cref="IEnumerable{T}"/> instance.
    /// </returns>
    public static (IList<TSource> firstNItems, IEnumerable<TSource> reminder) EnumerateExactlyFirst<TSource>(
        this IEnumerable<TSource> source, int count)
    {
        var (first, reminder) = source.EnumerateAtMostFirst(count);

        if (first.Count < count)
            throw new ArgumentException($"Sequence doesn't contain {count} elements.", nameof(source));

        return (first, reminder);
    }

    /// <summary>
    /// Eagerly enumerates the first <paramref name="count"/> items of <paramref name="source"/>, or less if there 
    /// aren't enough, returning an <see cref="IList{T}"/> of them and the reminder, as a lazily evaluated 
    /// <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to split into two pieces.</param>
    /// <param name="count">
    /// The number of items of <paramref name="source"/> to eagerly evaluate and return (at most).
    /// </param>
    /// <returns>
    /// A couple of an <see cref="IList{T}"/> instance and an <see cref="IEnumerable{T}"/> instance.
    /// </returns>
    public static (IList<TSource> firstNItems, IEnumerable<TSource> reminder) EnumerateAtMostFirst<TSource>(
        this IEnumerable<TSource> source, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException($"Invalid {nameof(count)}: {count}");

        var enumerator = source.GetEnumerator();

        var first = new List<TSource> { };
        for (var i = 0; i < count; i++)
        {
            if (!enumerator.MoveNext())
                break;

            first.Add(enumerator.Current);
        }

        IEnumerable<TSource> ToEnumerable()
        {
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        return (first, ToEnumerable());
    }
}
