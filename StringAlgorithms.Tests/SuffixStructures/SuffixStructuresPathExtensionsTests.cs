using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTrees;
using StringAlgorithms.Tests.SuffixTrees;
using StringAlgorithms.Tests.SuffixTries;

namespace StringAlgorithms.Tests.SuffixStructures;

[TestClass]
public class SuffixStructuresPathExtensionsTests
{
    [TestMethod]
    public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTree()
    {
        var path = SuffixTreePathTests.BuildSuffixTreePathExample();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual($"abaa{TestUtilities.ExampleText2.Terminator}", suffix);
    }

    [TestMethod]
    public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTrie()
    {
        var path = SuffixTriePathTests.BuildSuffixTriePathExample();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual($"abaa{TestUtilities.ExampleText2.Terminator}", suffix);
    }

    [TestMethod]
    public void SuffixFor_IsCorrectForEmptyPath()
    {
        var path = new SuffixTreeBuilder().EmptyPath();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual(string.Empty, suffix);
    }

    [TestMethod]
    public void IsSuffixOf_IsCorrectForNonEmtpyPath()
    {
        var path = SuffixTreePathTests.BuildSuffixTreePathExample();
        Assert.IsTrue(path.IsSuffixOf(TestUtilities.ExampleText2));
    }

    [TestMethod]
    public void IsSuffixOf_IsTrueForEmtpyPath()
    {
        var path = new SuffixTreeBuilder().EmptyPath();
        Assert.IsTrue(path.IsSuffixOf(new("anytext")));
    }
}
