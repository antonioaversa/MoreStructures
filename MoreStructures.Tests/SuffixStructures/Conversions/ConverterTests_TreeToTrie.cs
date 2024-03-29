﻿using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixStructures.Conversions;

using static ConversionEquivalences.EquivalenceId;

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
        var trieNode = Converter.TreeToTrie(treeNode);
        Assert.AreEqual(expectedTrieNode, trieNode);
    }

    private sealed record SingletonSuffixTreeNode(SuffixTreeEdge SingleEdge, SuffixTreeNode SingleNode) 
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { [SingleEdge] = SingleNode }, null)
    {
        [ExcludeFromCodeCoverage]
        public override bool IsEquivalentTo(SuffixTreeNode other, params TextWithTerminator[] texts)
        {
            throw new NotImplementedException();
        }
    }

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
