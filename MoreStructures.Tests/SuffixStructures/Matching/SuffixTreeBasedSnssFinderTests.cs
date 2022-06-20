using MoreStructures.SuffixStructures.Matching;

namespace MoreStructures.Tests.SuffixTrees.Matching;

[TestClass]
public class SuffixTreeBasedSnssFinderTests : SuffixStructureBasedSnssFinderTests
{
    public SuffixTreeBasedSnssFinderTests()
        : base(new SuffixTreeBasedSnssFinder(DefaultTerminator1, DefaultTerminator2))
    {
    }

    [TestMethod]
    public void Ctor_RaiseExceptionWhenTerminatorsAreEqual()
    {
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeBasedSnssFinder('#', '#'));
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeBasedSnssFinder('$', '$'));
    }
}
