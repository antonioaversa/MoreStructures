using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixStructures.Conversions;
using MoreStructures.SuffixTries;
using System;
using System.Collections.Generic;

namespace MoreStructures.Tests.SuffixStructures.Conversions;

using static ConversionEquivalences.EquivalenceId;

public abstract partial class ConverterTests
{
    protected IConverter Converter { get; init; }

    public ConverterTests(IConverter suffixStructuresConverter)
    {
        Converter = suffixStructuresConverter;
    }

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
    public void TrieToTree_IsCorrect(ConversionEquivalences.EquivalenceId equivalenceId)
    {
        var (expectedTreeNode, trieNode) = ConversionEquivalences.Equivalences[equivalenceId];
        var treeNode = Converter.TrieToTree(trieNode);
        Assert.AreEqual(expectedTreeNode, treeNode);
    }

    private record SingletonSuffixTrieNode(SuffixTrieEdge SingleEdge, SuffixTrieNode SingleNode) : SuffixTrieNode(
        new Dictionary<SuffixTrieEdge, SuffixTrieNode> { [SingleEdge] = SingleNode }, null);
    
    [TestMethod]
    public void TrieToTree_OnlySupportsLeafAndIntermediateNodes_AsArgument()
    {
        var trieRoot = new SingletonSuffixTrieNode(new(0), new SuffixTrieNode.Leaf(7));
        Assert.ThrowsException<NotSupportedException>(() => Converter.TrieToTree(trieRoot));
    }

    [TestMethod]
    public void TrieToTree_OnlySupportsLeafAndIntermediateNodes_DeepInTheStructure()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>()
            {
                [new(3)] = new SingletonSuffixTrieNode(new(0), new SuffixTrieNode.Leaf(7)),
                [new(4)] = new SuffixTrieNode.Leaf(8),
            }),
        });
        Assert.ThrowsException<NotSupportedException>(() => Converter.TrieToTree(trieRoot));
    }
}
