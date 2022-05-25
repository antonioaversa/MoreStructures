using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Conversions;
using System.Linq;

namespace MoreStructures.Tests.RecImmTrees.Conversions;

using static TreeMock;
using static StringifierTestsHelpers;

[TestClass]
public class FullyIterativeStringifierTests : StringifierTests
{
    public FullyIterativeStringifierTests() : base(
        new FullyIterativeStringifier<Edge, Node>(
            DefaultRootStringifier, DefaultEdgeAndNodeStringifier)
        {
            NewLine = DefaultNewLine,
            Indent = DefaultIndent,
        })
    {
    }

    [TestMethod]
    public void Stringify_DoesntStackOverflowWithDeepStructures()
    {
        var numberOfIntermediateNodes = 10000;
        var root = BuildMostUnbalancedTree(numberOfIntermediateNodes);

        var rootStr = Stringifier.Stringify(root);
        Assert.AreEqual(numberOfIntermediateNodes + 1, rootStr.Split(Stringifier.NewLine).Length);
    }

    [TestMethod]
    public void StopIndentingLevel_StopsIndentingWhenReached_WithZero()
    {
        var customStringifier = new FullyIterativeStringifier<Edge, Node>(
            r => r.Id.ToString(), (e, n) => n.Id.ToString())
        {
            NewLine = DefaultNewLine,
            Indent = DefaultIndent,
            StopIndentingLevel = 0,
            PrependLevelAfterStopIndenting = false,
        };
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(10),
            Enumerable.Range(0, 11).Select(i => $"{i}").ToArray());
    }

    [TestMethod]
    public void StopIndentingLevel_StopsIndentingWhenReached_WithOne()
    {
        var customStringifier = new FullyIterativeStringifier<Edge, Node>(
            r => r.Id.ToString(), (e, n) => n.Id.ToString())
        {
            NewLine = DefaultNewLine,
            Indent = DefaultIndent,
            StopIndentingLevel = 1,
            PrependLevelAfterStopIndenting = false,
        };
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(0), $"0");
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(1), $"0", $"{I}1");
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(10), 
            new string[] { $"0" }.Concat(Enumerable.Range(1, 10).Select(i => $"{I}{i}")).ToArray());
    }

    [TestMethod]
    public void StopIndentingLevel_StopsIndentingWhenReached_WithTwo()
    {
        var customStringifier = new FullyIterativeStringifier<Edge, Node>(
            r => r.Id.ToString(), (e, n) => n.Id.ToString())
        {
            NewLine = DefaultNewLine,
            Indent = DefaultIndent,
            StopIndentingLevel = 2,
            PrependLevelAfterStopIndenting = false,
        };
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(0), $"0");
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(1), $"0", $"{I}1");
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(10),
            new string[] { $"0", $"{I}1" }.Concat(Enumerable.Range(2, 9).Select(i => $"{I}{I}{i}")).ToArray());
    }

    [TestMethod]
    public void PrependLevelAfterStopIndenting_RendersLevelWhenStopIndentingLevelIsReached()
    {
        var customStringifier = new FullyIterativeStringifier<Edge, Node>(
            r => r.Id.ToString(), (e, n) => n.Id.ToString())
        {
            NewLine = DefaultNewLine,
            Indent = DefaultIndent,
            StopIndentingLevel = 2,
            PrependLevelAfterStopIndenting = true,
        };
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(0), $"0");
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(1), $"0", $"{I}1");
        AssertAreEqualBySetOfLines(customStringifier, BuildMostUnbalancedTree(10),
            new string[] { $"0", $"{I}1", $"{I}{I}2" }
                .Concat(Enumerable.Range(3, 8).Select(i => $"{I}{I}[level {i}]{i}")).ToArray());
    }
}
