﻿using System.Collections.Concurrent;

namespace MoreStructures.Utilities;

/// <summary>
/// An implementation of <see cref="IComparer{T}"/> for <see cref="string"/>, which compares string taking into account
/// the fact that the string may contain (at any index, not necessarily at then end) a special char, called terminator,
/// which has to be considered smaller than any other char.
/// </summary>
public class StringIncludingTerminatorComparer : IComparer<string>
{
    /// <summary>
    /// The character acting as terminator, and which has to be considered smaller than any other char.
    /// </summary>
    public char Terminator { get; }

    private StringIncludingTerminatorComparer(char terminator)
    {
        Terminator = terminator;
    }

    private static readonly ConcurrentDictionary<char, StringIncludingTerminatorComparer> _instances = new();

    /// <summary>
    /// Builds a <see cref="StringIncludingTerminatorComparer"/> with the provided <paramref name="terminator"/>. 
    /// Caches instances.
    /// </summary>
    /// <param name="terminator"><inheritdoc cref="Terminator" path="/summary"/></param>
    /// <returns>
    /// An instance of <see cref="StringIncludingTerminatorComparer"/>, new or previously created and cached.
    /// </returns>
    public static StringIncludingTerminatorComparer Build(char terminator)
    {
        return _instances.GetOrAdd(terminator, t => new StringIncludingTerminatorComparer(t));
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     Special rules applied by <see cref="StringIncludingTerminatorComparer"/>:
    ///     <br/>
    ///     - If either string is null or empty, the standard <see cref="string.Compare(string?, string?)"/> is used.
    ///       <br/>
    ///     - If one string starts with the terminator, and the other doesn't, the one which does is smaller.
    ///       <br/>
    ///     - If none of the cases above applies, <see cref="string.Compare(string?, string?)"/> is used on the 
    ///       substring starting from index 1 of each of the strings <paramref name="x"/> and <paramref name="y"/>.
    /// </remarks>
    public int Compare(string? x, string? y)
    {
        if (x == null || y == null || x.Length == 0 || y.Length == 0)
            return string.Compare(x, y);

        if (x[0] == Terminator && y[0] != Terminator)
            return -1;
        
        if (x[0] != Terminator && y[0] == Terminator)
            return 1;

        var firstCharDifference = x[0] - y[0];
        if (firstCharDifference != 0)
            return firstCharDifference;

        return Compare(x[1..], y[1..]);
    }
}
