using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTries;
using System;
using System.Linq;
using static StringAlgorithms.TextWithTerminator;

namespace StringAlgorithms.Tests.SuffixTries;

[TestClass]
public class SuffixTrieBuilderTests
{
    private readonly SuffixTrieBuilder Builder = new();

    [TestMethod]
    public void BuildTree_WithTextWithTerminatorInput()
    {
        Assert.AreEqual(Builder.BuildTree(""), Builder.BuildTree(new TextWithTerminator("")));
        Assert.AreEqual(Builder.BuildTree("a"), Builder.BuildTree(new TextWithTerminator("a")));
        Assert.AreEqual(Builder.BuildTree("aa"), Builder.BuildTree(new TextWithTerminator("aa")));
    }

    [TestMethod]
    public void BuildTree_EmptyString()
    {
        var root = Builder.BuildTree(string.Empty);
        Assert.AreEqual(1, root.Children.Count);
        Assert.AreEqual(new(0), root.Children.Keys.Single());
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
        Assert.IsFalse(root[new(0)].IsLeaf());
        Assert.IsTrue(root[new(0)][new(1)].IsLeaf());
        Assert.IsTrue(root[new(1)].IsLeaf());
    }

    [TestMethod]
    public void BuildTree_TwoCharsString_DifferentPrefixes()
    {
        SuffixTrieNode root = Builder.BuildTree("ab");

        Assert.AreEqual(3, root.Children.Count);
        Assert.IsTrue(root[new(0)][new(1)][new(2)].IsLeaf());
        Assert.IsTrue(root[new(1)][new(2)].IsLeaf());
        Assert.IsTrue(root[new(2)].IsLeaf());
    }

    [TestMethod]
    public void BuildTree_TwoCharsString_SamePrefixes()
    {
        SuffixTrieNode root = Builder.BuildTree("aa");

        Assert.AreEqual(2, root.Children.Count);

        var child1 = root[new(0)];
        Assert.IsTrue(child1.Children.Count == 2);
        Assert.IsTrue(child1[new(1)].Children.Count == 1);
        Assert.IsTrue(child1[new(1)][new(2)].IsLeaf());
        Assert.IsTrue(child1[new(2)].IsLeaf());

        var child2 = root[new(2)];
        Assert.IsTrue(child2.IsLeaf());
    }

    [TestMethod]
    public void BuildTree_ThreeCharsString_SamePrefixes()
    {
        SuffixTrieNode root = Builder.BuildTree("aaa");

        Assert.AreEqual(2, root.Children.Count);
        Assert.AreEqual(2, root[new(0)].Children.Count);
        Assert.AreEqual(2, root[new(0)][new(1)].Children.Count);
        Assert.AreEqual(1, root[new(0)][new(1)][new(2)].Children.Count);
        Assert.AreEqual(0, root[new(0)][new(1)][new(2)][new (3)].Children.Count);
        Assert.AreEqual(0, root[new(0)][new(1)][new(3)].Children.Count);
        Assert.AreEqual(0, root[new(0)][new(3)].Children.Count);
        Assert.AreEqual(0, root[new(3)].Children.Count);
    }

    [TestMethod]
    public void BuildTree_ThreeCharsString_PartiallySamePrefixes()
    {
        SuffixTrieNode root = Builder.BuildTree("aba");

        Assert.AreEqual(3, root.Children.Count);
        Assert.AreEqual(2, root[new(0)].Children.Count);
        Assert.AreEqual(1, root[new(0)][new(1)].Children.Count);
        Assert.AreEqual(1, root[new(0)][new(1)][new(2)].Children.Count);
        Assert.AreEqual(0, root[new(0)][new(1)][new(2)][new(3)].Children.Count);
        Assert.AreEqual(0, root[new(0)][new(3)].Children.Count);
        Assert.AreEqual(1, root[new(1)].Children.Count);
        Assert.AreEqual(1, root[new(1)][new(2)].Children.Count);
        Assert.AreEqual(0, root[new(1)][new(2)][new(3)].Children.Count);
        Assert.AreEqual(0, root[new(3)].Children.Count);
    }

    [TestMethod]
    public void BuildTree_ThreeCharsString_DifferentPrefixes()
    {
        SuffixTrieNode root = Builder.BuildTree("abc");

        Assert.AreEqual(4, root.Children.Count);
        Assert.AreEqual(1, root[new(0)].Children.Count);
        Assert.AreEqual(1, root[new(0)][new(1)].Children.Count);
        Assert.AreEqual(1, root[new(0)][new(1)][new(2)].Children.Count);
        Assert.AreEqual(0, root[new(0)][new(1)][new(2)][new(3)].Children.Count);
        Assert.AreEqual(1, root[new(1)].Children.Count);
        Assert.AreEqual(1, root[new(1)][new(2)].Children.Count);
        Assert.AreEqual(0, root[new(1)][new(2)][new(3)].Children.Count);
        Assert.AreEqual(1, root[new(2)].Children.Count);
        Assert.AreEqual(0, root[new(2)][new(3)].Children.Count);
        Assert.AreEqual(0, root[new(3)].Children.Count);
    }

    [TestMethod]
    public void BuildTree_ReturnsOnlySuffixes()
    {
        var text = new TextWithTerminator("aababcabcd");
        SuffixTrieNode root = Builder.BuildTree(text);

        foreach (var rootToLeafPath in root.GetAllNodeToLeafPaths())
        {
            var suffix = rootToLeafPath.SuffixFor(text);
            Assert.IsTrue(text.EndsWith(suffix));
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

    // TODO: fix the following test
    /*
    [TestMethod]
    public void BuildTree_UsesTerminatorForMatchToDistinguishSuffixesFromAnySubstring()
    {
        var text1 = new TextWithTerminator("abab");
        var root1 = Build(text1);
        Assert.IsTrue(root1.Match(text1, "ab") is SuffixTrieMatch { Success: true });
        Assert.IsTrue(root1.Match(text1, "abab") is SuffixTrieMatch { Success: true });
    }
    */

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
    public void BuildTree_BuildsExampleTrieFromExampleText()
    {
        Assert.AreEqual(SuffixTrieNodeTests.BuildSuffixTrieExample(), Builder.BuildTree(TestUtilities.ExampleText1));
    }
}
