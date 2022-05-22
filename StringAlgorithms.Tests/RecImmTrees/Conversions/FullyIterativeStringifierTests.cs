using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees.Conversions;
using System.Collections.Generic;
using static StringAlgorithms.Tests.RecImmTrees.Conversions.TreeMock;

namespace StringAlgorithms.Tests.RecImmTrees.Conversions;

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
    public void Stringify_RunsOnDeepStructures()
    {
        var numberOfNodes = 10000;
        var root = new Node(0);
        for (int i = numberOfNodes - 1; i >= 1; i--)
            root = new Node(i, new Dictionary<Edge, Node> { [new(i - 1)] = root });

        Stringifier.Stringify(root);
    }
}
