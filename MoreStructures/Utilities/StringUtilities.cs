
namespace MoreStructures.Utilities;

/// <summary>
/// Generic utilities and extensions for strings.
/// </summary>
public static class StringUtilities
{
    /// <summary>
    /// Returns the length of the longest prefix in common between the provided <see cref="IEnumerable{T}"/> of 
    /// <see cref="char"/>.
    /// </summary>
    /// <returns>
    /// An integer betwen 0 and the length of the shortest of the enumerabbles provided.
    /// </returns>
    public static int LongestPrefixInCommon(IEnumerable<char> enumerable1, IEnumerable<char> enumerable2)
    {
        var result = 0;
        var enumerator1 = enumerable1.GetEnumerator();
        var enumerator2 = enumerable2.GetEnumerator();

        var enumerator1MoveNext = enumerator1.MoveNext();
        var enumerator2MoveNext = enumerator2.MoveNext();
        while (enumerator1MoveNext && enumerator2MoveNext && enumerator1.Current == enumerator2.Current)
        {
            result++;
            enumerator1MoveNext = enumerator1.MoveNext();
            enumerator2MoveNext = enumerator2.MoveNext();
        }

        return result;
    }
}
