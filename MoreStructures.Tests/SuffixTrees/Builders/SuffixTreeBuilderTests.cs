using MoreStructures.RecImmTrees.Paths;
using MoreStructures.SuffixStructures;
using MoreStructures.SuffixStructures.Builders;
using MoreStructures.SuffixStructures.Matching;
using MoreStructures.SuffixTrees;
using MoreStructures.Utilities;
using static MoreStructures.TextWithTerminator;

namespace MoreStructures.Tests.SuffixTrees.Builders;

using static BuilderEquivalences.EquivalenceId;

public abstract class SuffixTreeBuilderTests
{
    protected static readonly INodeToLeafPathsBuilder NodeToLeafPathsBuilder = 
        new FullyIterativeNodeToLeafPathsBuilder();

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
    [DataRow(Issue75_BreakingUkkonen)]
    [DataTestMethod]
    public void BuildTree_IsCorrectWithSingleText(BuilderEquivalences.EquivalenceId equivalenceId)
    {
        var (texts, expectedTreeNode) = BuilderEquivalences.Equivalences[equivalenceId];
        var (fullText, _) = texts.GenerateFullText();
        var treeNode = Builder.BuildTree(texts);
        Assert.IsTrue(
            treeNode.IsEquivalentTo(expectedTreeNode, fullText), 
            $"Expected: {expectedTreeNode}, Actual: {treeNode}");
    }

    [DataRow(EmptyStrings)]
    [DataRow(OneNonEmptyOneEmpty)]
    [DataRow(OneEmptyOneNonEmpty)]
    [DataRow(TwoEmptyOneNonEmpty)]
    [DataRow(TwoEmptyOneNonEmptyDifferentOrder)]
    [DataRow(TwoNonSharingChars)]
    [DataRow(TwoSharingChars)]
    [DataRow(TwoSame)]
    [DataRow(ThreeDifferent)]
    [DataRow(TwoSameOneDifferent)]
    [DataRow(ThreeSame)]
    [DataTestMethod]
    public void BuildTree_IsCorrectWithMultipleTexts(BuilderEquivalences.EquivalenceId equivalenceId)
    {
        var (texts, expectedTreeNode) = BuilderEquivalences.Equivalences[equivalenceId];
        var (fullText, _) = texts.GenerateFullText();
        var treeNode = Builder.BuildTree(texts);
        Assert.IsTrue(
            treeNode.IsEquivalentTo(expectedTreeNode, fullText),
            $"Expected: {expectedTreeNode}, Actual: {treeNode}");
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

        foreach (var rootToLeafPath in 
            NodeToLeafPathsBuilder.GetAllNodeToLeafPaths<SuffixTreeEdge, SuffixTreeNode>(root))
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
            .Select(i => text[i..].AsValue() as IEnumerable<char>)
            .ToHashSet();

        var root = Builder.BuildTree(text);
        var suffixes = root
            .GetAllSuffixesFor<SuffixTreeEdge, SuffixTreeNode>(text)
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
            from rootToLeafPath in NodeToLeafPathsBuilder.GetAllNodeToLeafPaths<SuffixTreeEdge, SuffixTreeNode>(root1)
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
            from rootToLeafPath in NodeToLeafPathsBuilder.GetAllNodeToLeafPaths<SuffixTreeEdge, SuffixTreeNode>(root1)
            let suffixStart = rootToLeafPath.PathNodes.Last().Value.Start ?? throw new Exception("Invalid leaf Start")
            let suffix = rootToLeafPath.SuffixFor(text1)
            select text1[suffixStart..].SequenceEqual(suffix))
            .All(e => e));
    }

    [TestMethod]
    public void BuildTree_BuildsExampleTreeFromExampleText()
    {
        var expectedTreeNode = SuffixTreeNodeTests.BuildSuffixTreeExample();
        var treeNode = Builder.BuildTree(TestUtilities.ExampleText1);
        Assert.IsTrue(
            expectedTreeNode.IsEquivalentTo(treeNode, TestUtilities.ExampleText1),
            $"Expected: {expectedTreeNode}, Actual: {treeNode}");
    }
}
