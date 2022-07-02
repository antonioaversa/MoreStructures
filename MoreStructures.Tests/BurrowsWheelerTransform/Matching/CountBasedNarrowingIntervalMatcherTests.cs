using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Matching;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

[TestClass]
public class CountBasedNarrowingIntervalMatcherTests : NarrowingIntervalMatcherTests
{
    public CountBasedNarrowingIntervalMatcherTests() : base(
        text =>
        {
            var bwtBuilder = new LastFirstPropertyBasedBuilder();
            var bwt = bwtBuilder.BuildTransform(text).Content;
            return new CountBasedNarrowingIntervalMatcher(bwt, BWTransform.QuickSort(bwt).sortedText);
        })
    {
    }

    [TestMethod]
    public override void Ctor_RaisesExceptionWithIncosistentBWTAndSortedBWT()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new CountBasedNarrowingIntervalMatcher(new("a#", '#'), new RotatedTextWithTerminator("$a", '$')));
        Assert.ThrowsException<ArgumentException>(
            () => new CountBasedNarrowingIntervalMatcher(new("a#$", '#'), new RotatedTextWithTerminator("$#a", '$')));
    }

    [TestMethod]
    public override void Match_RaisesExceptionWithEmptyPattern()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new CountBasedNarrowingIntervalMatcher(new("a$"), new RotatedTextWithTerminator("$a")).Match(""));
    }
}
