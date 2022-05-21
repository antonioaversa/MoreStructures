using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTrees;
using StringAlgorithms.Tests.RecImmTrees;

namespace StringAlgorithms.Tests.SuffixStructures;

[TestClass]
public class SuffixStructuresTreePathExtensionsTests
{
    [TestMethod]
    public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTree()
    {
        var path = TreePathTests.BuildSuffixTreePathExample();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual($"abaa{TestUtilities.ExampleText2.Terminator}", suffix);
    }

    [TestMethod]
    public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTrie()
    {
        var path = TreePathTests.BuildSuffixTriePathExample();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual($"abaa{TestUtilities.ExampleText2.Terminator}", suffix);
    }

    [TestMethod]
    public void SuffixFor_IsCorrectForEmptyPath()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual(string.Empty, suffix);
    }

    [TestMethod]
    public void IsSuffixOf_IsCorrectForNonEmtpyPath()
    {
        var path = TreePathTests.BuildSuffixTreePathExample();
        Assert.IsTrue(path.IsSuffixOf(TestUtilities.ExampleText2));
    }

    [TestMethod]
    public void IsSuffixOf_IsTrueForEmtpyPath()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        Assert.IsTrue(path.IsSuffixOf(new("anytext")));
    }
}
