using System.Collections.Generic;

namespace MoreStructures.Utilities;

/// <summary>
/// An implementation of <see cref="IComparer{T}"/> for <see cref="string"/>, which compares string taking into account
/// the fact that the string may contain (at any index, not necessarily at then end) a special char, called terminator,
/// which has to be considered smaller than any other char.
/// </summary>
public class StringIncludingTerminatorComparer : IComparer<string>
{
    private readonly char _terminator;

    /// <summary>
    /// Builds a <see cref="StringIncludingTerminatorComparer"/>.
    /// <inheritdoc cref="StringIncludingTerminatorComparer"/>
    /// </summary>
    /// <param name="terminator">The char to be considered as terminator.</param>
    public StringIncludingTerminatorComparer(char terminator)
    {
        _terminator = terminator;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="x"><inheritdoc path="/param[@name='x']"/></param>
    /// <param name="y"><inheritdoc path="/param[@name='y']"/></param>
    /// <remarks>
    /// Special rules applied by <see cref="StringIncludingTerminatorComparer"/>:
    /// <list type="bullet">
    ///     <item>
    ///     If either string is null or empty, the standard <see cref="string.Compare(string?, string?)"/> is used.
    ///     </item>
    ///     <item>
    ///     If one string starts with the terminator, and the other doesn't, the one which does is smaller.
    ///     </item>
    ///     <item>
    ///     If none of the cases above applies, <see cref="string.Compare(string?, string?)"/> is used on the 
    ///     substring starting from index 1 of each of the strings <paramref name="x"/> and <paramref name="y"/>.
    ///     </item>
    /// </list>
    /// </remarks>
    /// <returns><inheritdoc/></returns>
    public int Compare(string? x, string? y)
    {
        if (x == null || y == null || x.Length == 0 || y.Length == 0)
            return string.Compare(x, y);

        if (x[0] == _terminator && y[0] != _terminator)
            return -1;
        
        if (x[0] != _terminator && y[0] == _terminator)
            return 1;

        var firstCharDifference = x[0] - y[0];
        if (firstCharDifference != 0)
            return firstCharDifference;

        return Compare(x[1..], y[1..]);
    }
}
