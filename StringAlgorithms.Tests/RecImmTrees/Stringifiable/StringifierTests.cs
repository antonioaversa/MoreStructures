using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees.Stringifiable;
using System;
using System.Collections.Generic;
using static StringAlgorithms.Tests.RecImmTrees.Stringifiable.TreeMock;

namespace StringAlgorithms.Tests.RecImmTrees.Stringifiable;

public abstract class StringifierTests
{
    protected IStringifier<Edge, Node, Path, Builder> Stringifier { get; init; }
    protected string NL => Stringifier.NewLine;
    protected string I => Stringifier.Indent;

    protected static readonly string DefaultNewLine =
        Environment.NewLine;
    protected static readonly string DefaultIndent = 
        new(' ', 1);
    protected static readonly Func<Node, string> DefaultRootStringifier =
        n => $"R({n.Id})";
    protected static readonly Func<Edge, Node, string> DefaultEdgeAndNodeStringifier =
        (e, n) => $"e({e.Id}):N({n.Id})";

    public StringifierTests(IStringifier<Edge, Node, Path, Builder> stringifier)
    {
        Stringifier = stringifier;
    }

    [TestMethod]
    public void Stringify_OfLeaf()
    {
        var root = new Node(1);
        Assert.AreEqual("R(1)", Stringifier.Stringify(root));
    }

    [TestMethod]
    public void Stringify_OfTwoLevelsTree()
    {
        var root = new Node(1, new Dictionary<Edge, Node> 
        {
            [new(1)] = new(2),
            [new(2)] = new(3),
        });
        Assert.AreEqual($"R(1){NL}{I}e(1):N(2){NL}{I}e(2):N(3)", Stringifier.Stringify(root));
    }

    [TestMethod]
    public void Stringify_OfThreeLevelsTree()
    {
        var root = new Node(1, new Dictionary<Edge, Node>
        {
            [new(1)] = new(2,
            new Dictionary<Edge, Node>
            {
                [new(3)] = new(4),
            }),
            [new(2)] = new(3),
        });
        Assert.AreEqual($"R(1){NL}{I}e(1):N(2){NL}{I}{I}e(3):N(4){NL}{I}e(2):N(3)", Stringifier.Stringify(root));
    }
}
