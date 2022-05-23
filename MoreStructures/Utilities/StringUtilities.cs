
namespace MoreStructures.Utilities;

/// <summary>
/// Generic utilities and extensions for strings.
/// </summary>
public static class StringUtilities
{
    /// <summary>
    /// Returns the length of the longest prefix in common between the provided strings.
    /// </summary>
    /// <returns>
    /// An integer betwen 0 and the length of the shortest of the strings provided.
    /// </returns>
    public static int LongestPrefixInCommon(string s1, string s2)
    {
        var shortestLength = Math.Min(s1.Length, s2.Length);
        for (int i = 0; i < shortestLength; i++)
        {
            if (s1[i] != s2[i])
                return i;
        }
        return shortestLength;
    }
}
