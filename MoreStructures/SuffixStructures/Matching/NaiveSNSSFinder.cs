namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// A <see cref="ISNSSFinder"/> implementation which checks for the presence of each substring of the first text in the 
/// second text, from the longest to the shortest.
/// </summary>
public class NaiveSNSSFinder : ISNSSFinder
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This specific implementation checks for the presence of each substring of the first text in the second text, 
    /// from the shortest to the longest.
    /// </remarks>
    public string? Find(IEnumerable<char> text1, IEnumerable<char> text2)
    {
        var string1 = string.Concat(text1);
        var string2 = string.Concat(text2);

        if (string2.Contains(string1))
            return null;

        return (
            from i in Enumerable.Range(1, string1.Length - 1)
            from j in Enumerable.Range(0, string1.Length - i + 1)
            let substringOfString1 = string1[j..(j + i)]
            where !string2.Contains(substringOfString1)
            select substringOfString1)
            .FirstOrDefault();
    }
}
