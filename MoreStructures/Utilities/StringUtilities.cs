
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
    ///     - The current item of each enumerable has a constant size.
    ///       <br/>
    ///     - If l1 and l2 are O(n), Time Complexity is O(n) and Space Complexity is O(1).
    ///     </para>
    /// </remarks>
    public static int LongestCommonPrefix(IEnumerable<char> enumerable1, IEnumerable<char> enumerable2)
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

    /// <summary>
    /// Returns the length of the longest prefix in common between the substrings of the provided <see cref="string"/>
    /// instances, starting at the provided indexes.
    /// </summary>
    /// <returns>
    /// An integer betwen 0 and the length of the shortest of the strings provided, minus related starting index.
    /// </returns>
    /// <remarks>
    ///     <para id="advantages">
    ///     ADVANTAGES AND DISADVANTAGES
    ///     <br/>
    ///     - While <c>LongestCommonPrefix(s1, i1, s2, i2)</c> is conceptually equivalent to 
    ///       <c>LongestCommonPrefix(s1.Skip(i1), s2.Skip(i2))</c>, making use of the general implementation of LCP
    ///       given by <see cref="LongestCommonPrefix(IEnumerable{char}, IEnumerable{char})"/>, this specific 
    ///       implementation, with <see cref="string"/> instances and <see cref="int"/> starting indexes, has been 
    ///       provided.
    ///       <br/>
    ///     - The reason behind the decision of providing a specialized implementation is that the LINQ method 
    ///       <see cref="Enumerable.Skip{TSource}(IEnumerable{TSource}, int)"/> is O(n), rather than O(1). 
    ///       <br/>
    ///     - Moreover, the performance "suboptimality" cannot be overcome by using ranges on <see cref="string"/>: 
    ///       <c>LongestCommonPrefix(s1[i1..], s2[i2..])</c> builds two new strings, <c>s1[i1..]</c> and 
    ///       <c>s2[i2..]</c>, by copying the part of the underlying arrays of s1 and s2 starting at index i1 and i2
    ///       respectively. This results in <c>s1[i1..]</c> and <c>s2[i2..]</c> being O(n) both in time and space.
    ///       <br/>
    ///     - Check https://stackoverflow.com/questions/6742923/, and the answer from Eric Lippert, for further 
    ///       information about the performance of substring extraction from a <see cref="string"/> instance.
    ///     </para>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The algorithm iterates over two strings in parallel, each from the provided index.
    ///       <br/>
    ///     - It keeps checking chars from the two strings until one of the two strings is over, or the current char 
    ///       of the two strings are different.
    ///       <br/>
    ///     - It keeps a counter of the number of chars found equal from the beginning, which is returned as result.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Direct char accessm on a string, as well as checking its length, are constant-time operations.
    ///       <br/>
    ///     - If the two strings have length l1 and l2 respectively, and starting indexes are i1 and i2 respectively,
    ///       there are at most max(l1 - i1, l2 - i2) iterations.
    ///       <br/>
    ///     - If l1, i1, l2 and i2 are O(n), Time Complexity is O(n) and Space Complexity is O(1).
    ///     </para>
    /// </remarks>
    public static int LongestCommonPrefix(string string1, int index1, string string2, int index2)
    {
        if (index1 < 0 || index1 >= string1.Length)
            throw new ArgumentOutOfRangeException(
                nameof(index1), $"Must be non-negative and smaller than {nameof(string1)} length.");
        if (index2 < 0 || index2 >= string2.Length)
            throw new ArgumentOutOfRangeException(
                nameof(index2), $"Must be non-negative and smaller than {nameof(string2)} length.");

        var result = 0;
        while (index1 < string1.Length && index2 < string2.Length && string1[index1] == string2[index2])
        {
            index1++;
            index2++;
            result++;
        }

        return result;
    }
}
