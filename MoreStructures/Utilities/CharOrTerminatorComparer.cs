namespace MoreStructures.Utilities;

/// <summary>
/// An implementation of <see cref="IComparer{T}"/> for <see cref="char"/>, which compares chars taking into account
/// the fact that the char may be a special char, called terminator, which has to be considered smaller than any other 
/// char.
/// </summary>
public record CharOrTerminatorComparer(char Terminator) : IComparer<char>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="x"><inheritdoc path="/param[@name='x']"/></param>
    /// <param name="y"><inheritdoc path="/param[@name='y']"/></param>
    /// <remarks>
    /// Special rules applied by <see cref="CharOrTerminatorComparer"/>:
    /// <list type="bullet">
    ///     <item>
    ///     If one char is the terminator, and the other isn't, the one which is the terminator is smaller.
    ///     </item>
    ///     <item>
    ///     If none of the cases above applies, <see cref="char.CompareTo(char)"/> is used on <paramref name="x"/> and 
    ///     <paramref name="y"/>.
    ///     </item>
    /// </list>
    /// </remarks>
    /// <returns><inheritdoc/></returns>
    public int Compare(char x, char y)
    {
        if (x == Terminator && y != Terminator)
            return -1;
        if (x != Terminator && y == Terminator)
            return 1;
        return x.CompareTo(y);
    }
}
