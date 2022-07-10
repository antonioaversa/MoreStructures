using MoreStructures.KnuthMorrisPratt.Matching;

namespace MoreStructures.Tests.KnuthMorrisPratt.Matching;

[TestClass]
public class MatcherTests
{
    [DataRow("abcdabcaba", "a", new int[] { 0, 4, 7, 9 })]
    [DataRow("abcdabcaba", "ab", new int[] { 0, 4, 7 })]
    [DataRow("abcdabcaba", "abc", new int[] { 0, 4 })]
    [DataRow("abcdabcaba", "abcd", new int[] { 0 })]
    [DataRow("abcdabcaba", "abcde", new int[] { })]
    [DataRow("abcdabcaba", "e", new int[] { })]
    [DataTestMethod]
    public void Match_IsCorrect(string text, string pattern, int[] startIndexes)
    {
        var matches = Matcher.Match(text, pattern, '$').ToList();
        Assert.IsTrue(matches.All(m => m.Success));
        Assert.IsTrue(startIndexes.SequenceEqual(matches.Select(m => m.Begin)));
        Assert.IsTrue(matches.All(m => m.MatchedChars == pattern.Length));
    }
}
