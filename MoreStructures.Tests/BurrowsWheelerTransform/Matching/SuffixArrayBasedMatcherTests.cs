using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Matching;
using MoreStructures.SuffixArrays.Builders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

[TestClass]
public class SuffixArrayBasedMatcherTests : MatcherTests
{
    public SuffixArrayBasedMatcherTests() : base(
        text => 
        {
            var suffixArrayBuilder = new NaiveSuffixArrayBuilder(text);
            var suffixArray = suffixArrayBuilder.Build().ToList();
            var bwtBuilder = new LastFirstPropertyBasedBuilder();
            var sbwt = BWTransform.QuickSort(new(text)).sortedText;
            return new SuffixArrayBasedMatcher(sbwt, text, suffixArray);
        })
    {
    }

    [DataRow("mississippi", "xissi", false, 0, -1, -1)] // Matching goes forwards => matches no char
    [DataRow("mississippi", "issix", false, 4, 3, 3)]  // Matching goes forwards => matches 4 chars then fails
    [DataRow("mississippi", "x", false, 0, -1, -1)] // Fail right away
    [DataRow("abaabaabbacba", "abaabc", false, 5, 4, 4)]
    [DataRow("abaabaabbacba", "abaabac", false, 6, 4, 4)]
    [DataTestMethod]
    public void Match_IsCorrect(
        string textContent, string patternContent, bool expectedSuccess, int expectedMatchedChars, int expectedStart,
        int expectedEnd)
    {
        var text = new TextWithTerminator(textContent);
        var matcher = MatcherBuilder(text);
        var (success, matchedChars, startIndex, endIndex) = matcher.Match(patternContent);
        Assert.AreEqual(expectedSuccess, success);
        Assert.AreEqual(expectedMatchedChars, matchedChars);
        Assert.AreEqual(expectedStart, startIndex);
        Assert.AreEqual(expectedEnd, endIndex);
    }
}