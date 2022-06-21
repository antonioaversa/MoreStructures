using MoreStructures.CountTrees;
using MoreStructures.SuffixStructures.Conversions;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;

namespace MoreStructures.Tests.SuffixStructures.Conversions;

/// <summary>
/// Runs tests defined in <see cref="ConverterTests"/> for <see cref="FullyIterativeConverter"/>.
/// </summary>
[TestClass]
public class FullyIterativeConverterTests : ConverterTests
{
    public FullyIterativeConverterTests() : base(new FullyIterativeConverter())
    {
    }

    [TestMethod]
    public void TrieToTree_DoesntStackOverflowWithDeepStructures()
    {
        var numberOfIntermediateNodes = 10000;
        SuffixTrieNode trieRoot = new SuffixTrieNode.Leaf(0);
        for (int i = numberOfIntermediateNodes; i >= 1; i--)
            trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode> 
            { 
                [new(i - 1)] = trieRoot,
                [new(numberOfIntermediateNodes)] = new SuffixTrieNode.Leaf(numberOfIntermediateNodes - i + 1),
            });

        var treeRoot = Converter.TrieToTree(trieRoot);
        var countTree = new CountTreeNode<SuffixTreeEdge, SuffixTreeNode>(treeRoot);
        Assert.AreEqual(2 * numberOfIntermediateNodes, countTree.DescendantsCount); // Root node excluded
    }
}
