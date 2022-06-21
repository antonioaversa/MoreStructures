using MoreStructures.SuffixStructures.Matching;

namespace MoreStructures.Tests.SuffixTrees.Matching;

public abstract class SuffixStructureBasedSnssFinderTests : SnssFinderTests
{
    protected const char DefaultTerminator1 = '#';
    protected const char DefaultTerminator2 = '$';

    protected SuffixStructureBasedSnssFinderTests(ISnssFinder finder) : base(finder)
    {
    }

    [TestMethod]
    public void Find_RaiseExceptionWhenAnyTerminatorIsInAnyText()
    {
        Assert.ThrowsException<ArgumentException>(
            () => Finder.Find($"a{DefaultTerminator1}", $"b"));
        Assert.ThrowsException<ArgumentException>(
            () => Finder.Find($"{DefaultTerminator1}a", $"b"));
        Assert.ThrowsException<ArgumentException>(
            () => Finder.Find($"a{DefaultTerminator1}a", $"b"));
        Assert.ThrowsException<ArgumentException>(
            () => Finder.Find($"a", $"b{DefaultTerminator1}"));
        Assert.ThrowsException<ArgumentException>(
            () => Finder.Find($"a", $"b{DefaultTerminator2}"));
        Assert.ThrowsException<ArgumentException>(
            () => Finder.Find($"a", $"{DefaultTerminator2}b{DefaultTerminator1}"));
    }
}
