using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Matching;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

[TestClass]
public class NarrowingIntervalMatcherTests : MatcherTests
{
    public NarrowingIntervalMatcherTests() : base(
        text =>
        {
            var bwtBuilder = new LastFirstPropertyBasedBuilder();
            var bwt = bwtBuilder.BuildTransform(text).Content;
            return new NarrowingIntervalMatcher(bwt, BWTransform.QuickSort);
        })
    {
    }

    protected NarrowingIntervalMatcherTests(
        Func<TextWithTerminator, IMatcher> matcherBuilder)
        : base(matcherBuilder)
    {
    }

    [TestMethod]
    public virtual void Ctor_RaisesExceptionWithIncosistentBWTAndSortedBWT()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new NarrowingIntervalMatcher(new("a#", '#'), new RotatedTextWithTerminator("$a", '$')));
        Assert.ThrowsException<ArgumentException>(
            () => new NarrowingIntervalMatcher(new("a#$", '#'), new RotatedTextWithTerminator("$#a", '$')));
    }

    [TestMethod]
    public virtual void Match_RaisesExceptionWithEmptyPattern()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new NarrowingIntervalMatcher(new("a$"), new RotatedTextWithTerminator("$a")).Match(""));
    }

    [DataRow("mississippi", "xissi", false, 4, 3, 4)] // Matching goes backwards => matches 4 chars then fails
    [DataRow("mississippi", "issix", false, 0, -1, -1)] // Matching goes backwards => matches no char
    [DataRow("mississippi", "x", false, 0, -1, -1)] // Fail right away
    [DataTestMethod]
    public void Match_IsCorrectWhenFailure(
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
