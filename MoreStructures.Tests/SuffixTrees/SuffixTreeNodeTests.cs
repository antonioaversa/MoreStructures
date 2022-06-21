using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixTrees;

[TestClass]
public class SuffixTreeNodeTests
{
    private record SuffixTreeNodeInvalidLeaf()
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }, null);

    private record SuffixTreeNodeInvalidIntermediate()
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { [new(0, 1)] = new Leaf(0) }, 0);

    [TestMethod]
    public void Ctor_InvalidArguments()
    {
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeNode.Leaf(-1));
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeNodeInvalidLeaf());
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeNodeInvalidIntermediate());
    }

    [TestMethod]
    public void Equality_IsAlwaysByValue()
    {
        var root1 = BuildSuffixTreeExample();
        var root2 = BuildSuffixTreeExample();
        Assert.AreEqual(root1, root2);
        Assert.IsTrue(root1 == root2);
        Assert.IsFalse(root1 != root2);
    }

    [TestMethod]
    public void Indexer_RetrievesChild()
    {
        var root = BuildSuffixTreeExample();
        Assert.AreEqual(root.Children[new(0, 1)], root[new(0, 1)]);
        Assert.AreEqual(root.Children[new(3, 1)], root[new(3, 1)]);
    }

    [TestMethod]
    public void Children_Immutability_OnGet()
    {
        var root = BuildSuffixTreeExample();
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Clear());
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children[root.Children.First().Key] = new SuffixTreeNode.Leaf(0));
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Remove(root.Children.First().Key));
    }

    [TestMethod]
    public void Children_Immutability_FromCtorParam()
    {
        var rootChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(0, 2)] = new SuffixTreeNode.Leaf(0),
            [new(1, 1)] = new SuffixTreeNode.Leaf(1),
        };
        var root = new SuffixTreeNode.Intermediate(rootChildren);
        Assert.AreEqual(2, root.Children.Count);

        rootChildren.Add(new(0, 1), new SuffixTreeNode.Leaf(0));
        Assert.AreEqual(2, root.Children.Count);
    }

    [TestMethod]
    public void ToString_IsTheSameOnEquivalentTrees()
    {
        var root1Str = BuildSuffixTreeExample().ToString();
        var root2Str = BuildSuffixTreeExample().ToString();
        Assert.AreEqual(root1Str, root2Str);
    }

    [TestMethod]
    public void ToString_OnLeafIncludesStart()
    {
        var root1Str = new SuffixTreeNode.Leaf(123).ToString();
        Assert.IsTrue(root1Str.Contains(123.ToString()));
    }

    /// <remarks>
    /// The example is built from the text <see cref="TestUtilities.ExampleText1"/>.
    /// </remarks>
    internal static SuffixTreeNode BuildSuffixTreeExample()
    {
        return new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(0, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(2, 2)] = new SuffixTreeNode.Leaf(0),
                    [new(3, 1)] = new SuffixTreeNode.Leaf(1),
                }),
                [new(3, 1)] = new SuffixTreeNode.Leaf(2),
            }),
            [new(3, 1)] = new SuffixTreeNode.Leaf(3),
        });
    }
}
