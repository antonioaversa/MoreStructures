using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTrees;
using System;
using System.Collections.Generic;
using static MoreStructures.Tests.SuffixStructures.Conversions.ConversionEquivalences.EquivalenceId;

namespace MoreStructures.Tests.SuffixStructures.Conversions;

public abstract partial class ConverterTests
{
    [DataRow(Leaf)]
    [DataRow(Example)]
    [DataRow(TwoLevelsTreeSingleChainSingleLeafToThreeLevelsTrie)]
    [DataRow(TwoLevelsTreeSingleChainTwoLeafsToThreeLevelsTrie)]
    [DataRow(TwoLevelsTreeToFourLevelsTrie)]
    [DataRow(TwoLevelsTreeToTwoLevelsTrie)]
    [DataRow(TwoLevelsTreeWithSiblingToThreeLevelsTrie)]
    [DataRow(ThreeLevelsTreeToFourLevelsTrie)]
    [DataRow(ThreeLevelsTreeToSixLevelsTrie)]
    [DataRow(OneLevelTreeToMostUnbalancedTenLevelsTrie)]
    [DataRow(OneLevelTreeToMostUnbalancedAHundredLevelsTrie)]
    [DataTestMethod]
    public void TreeToTrie_IsCorrect(ConversionEquivalences.EquivalenceId equivalenceId)
    {
        var (treeNode, expectedTrieNode) = ConversionEquivalences.Equivalences[equivalenceId];
        var trieRoot = Converter.TreeToTrie(treeNode);
        Assert.AreEqual(expectedTrieNode, trieRoot);
    }

    private record SingletonSuffixTreeNode(SuffixTreeEdge SingleEdge, SuffixTreeNode SingleNode) : SuffixTreeNode(
        new Dictionary<SuffixTreeEdge, SuffixTreeNode> { [SingleEdge] = SingleNode }, null);

    [TestMethod]
    public void TreeToTrie_OnlySupportsLeafAndIntermediateNodes_AsArgument()
    {
        var treeRoot = new SingletonSuffixTreeNode(new(0, 2), new SuffixTreeNode.Leaf(7));
        Assert.ThrowsException<NotSupportedException>(() => Converter.TreeToTrie(treeRoot));
    }

    [TestMethod]
    public void TreeToTrie_OnlySupportsLeafAndIntermediateNodes_DeepInTheStructure()
    {
        var treeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(2, 2)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>()
            {
                [new(4, 3)] = new SingletonSuffixTreeNode(new(5, 1), new SuffixTreeNode.Leaf(7)),
                [new(7, 5)] = new SuffixTreeNode.Leaf(8),
            }),
        });
        Assert.ThrowsException<NotSupportedException>(() => Converter.TreeToTrie(treeRoot));
    }
}
