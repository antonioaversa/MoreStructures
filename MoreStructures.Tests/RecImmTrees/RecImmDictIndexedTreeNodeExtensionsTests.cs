using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees;
using MoreStructures.Tests.SuffixTrees;
using MoreStructures.Tests.SuffixTries;

namespace MoreStructures.Tests.RecImmTrees;

[TestClass]
public class RecImmDictIndexedTreeNodeExtensionsTests
{
    [TestMethod]
    public void IsLeaf_IsCorrectForSuffixTree()
    {
        var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
        Assert.IsFalse(root.IsLeaf());
        Assert.IsFalse(root[new(0, 1)][new(1, 1)].IsLeaf());
        Assert.IsTrue(root[new(0, 1)][new(1, 1)][new(3, 1)].IsLeaf());
    }

    [TestMethod]
    public void IsLeaf_IsCorrectForSuffixTrie()
    {
        var root = SuffixTrieNodeTests.BuildSuffixTrieExample();
        Assert.IsFalse(root.IsLeaf());
        Assert.IsFalse(root[new(0)][new(1)].IsLeaf());
        Assert.IsTrue(root[new(0)][new(1)][new(2)][new(3)].IsLeaf());
    }
}
