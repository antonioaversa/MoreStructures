using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var suffixArray = suffixArrayBuilder.Build();
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
    [DataRow("abaa", "abaa$b", false, 5, 3, 3)]
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

    [TestMethod]
    public void BWT_IsNotSupported()
    {
        Assert.ThrowsException<NotSupportedException>(() => MatcherBuilder(new("")).BWT);
    }

    [TestMethod]
    public void Ctor_WithSuffixArrayWithDynamicIndexes()
    {
        var suffixArrayList = new List<int> 
        { 
            9, // $ 
            0, // aba...
            6, // abc$
            2, // ac...
            1, // ba...
            7, // bc$
            4, // bca...
            5, // ca...
            3, // cb...
        };

        IEnumerable<int> GetInts()
        {
            foreach (var i in suffixArrayList)
                yield return i;
        }

        var text = new TextWithTerminator("abacbcabc");
        var bwtBuilder = new LastFirstPropertyBasedBuilder();
        var sbwt = BWTransform.QuickSort(new(text)).sortedText;
        var matcher1 = new SuffixArrayBasedMatcher(sbwt, text, new(suffixArrayList));
        var matcher2 = new SuffixArrayBasedMatcher(sbwt, text, new(GetInts()));

        Assert.AreEqual(matcher1.Match("a"), matcher2.Match("a"));
        Assert.AreEqual(matcher1.Match("ab"), matcher2.Match("ab"));
        Assert.AreEqual(matcher1.Match("aba"), matcher2.Match("aba"));
        Assert.AreEqual(matcher1.Match("abb"), matcher2.Match("abb"));
    }
}