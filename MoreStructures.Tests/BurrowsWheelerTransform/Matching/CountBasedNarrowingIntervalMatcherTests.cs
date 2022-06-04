using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Matching;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

[TestClass]
public class CountBasedNarrowingIntervalMatcherTests : MatcherTests
{
    public CountBasedNarrowingIntervalMatcherTests() : base(
        bwt => new CountBasedNarrowingIntervalMatcher(bwt, BWTransform.QuickSort),
        (bwt, sbwt) => new CountBasedNarrowingIntervalMatcher(bwt, sbwt))
    {
    }
}
