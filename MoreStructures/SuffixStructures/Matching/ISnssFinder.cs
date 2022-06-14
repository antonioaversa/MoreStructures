namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// Any algorithm finding the Shortest Non-shared Substring (Snss) between two strings.
/// </summary>
public interface ISnssFinder
{
    /// <summary>
    /// Returns any substring of <paramref name="text1"/> which is not present in <paramref name="text2"/> and has 
    /// minimal length. Returns <see langword="null"/> if there is no substring of <paramref name="text1"/> which is
    /// not a substring of <paramref name="text2"/>, i.e. if the two string coincide.
    /// </summary>
    /// <param name="text1">The sequence of chars of the first text.</param>
    /// <param name="text2">The sequence of chars of the second text.</param>
    /// <returns>A string, containing the substring of <paramref name="text1"/>.</returns>
    string? Find(IEnumerable<char> text1, IEnumerable<char> text2);
}
