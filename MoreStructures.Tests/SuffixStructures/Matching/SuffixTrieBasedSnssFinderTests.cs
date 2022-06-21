using MoreStructures.SuffixStructures.Matching;

namespace MoreStructures.Tests.SuffixTrees.Matching;

[TestClass]
public class SuffixTrieBasedSnssFinderTests : SuffixStructureBasedSnssFinderTests
{
    public SuffixTrieBasedSnssFinderTests() 
        : base(new SuffixTrieBasedSnssFinder(DefaultTerminator1, DefaultTerminator2))
    {
    }

    [TestMethod]
    public void Ctor_RaiseExceptionWhenTerminatorsAreEqual()
    {
        Assert.ThrowsException<ArgumentException>(() => new SuffixTrieBasedSnssFinder('#', '#'));
        Assert.ThrowsException<ArgumentException>(() => new SuffixTrieBasedSnssFinder('$', '$'));
    }
}
