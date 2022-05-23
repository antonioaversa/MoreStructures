using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Conversions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static MoreStructures.Tests.RecImmTrees.Conversions.TreeMock;

namespace MoreStructures.Tests.RecImmTrees.Conversions;

[ExcludeFromCodeCoverage(Justification = "Testing API extension")]
internal static class StringifierTestsHelpers
{
    public static void AssertAreEqualBySetOfLines(
        IStringifier<Edge, Node> stringifier, Node root, params string[] validResultsLines)
    {
        var rootStr = stringifier.Stringify(root);
        var rootStrLinesSet = rootStr.Split(stringifier.NewLine).ToHashSet();
        var validResultsLinesSet = validResultsLines.ToHashSet();
        if (!validResultsLinesSet.SetEquals(rootStrLinesSet))
            Assert.Fail(
                $"Expected: [{string.Join(", ", validResultsLinesSet.Select(s => $"\"{s}\""))}]. " +
                $"Actual: [{string.Join(", ", rootStrLinesSet.Select(s => $"\"{s}\""))}]");
    }
}