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
            return new CountBasedNarrowingIntervalMatcher(bwt, BWTransform.QuickSort);
        })
    {
    }
}
