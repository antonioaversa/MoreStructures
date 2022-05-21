using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees.Stringifiable;
using System.Collections.Generic;
using static StringAlgorithms.Tests.RecImmTrees.Stringifiable.TreeMock;

namespace StringAlgorithms.Tests.RecImmTrees.Stringifiable;

[TestClass]
public class FullyRecursiveStringifierTests : StringifierTests
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
    public void CustomNewLine_IsReflectedIntoOutput()
    {
        var customStringifier = new FullyRecursiveStringifier<Edge, Node>(
            DefaultRootStringifier, DefaultEdgeAndNodeStringifier) 
        { 
            NewLine = "a",
            Indent = DefaultIndent,
        };
        var root = new Node(0, new Dictionary<Edge, Node>
        {
            [new(1)] = new(1)
        });
        Assert.AreEqual($"R(0)a{I}e(1):N(1)", customStringifier.Stringify(root));
    }

    [TestMethod]
    public void CustomIndent_IsReflectedIntoOutput()
    {
        var customStringifier = new FullyRecursiveStringifier<Edge, Node>(
            DefaultRootStringifier, DefaultEdgeAndNodeStringifier)
        {
            NewLine = DefaultNewLine,
            Indent = "t",
        };
        var root = new Node(0, new Dictionary<Edge, Node>
        {
            [new(1)] = new(1)
        });
        Assert.AreEqual($"R(0){NL}te(1):N(1)", customStringifier.Stringify(root));
    }
}
