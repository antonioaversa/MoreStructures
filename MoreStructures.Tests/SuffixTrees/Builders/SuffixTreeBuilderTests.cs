using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees;
using MoreStructures.SuffixStructures;
using MoreStructures.SuffixStructures.Builders;
using MoreStructures.SuffixStructures.Matching;
using MoreStructures.SuffixTrees;
using System;
using System.Linq;
using static MoreStructures.TextWithTerminator;

namespace MoreStructures.Tests.SuffixTrees.Builders;

using static BuilderEquivalences.EquivalenceId;

public abstract class SuffixTreeBuilderTests
{
    protected readonly IBuilder<SuffixTreeEdge, SuffixTreeNode> Builder;

    public SuffixTreeBuilderTests(IBuilder<SuffixTreeEdge, SuffixTreeNode> builder)
    {
        Builder = builder;
    }

    [DataRow(EmptyString)]
    [DataRow(SingleChar)]
    [DataRow(TwoCharsString_DifferentPrefixes)]
    [DataRow(TwoCharsString_SamePrefixes)]
    [DataRow(ThreeCharsString_SamePrefixes)]
    [DataRow(ThreeCharsString_PartiallySamePrefixes)]
    [DataRow(ThreeCharsString_DifferentPrefixes)]
    [DataRow(TwoChars_ExtendingPrefixes)]
    [DataRow(ThreeChars_ExtendingPrefixes)]
    [DataTestMethod]
    public void BuildTree_IsCorrect(BuilderEquivalences.EquivalenceId equivalenceId)
    {
        var (text, expectedTreeNode) = BuilderEquivalences.Equivalences[equivalenceId];
        var treeNode = Builder.BuildTree(text);
        Assert.AreEqual(expectedTreeNode, treeNode);
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
            select text1[suffixStart..].SequenceEqual(suffix))
            .All(e => e));
    }

    [TestMethod]
    public void BuildTree_BuildsExampleTreeFromExampleText()
    {
        Assert.AreEqual(SuffixTreeNodeTests.BuildSuffixTreeExample(), Builder.BuildTree(TestUtilities.ExampleText1));
    }
}
