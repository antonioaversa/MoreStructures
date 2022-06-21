using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Matching;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

[TestClass]
public class NarrowingIntervalMatcherTests : MatcherTests
{
    public NarrowingIntervalMatcherTests() : base(
        bwt => new NarrowingIntervalMatcher(bwt, BWTransform.QuickSort),
        (bwt, sbwt) => new NarrowingIntervalMatcher(bwt, sbwt))
    {
    }
}
