using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixTrees;

[TestClass]
public class SuffixTreeNodeTests
{
    private record SuffixTreeNodeInvalidLeaf()
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }, null)
    {
        [ExcludeFromCodeCoverage]
        public override bool IsEquivalentTo(SuffixTreeNode other, params TextWithTerminator[] texts)
        {
            throw new NotImplementedException();
        }
    }

    private record SuffixTreeNodeInvalidIntermediate()
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { [new(0, 1)] = new Leaf(0) }, 0)
    {
        [ExcludeFromCodeCoverage]
        public override bool IsEquivalentTo(SuffixTreeNode other, params TextWithTerminator[] texts)
        {
            throw new NotImplementedException();
        }
    }

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

    [TestMethod]
    public void IsEquivalent_IsCorrectWithLeaves()
    {
        var leaf1 = new SuffixTreeNode.Leaf(5);
        Assert.IsTrue(leaf1.IsEquivalentTo(leaf1));

        var leaf2 = new SuffixTreeNode.Leaf(5);
        Assert.IsTrue(leaf1.IsEquivalentTo(leaf2));
        Assert.IsTrue(leaf2.IsEquivalentTo(leaf1));

        var leaf3 = new SuffixTreeNode.Leaf(6);
        Assert.IsFalse(leaf1.IsEquivalentTo(leaf3));
        Assert.IsFalse(leaf3.IsEquivalentTo(leaf1));

        var intermediate1 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 1)] = leaf1
            });
        Assert.IsFalse(leaf1.IsEquivalentTo(intermediate1));
        Assert.IsTrue(leaf1.IsEquivalentTo(intermediate1.Children[new(0, 1)]));
    }

    [TestMethod]
    public void IsEquivalent_IsCorrectWithIntermediateWithSingleCharEdges()
    {
        var leaf1 = new SuffixTreeNode.Leaf(5);
        var leaf2 = new SuffixTreeNode.Leaf(6);
        var intermediate1 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 1)] = leaf1
            });
        var intermediate2 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 1)] = leaf1
            });
        Assert.IsTrue(intermediate2.IsEquivalentTo(intermediate1, new TextWithTerminator("")));
        Assert.IsTrue(intermediate2.IsEquivalentTo(intermediate1, new TextWithTerminator("a")));

        var intermediate3 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 1)] = leaf2
            });
        Assert.IsFalse(intermediate3.IsEquivalentTo(intermediate1, new TextWithTerminator("a")));

        var intermediate4 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 1)] = leaf1,
                [new(1, 1)] = leaf1,
            });
        Assert.IsFalse(intermediate4.IsEquivalentTo(intermediate1, new TextWithTerminator("ab")));

        var intermediate5 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(1, 1)] = leaf1,
            });
        Assert.IsFalse(intermediate5.IsEquivalentTo(intermediate1, new TextWithTerminator("ab")));
        Assert.IsTrue(intermediate5.IsEquivalentTo(intermediate1, new TextWithTerminator("aa")));
    }

    [TestMethod]
    public void IsEquivalent_IsCorrectWithIntermediateWithMultipleCharsEdges()
    {
        var intermediate1 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 3)] = new SuffixTreeNode.Leaf(5)
            });
        var intermediate2 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(2, 3)] = new SuffixTreeNode.Leaf(5)
            });

        Assert.IsTrue(intermediate2.IsEquivalentTo(intermediate1, new TextWithTerminator("ababa")));
        Assert.IsFalse(intermediate2.IsEquivalentTo(intermediate1, new TextWithTerminator("abcba")));
    }

    [TestMethod]
    public void IsEquivalent_IsCorrectWithIntermediateWithSameChildrenButShuffledEdges()
    {
        var intermediate1 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 1)] = new SuffixTreeNode.Leaf(5),
                [new(1, 2)] = new SuffixTreeNode.Leaf(6),
                [new(3, 3)] = new SuffixTreeNode.Leaf(0),
            });
        var intermediate2 = new SuffixTreeNode.Intermediate(
            new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(0, 1)] = new SuffixTreeNode.Leaf(6),
                [new(1, 2)] = new SuffixTreeNode.Leaf(5),
                [new(3, 3)] = new SuffixTreeNode.Leaf(0),
            });
        Assert.IsFalse(intermediate2.IsEquivalentTo(intermediate1, new TextWithTerminator("abcde")));
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
