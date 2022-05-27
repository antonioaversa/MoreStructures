using MoreLinq;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform;

/// <summary>
/// Exposes text rotation functionalities related to the Burrows-Wheeler Transform of strings.
/// </summary>
public static class BWTCyclicRotations
{
    /// <summary>
    /// Returns all cyclic rotations of the provided <see cref="TextWithTerminator"/>, sorted in ascending order and
    /// taking into account that <see cref="TextWithTerminator.Terminator"/> is to be considered smaller than any other
    /// char in the text.
    /// </summary>
    /// <param name="text">The text, to calculate cyclic rotations of.</param>
    /// <returns>A list of string, each one containing a cyclic rotation of the provided text.</returns>
    /// <example>
    /// Code: 
    /// <code>
    /// CalculateCyclicRotations(new("mississippi"))
    /// </code>
    /// 
    /// Result:
    /// <code>
    /// {
    ///     "$mississippi",
    ///     "i$mississipp",
    ///     "ippi$mississ",
    ///     "issippi$miss",
    ///     "ississippi$m",
    ///     "mississippi$",
    ///     "pi$mississip",
    ///     "ppi$mississi",
    ///     "sippi$missis",
    ///     "sissippi$mis",
    ///     "ssippi$missi",
    ///     "ssissippi$mi",
    /// }
    /// </code>
    /// </example>
    public static IList<string> CalculateCyclicRotations(TextWithTerminator text) =>
        Enumerable
            .Range(0, text.Length)
            .Select(i => new string(text.Skip(i).Take(text.Length - i).Concat(text.Take(i)).ToArray()))
            .OrderBy(i => i, new StringIncludingTerminatorComparer(text.Terminator))
            .ToArray();
}
