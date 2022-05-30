using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Matching;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

[TestClass]
public class NarrowingIntervalMatcherTests : MatcherTests
{
    public NarrowingIntervalMatcherTests() : base(new NarrowingIntervalMatcher())
    {
    }
}
