using MoreStructures.SuffixStructures;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;
using MoreStructures.Tests.SuffixTrees;
using MoreStructures.Tests.SuffixTries;

namespace MoreStructures.Tests.SuffixStructures;

[TestClass]
public class SuffixStructuresNodeExtensionsTests
{
    [TestMethod]
    public void GetAllSuffixesFor_IsCorrectWithTries()
    {
        var text = new TextWithTerminator("abc");
        var root = SuffixTrieNodeTests.BuildSuffixTrieExample();
        var suffixes = root
            .GetAllSuffixesFor<SuffixTrieEdge, SuffixTrieNode>(text)
            .Select(s => string.Concat(s));

        var t = text.Terminator;
        Assert.IsTrue(suffixes.OrderBy(s => s).SequenceEqual(
            new List<string> { $"abc{t}", $"ab{t}", $"a{t}", $"{t}" }.OrderBy(s => s)));
    }

    [TestMethod]
    public void GetAllSuffixesFor_IsCorrectWithTrees()
    {
        var text = new TextWithTerminator("abc");
        var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
        var suffixes = root
            .GetAllSuffixesFor<SuffixTreeEdge, SuffixTreeNode>(text)
            .Select(s => string.Concat(s)); ;

        var t = text.Terminator;
        Assert.IsTrue(suffixes.OrderBy(s => s).SequenceEqual(
            new List<string> { $"abc{t}", $"ab{t}", $"a{t}", $"{t}" }.OrderBy(s => s)));
    }
}
