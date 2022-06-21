using MoreStructures.BurrowsWheelerTransform.Builders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class NaiveBuilderTests : BuilderTests
{
    public NaiveBuilderTests() : base(new NaiveBuilder())
    {
    }

    // TODO: fix issue with ElementAt being O(n) instead of O(1) for strings
    // [TestMethod]
    // public void Memory()
    // {
    //     var text = new TextWithTerminator(Enumerable.Repeat("abcdefabcdeabcbce", 1000000).SelectMany(s => s));
    //     var bwt = Builder.BuildTransform(text);
    // }
}
