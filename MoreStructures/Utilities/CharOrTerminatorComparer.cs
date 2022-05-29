using System.Collections.Concurrent;

namespace MoreStructures.Utilities;

/// <summary>
/// An implementation of <see cref="IComparer{T}"/> for <see cref="char"/>, which compares chars taking into account
/// the fact that the char may be a special char, called terminator, which has to be considered smaller than any other 
/// char.
/// </summary>
public class CharOrTerminatorComparer : IComparer<char>
{
    /// <summary>
    /// The character acting as terminator, and which has to be considered smaller than any other char.
    /// </summary>
    public char Terminator { get; }

    private CharOrTerminatorComparer(char terminator)
    { 
        Terminator = terminator;
    }

    private static readonly ConcurrentDictionary<char, CharOrTerminatorComparer> _instances = new();

    /// <summary>
    /// Builds a <see cref="CharOrTerminatorComparer"/> with the provided <paramref name="terminator"/>. Caches 
    /// instances.
    /// </summary>
    /// <param name="terminator"><inheritdoc cref="Terminator" path="/summary"/></param>
    /// <returns>An instance of <see cref="CharOrTerminatorComparer"/>, new or previously created and cached.</returns>
    public static CharOrTerminatorComparer Build(char terminator)
    {
        return _instances.GetOrAdd(terminator, t => new CharOrTerminatorComparer(t));
    }

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

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Two <see cref="CharOrTerminatorComparer"/> are equal if they have the same <see cref="Terminator"/>.
    /// </remarks>
    public override bool Equals(object? obj) => 
        obj is CharOrTerminatorComparer other && other.Terminator == Terminator;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// The <see cref="GetHashCode"/> is based on <see cref="Terminator"/> hash.
    /// </remarks>
    public override int GetHashCode() =>
        Terminator.GetHashCode();
}
