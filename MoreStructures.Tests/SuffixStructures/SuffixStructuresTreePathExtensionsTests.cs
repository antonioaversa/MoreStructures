using MoreStructures.RecImmTrees;
using MoreStructures.SuffixStructures;
using MoreStructures.SuffixTrees;
using MoreStructures.Tests.RecImmTrees;
using MoreStructures.Utilities;

namespace MoreStructures.Tests.SuffixStructures;

[TestClass]
public class SuffixStructuresTreePathExtensionsTests
{
    [TestMethod]
    public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTree()
    {
        var path = TreePathTests.BuildSuffixTreePathExample();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual($"abaa{TestUtilities.ExampleText2.Terminator}".AsValue(), suffix);
    }

    [TestMethod]
    public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTrie()
    {
        var path = TreePathTests.BuildSuffixTriePathExample();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual($"abaa{TestUtilities.ExampleText2.Terminator}".AsValue(), suffix);
    }

    [TestMethod]
    public void SuffixFor_IsCorrectForEmptyPath()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        var suffix = path.SuffixFor(TestUtilities.ExampleText2);
        Assert.AreEqual(string.Empty.AsValue(), suffix);
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

    [TestMethod]
    public void ContainsIndex_ThrowsExceptionOnInvalidIndex()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => path.ContainsIndex(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => path.ContainsIndex(-2));
    }

    [TestMethod]
    public void ContainsIndex_IsAlwaysFalseOnAEmptyPath()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        Assert.IsFalse(path.ContainsIndex(0));
        Assert.IsFalse(path.ContainsIndex(2));
    }

    [TestMethod]
    public void ContainsIndex_IsCorrect()
    {
        var path = BuildExamplePath();

        Assert.IsFalse(path.ContainsIndex(0));
        Assert.IsFalse(path.ContainsIndex(1));
        Assert.IsTrue(path.ContainsIndex(2));
        Assert.IsTrue(path.ContainsIndex(3));
        Assert.IsTrue(path.ContainsIndex(6));
        Assert.IsTrue(path.ContainsIndex(9));
        Assert.IsTrue(path.ContainsIndex(10));
        Assert.IsFalse(path.ContainsIndex(11));
        Assert.IsFalse(path.ContainsIndex(20));
    }

    [TestMethod]
    public void ContainsIndexesNonBiggerThan_ThrowsExceptionOnInvalidIndex()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => path.ContainsIndexesNonBiggerThan(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => path.ContainsIndexesNonBiggerThan(-2));
    }

    [TestMethod]
    public void ContainsIndexesNonBiggerThan_IsAlwaysFalseOnAEmptyPath()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        Assert.IsFalse(path.ContainsIndexesNonBiggerThan(0));
        Assert.IsFalse(path.ContainsIndexesNonBiggerThan(2));
    }

    [TestMethod]
    public void ContainsIndexesNonBiggerThan_IsCorrect()
    {
        var path = BuildExamplePath();

        Assert.IsFalse(path.ContainsIndexesNonBiggerThan(0));
        Assert.IsFalse(path.ContainsIndexesNonBiggerThan(1));
        Assert.IsTrue(path.ContainsIndexesNonBiggerThan(2));
        Assert.IsTrue(path.ContainsIndexesNonBiggerThan(3));
        Assert.IsTrue(path.ContainsIndexesNonBiggerThan(6));
        Assert.IsTrue(path.ContainsIndexesNonBiggerThan(9));
        Assert.IsTrue(path.ContainsIndexesNonBiggerThan(10));
        Assert.IsTrue(path.ContainsIndexesNonBiggerThan(11));
        Assert.IsTrue(path.ContainsIndexesNonBiggerThan(20));
    }

    /// <remarks>
    /// Path: <c>- (2,4) -> I1 - (6, 1) -> I2 - (7, 2) -> I3 - (9, 2) -> L</c>
    /// </remarks>
    private static TreePath<SuffixTreeEdge, SuffixTreeNode> BuildExamplePath()
    {
        var leaf = new SuffixTreeNode.Leaf(3);
        var edgeToLeaf = new SuffixTreeEdge(9, 2);
        var intermediate3 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [edgeToLeaf] = leaf,
        });
        var edgeToIntermediate3 = new SuffixTreeEdge(7, 2);
        var intermediate2 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [edgeToIntermediate3] = intermediate3,
        });
        var edgeToIntermediate2 = new SuffixTreeEdge(6, 1);
        var intermediate1 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [edgeToIntermediate2] = intermediate2,
        });
        var edgeToIntermediate1 = new SuffixTreeEdge(2, 4);

        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>(
            (edgeToIntermediate1, intermediate1),
            (edgeToIntermediate2, intermediate2),
            (edgeToIntermediate3, intermediate3),
            (edgeToLeaf, leaf));
        return path;
    }
}
