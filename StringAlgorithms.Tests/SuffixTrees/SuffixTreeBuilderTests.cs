using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixStructures.Matching;
using StringAlgorithms.SuffixTrees;
using System;
using System.Linq;
using static StringAlgorithms.TextWithTerminator;

namespace StringAlgorithms.Tests.SuffixTrees;

[TestClass]
public class SuffixTreeBuilderTests
{
    private readonly SuffixTreeBuilder Builder = new();

    [TestMethod]
    public void BuildTree_EmptyString()
    {
        var root = Builder.BuildTree(string.Empty);
        Assert.AreEqual(1, root.Children.Count);
        Assert.AreEqual(new(0, 1), root.Children.Keys.Single());
        Assert.IsTrue(root.Children.Values.Single().IsLeaf());
    }

    [TestMethod]
    public void BuildTree_StringIncludingTerminator()
    {
        Assert.ThrowsException<ArgumentException>(() => Builder.BuildTree(DefaultTerminator + string.Empty));
        Assert.ThrowsException<ArgumentException>(() => Builder.BuildTree($"{DefaultTerminator}a"));
        Assert.ThrowsException<ArgumentException>(() => Builder.BuildTree($"a{DefaultTerminator}"));
        Assert.ThrowsException<ArgumentException>(() => Builder.BuildTree($"a{DefaultTerminator}a"));
    }

    [TestMethod]
    public void BuildTree_SingleCharString()
    {
        var root = Builder.BuildTree("a");
        Assert.AreEqual(2, root.Children.Count);
        Assert.IsTrue(root[new(0, 2)].IsLeaf());
        Assert.IsTrue(root[new(1, 1)].IsLeaf());
    }

    [TestMethod]
    public void BuildTree_TwoCharsString_DifferentPrefixes()
    {
        SuffixTreeNode root = Builder.BuildTree("ab");

        Assert.AreEqual(3, root.Children.Count);
        Assert.IsTrue(root[new(0, 3)].IsLeaf());
        Assert.IsTrue(root[new(1, 2)].IsLeaf());
        Assert.IsTrue(root[new(2, 1)].IsLeaf());
    }

    [TestMethod]
    public void BuildTree_TwoCharsString_SamePrefixes()
    {
        SuffixTreeNode root = Builder.BuildTree("aa");

        Assert.AreEqual(2, root.Children.Count);

        var child1 = root[new(0, 1)];
        Assert.IsTrue(child1.Children.Count == 2);
        Assert.IsTrue(child1[new(1, 2)].IsLeaf());
        Assert.IsTrue(child1[new(2, 1)].IsLeaf());

        var child2 = root[new(2, 1)];
        Assert.IsTrue(child2.IsLeaf());
    }

    [TestMethod]
    public void BuildTree_ThreeCharsString_SamePrefixes()
    {
        SuffixTreeNode root = Builder.BuildTree("aaa");

        Assert.AreEqual(2, root.Children.Count);

        var child1 = root[new(0, 1)];
        Assert.IsTrue(child1.Children.Count == 2);

        var grandChild1 = child1[new(1, 1)];
        Assert.IsTrue(grandChild1.Children.Count == 2);

        var grandGrandChild1 = grandChild1[new(2, 2)];
        Assert.IsTrue(grandGrandChild1.IsLeaf());

        var grandGrandChild2 = grandChild1[new(3, 1)];
        Assert.IsTrue(grandGrandChild2.IsLeaf());

        var grandChild2 = child1[new(3, 1)];
        Assert.IsTrue(grandChild2.IsLeaf());

        var child2 = root[new(3, 1)];
        Assert.IsTrue(child2.IsLeaf());
    }

    [TestMethod]
    public void BuildTree_ThreeCharsString_PartiallySamePrefixes()
    {
        SuffixTreeNode root = Builder.BuildTree("aba");

        Assert.AreEqual(3, root.Children.Count);

        var child1 = root[new(0, 1)];
        Assert.IsTrue(child1.Children.Count == 2);

        var grandChild1 = child1[new(1, 3)];
        Assert.IsTrue(grandChild1.IsLeaf());

        var grandChild2 = child1[new(3, 1)];
        Assert.IsTrue(grandChild2.IsLeaf());

        var child2 = root[new(1, 3)];
        Assert.IsTrue(child2.IsLeaf());

        var child3 = root[new(3, 1)];
        Assert.IsTrue(child3.IsLeaf());
    }

    [TestMethod]
    public void BuildTree_ThreeCharsString_DifferentPrefixes()
    {
        SuffixTreeNode root = Builder.BuildTree("abc");

        Assert.AreEqual(4, root.Children.Count);

        var child1 = root[new(0, 4)];
        Assert.IsTrue(child1.IsLeaf());

        var child2 = root[new(1, 3)];
        Assert.IsTrue(child2.IsLeaf());

        var child3 = root[new(2, 2)];
        Assert.IsTrue(child3.IsLeaf());

        var child4 = root[new(3, 1)];
        Assert.IsTrue(child4.IsLeaf());
    }

    [TestMethod]
    public void BuildTree_ThreeCharsString_ExtendingPrefixes()
    {
        SuffixTreeNode root = Builder.BuildTree("aababcabcd");

        Assert.AreEqual(5, root.Children.Count);

        Assert.IsTrue(root[new(0, 1)].Children.Count == 2);
        Assert.IsTrue(root[new(0, 1)][new(1, 10)].Children.Count == 0);
        Assert.IsTrue(root[new(0, 1)][new(2, 1)].Children.Count == 2);
        Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(3, 8)].Children.Count == 0);
        Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(5, 1)].Children.Count == 2);
        Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(5, 1)][new(6, 5)].Children.Count == 0);
        Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(5, 1)][new(9, 2)].Children.Count == 0);
        Assert.IsTrue(root[new(2, 1)].Children.Count == 2);
        Assert.IsTrue(root[new(2, 1)][new(3, 8)].Children.Count == 0);
        Assert.IsTrue(root[new(2, 1)][new(5, 1)].Children.Count == 2);
        Assert.IsTrue(root[new(2, 1)][new(5, 1)][new(6, 5)].Children.Count == 0);
        Assert.IsTrue(root[new(2, 1)][new(5, 1)][new(9, 2)].Children.Count == 0);
        Assert.IsTrue(root[new(5, 1)].Children.Count == 2);
        Assert.IsTrue(root[new(5, 1)][new(6, 5)].Children.Count == 0);
        Assert.IsTrue(root[new(5, 1)][new(9, 2)].Children.Count == 0);
        Assert.IsTrue(root[new(9, 2)].Children.Count == 0);
        Assert.IsTrue(root[new(10, 1)].Children.Count == 0);
    }

    [TestMethod]
    public void BuildTree_ReturnsOnlySuffixes()
    {
        var text = new TextWithTerminator("aababcabcd");
        SuffixTreeNode root = Builder.BuildTree(text);

        foreach (var rootToLeafPath in root.GetAllNodeToLeafPaths())
        {
            Assert.IsTrue(rootToLeafPath.IsSuffixOf(text));
        }
    }

    [TestMethod]
    public void BuildTree_ReturnsAllSuffixes()
    {
        var text = new TextWithTerminator("aababcabcd");
        var allSuffixes = Enumerable
            .Range(0, text.Length)
            .Select(i => text[i..])
            .ToHashSet();

        var root = Builder.BuildTree(text);
        var suffixes = root
            .GetAllSuffixesFor(text)
            .ToHashSet();

        Assert.IsTrue(allSuffixes.SetEquals(suffixes));
    }

    [TestMethod]
    public void BuildTree_UsesTerminatorForMatchToDistinguishSuffixesFromAnySubstring()
    {
        var text1 = new TextWithTerminator("abab");
        var root1 = Builder.BuildTree(text1);
        Assert.IsTrue(root1.Match(text1, "ab") is { Success: true });
        Assert.IsTrue(root1.Match(text1, "abab") is { Success: true });
    }

    [TestMethod]
    public void BuildTree_StartLeftNullAtNonLeafNodes()
    {
        var text1 = new TextWithTerminator("abababab");
        var root1 = Builder.BuildTree(text1);
        Assert.IsTrue((
            from rootToLeafPath in root1.GetAllNodeToLeafPaths()
            from nonLeafNode in rootToLeafPath.PathNodes.SkipLast(1)
            select nonLeafNode.Value.Start == null)
            .All(e => e));
    }

    [TestMethod]
    public void BuildTree_StartCorrectlySetAtLeafNodes()
    {
        var text1 = new TextWithTerminator("abababab");
        var root1 = Builder.BuildTree(text1);
        Assert.IsTrue((
            from rootToLeafPath in root1.GetAllNodeToLeafPaths()
            let suffixStart = rootToLeafPath.PathNodes.Last().Value.Start ?? throw new Exception("Invalid leaf Start")
            let suffix = rootToLeafPath.SuffixFor(text1)
            select text1[suffixStart..] == suffix)
            .All(e => e));
    }

    [TestMethod]
    public void BuildTree_BuildsExampleTreeFromExampleText()
    {
        Assert.AreEqual(SuffixTreeNodeTests.BuildSuffixTreeExample(), Builder.BuildTree(TestUtilities.ExampleText1));
    }
}
