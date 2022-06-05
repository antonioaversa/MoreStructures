﻿using System.Collections;

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

    /// <summary>
    /// Optimized version of <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>, which runs in 
    /// constant time on <paramref name="source"/> of type <see cref="string"/>, <see cref="IList{T}"/>
    /// and <see cref="IList"/>, and calls <see cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/> 
    /// for any <paramref name="source"/> which cannot be assigned to either of these types.
    /// </summary>
    /// <typeparam name="TSource">
    ///     <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/typeparam[@name='TSource']"/>
    /// </typeparam>
    /// <param name="source">
    ///     <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/param[@name='source']"/>
    /// </param>
    /// <param name="index">
    ///     <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/param[@name='index']"/>
    /// </param>
    /// <returns>
    ///     <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)" 
    ///         path="/returns"/>
    /// </returns>
    public static TSource ElementAtO1<TSource>(this IEnumerable<TSource> source, int index)
    {
        if (source is string str)
        {
            if (index < 0 || index >= str.Length)
                throw new IndexOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return (TSource)(str[index] as object);
        }

        if (source is IList<TSource> genericList)
        {
            if (index < 0 || index >= genericList.Count)
                throw new IndexOutOfRangeException($"Invalid {nameof(index)}: {index}");
            return genericList[index];
        }

        if (source is IList nonGenericList)
        {
            if (index < 0 || index >= nonGenericList.Count)
                throw new IndexOutOfRangeException($"Invalid {nameof(index)}: {index}");
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
            throw new IndexOutOfRangeException($"Invalid {nameof(index)}: {index}");

        if (source is string str)
            return index >= 0 && index < str.Length ? (TSource)(str[index] as object) : default;
        if (source is IList<TSource> genericList)
            return index >= 0 && index < genericList.Count ? genericList[index] : default;
        if (source is IList nonGenericList)
            return index >= 0 && index < nonGenericList.Count ? (TSource)nonGenericList[index]! : default;
        return source.ElementAtOrDefault(index);
    }
}
