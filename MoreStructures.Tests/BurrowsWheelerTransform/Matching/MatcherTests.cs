using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Matching;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

public abstract class MatcherTests
{
    protected Func<TextWithTerminator, IMatcher> MatcherBuilder { get; }

    protected MatcherTests(
        Func<TextWithTerminator, IMatcher> matcherBuilder)
    {
        MatcherBuilder = matcherBuilder;
    }

    [DataRow("mississippi", "$", true, 1, 0, 0)] // Terminator can be specified in the pattern
    [DataRow("mississippi", "i$", true, 2, 1, 1)]
    [DataRow("mississippi", "issi", true, 4, 3, 4)]
    [DataRow("mississippi", "issip", true, 5, 3, 3)]
    [DataRow("mississippi", "s", true, 1, 8, 11)]
    [DataRow("mississippi", "mi", true, 2, 5, 5)]
    [DataRow("mississippi", "mis", true, 3, 5, 5)]
    [DataRow("baba", "ba", true, 2, 3, 4)]
    [DataRow("baba", "bab", true, 3, 4, 4)]
    [DataRow("aaaaaaaaaaa", "a", true, 1, 1, 11)]
    [DataRow("abaabaabbacba", "a", true, 1, 1, 7)]
    [DataRow("abaabaabbacba", "a$", true, 2, 1, 1)]
    [DataRow("abaabaabbacba", "aa", true, 2, 2, 3)]
    [DataRow("abaabaabbacba", "ab", true, 2, 4, 6)]
    [DataRow("abaabaabbacba", "ac", true, 2, 7, 7)]
    [DataRow("abaabaabbacba", "abaa", true, 4, 4, 5)]
    [DataRow("abaabaabbacba", "abaab", true, 5, 4, 5)]
    [DataTestMethod]
    public void Match_IsCorrectWhenSuccess(
            string textContent, string patternContent, bool expectedSuccess, int expectedMatchedChars, 
            int expectedStart, int expectedEnd)
    {
        var matcher = MatcherBuilder(new(textContent));
        var (success, matchedChars, startIndex, endIndex) = matcher.Match(patternContent);
        Assert.AreEqual(expectedSuccess, success);
        Assert.AreEqual(expectedMatchedChars, matchedChars);
        Assert.AreEqual(expectedStart, startIndex);
        Assert.AreEqual(expectedEnd, endIndex);
    }
}
