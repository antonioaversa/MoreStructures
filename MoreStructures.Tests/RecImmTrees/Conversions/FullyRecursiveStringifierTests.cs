using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Conversions;
using System.Collections.Generic;

namespace MoreStructures.Tests.RecImmTrees.Conversions;

using static TreeMock;

[TestClass]
public class FullyRecursiveStringifierTests : StringifierBaseTests
{
    public FullyRecursiveStringifierTests() : base(
        new FullyRecursiveStringifier<Edge, Node>(
            DefaultRootStringifier, DefaultEdgeAndNodeStringifier)
        {
            NewLine = DefaultNewLine,
            Indent = DefaultIndent,
        })
    {
    }

    [TestMethod]
    public void Stringify_DoesntStackOverflowWithNotSoDeepStructures()
    {
        var numberOfNodes = 100;
        var root = new Node(0);
        for (int i = numberOfNodes - 1; i >= 1; i--)
            root = new Node(i, new Dictionary<Edge, Node> { [new(i - 1)] = root });

        Stringifier.Stringify(root);
    }
}
