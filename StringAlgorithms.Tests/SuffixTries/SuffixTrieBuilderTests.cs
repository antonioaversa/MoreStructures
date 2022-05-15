using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTries;
using System;
using System.Linq;
using static StringAlgorithms.SuffixTries.SuffixTrieBuilder;
using static StringAlgorithms.TextWithTerminator;

namespace StringAlgorithms.Tests.SuffixTries;

[TestClass]
public class SuffixTrieBuilderTests
{
    [TestMethod]
    public void Build_WithTextWithTerminatorInput()
    {
        Assert.AreEqual(Build(""), Build(new TextWithTerminator("")));
        Assert.AreEqual(Build("a"), Build(new TextWithTerminator("a")));
        Assert.AreEqual(Build("aa"), Build(new TextWithTerminator("aa")));
    }

    [TestMethod]
    public void Build_EmptyString()
    {
        var root = Build(string.Empty);
        Assert.AreEqual(1, root.Children.Count);
        Assert.AreEqual(new(0), root.Children.Keys.Single());
        Assert.IsTrue(root.Children.Values.Single().IsLeaf());
    }
    
    [TestMethod]
    public void Build_StringIncludingTerminator()
    {
        Assert.ThrowsException<ArgumentException>(() => Build(DefaultTerminator + string.Empty));
        Assert.ThrowsException<ArgumentException>(() => Build($"{DefaultTerminator}a"));
        Assert.ThrowsException<ArgumentException>(() => Build($"a{DefaultTerminator}"));
        Assert.ThrowsException<ArgumentException>(() => Build($"a{DefaultTerminator}a"));
    }

    [TestMethod]
    public void Build_SingleCharString()
    {
        var root = Build("a");
        Assert.AreEqual(2, root.Children.Count);
        Assert.IsFalse(root[new(0)].IsLeaf());
        Assert.IsTrue(root[new(0)][new(1)].IsLeaf());
        Assert.IsTrue(root[new(1)].IsLeaf());
    }

    [TestMethod]
    public void Build_TwoCharsString_DifferentPrefixes()
    {
        SuffixTrieNode root = Build("ab");

        Assert.AreEqual(3, root.Children.Count);
        Assert.IsTrue(root[new(0)][new(1)][new(2)].IsLeaf());
        Assert.IsTrue(root[new(1)][new(2)].IsLeaf());
        Assert.IsTrue(root[new(2)].IsLeaf());
    }

    [TestMethod]
    public void Build_TwoCharsString_SamePrefixes()
    {
        SuffixTrieNode root = Build("aa");

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
    public void Build_ThreeCharsString_SamePrefixes()
    {
        SuffixTrieNode root = Build("aaa");

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
    public void Build_ThreeCharsString_PartiallySamePrefixes()
    {
        SuffixTrieNode root = Build("aba");

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
    public void Build_ThreeCharsString_DifferentPrefixes()
    {
        SuffixTrieNode root = Build("abc");

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
    public void Build_ReturnsOnlySuffixes()
    {
        var text = new TextWithTerminator("aababcabcd");
        SuffixTrieNode root = Build(text);

        foreach (var rootToLeafPath in root.GetAllNodeToLeafPaths())
        {
            var suffix = rootToLeafPath.SuffixFor(text);
            Assert.IsTrue(text.EndsWith(suffix));
        }
    }

    [TestMethod]
    public void Build_ReturnsAllSuffixes()
    {
        var text = new TextWithTerminator("aababcabcd");
        var allSuffixes = Enumerable
            .Range(0, text.Length)
            .Select(i => text[i..])
            .ToHashSet();

        var root = Build(text);
        var suffixes = root
            .GetAllSuffixesFor(text)
            .ToHashSet();

        Assert.IsTrue(allSuffixes.SetEquals(suffixes));
    }

    // TODO: fix the following test
    /*
    [TestMethod]
    public void Build_UsesTerminatorForMatchToDistinguishSuffixesFromAnySubstring()
    {
        var text1 = new TextWithTerminator("abab");
        var root1 = Build(text1);
        Assert.IsTrue(root1.Match(text1, "ab") is SuffixTrieMatch { Success: true });
        Assert.IsTrue(root1.Match(text1, "abab") is SuffixTrieMatch { Success: true });
    }
    */

    [TestMethod]
    public void Build_StartLeftNullAtNonLeafNodes()
    {
        var text1 = new TextWithTerminator("abababab");
        var root1 = Build(text1);
        Assert.IsTrue((
            from rootToLeafPath in root1.GetAllNodeToLeafPaths()
            from nonLeafNode in rootToLeafPath.PathNodes.SkipLast(1)
            select nonLeafNode.Value.Start == null)
            .All(e => e));
    }

    [TestMethod]
    public void Build_StartCorrectlySetAtLeafNodes()
    {
        var text1 = new TextWithTerminator("abababab");
        var root1 = Build(text1);
        Assert.IsTrue((
            from rootToLeafPath in root1.GetAllNodeToLeafPaths()
            let suffixStart = rootToLeafPath.PathNodes.Last().Value.Start ?? throw new Exception("Invalid leaf Start")
            let suffix = rootToLeafPath.SuffixFor(text1)
            select text1[suffixStart..] == suffix)
            .All(e => e));
    }
}
