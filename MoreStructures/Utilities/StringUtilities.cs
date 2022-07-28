
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
    /// An integer betwen 0 and the length of the shortest of the enumerables provided.
    /// </returns>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The algorithm iterates over two enumerators in parallel, one enumerator per enumerable provided.
    ///       <br/>
    ///     - It keeps running the enumerators until one of the two is over, or the current items of the two 
    ///       enumerators are different.
    ///       <br/>
    ///     - It keeps a counter of the number of chars found equal from the beginning, which is returned as result.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Enumerator creation, moving to next item and accessing the current item are all constant-time operations.
    ///       <br/>
    ///     - If the two enumerables have length l1 and l2 respectively, there are at most max(l1, l2) iterations.
    ///       <br/>
    ///     - The current item of each of the enumerable has a constant size.
    ///       <br/>
    ///     - If l1 and l2 are O(n), Time Complexity is O(n) and Space Complexity is O(1).
    ///     </para>
    /// </remarks>
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
