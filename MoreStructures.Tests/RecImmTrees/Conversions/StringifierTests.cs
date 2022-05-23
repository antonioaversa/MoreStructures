using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using static MoreStructures.Tests.RecImmTrees.Conversions.StringifierTestsHelpers;
using static MoreStructures.Tests.RecImmTrees.Conversions.TreeMock;

namespace MoreStructures.Tests.RecImmTrees.Conversions;

public abstract class StringifierTests
{
    protected IStringifier<Edge, Node> Stringifier { get; init; }
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

    public StringifierTests(IStringifier<Edge, Node> stringifier)
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
        var rootStr = Stringifier.Stringify(root);
        var validResults = new string[]
        {
            $"R(1){NL}{I}e(1):N(2){NL}{I}e(2):N(3)",
            $"R(1){NL}{I}e(2):N(3){NL}{I}e(1):N(2)",
        };
        Assert.IsTrue(validResults.Contains(rootStr));
    }

    [TestMethod]
    public void Stringify_OfThreeLevelsTree()
    {
        var root = new Node(1, new Dictionary<Edge, Node>
        {
            [new(1)] = new(2, new Dictionary<Edge, Node>
            {
                [new(3)] = new(4),
            }),
            [new(2)] = new(3),
        });
        var rootStr = Stringifier.Stringify(root);
        var validResults = new string[]
        {
            $"R(1){NL}{I}e(1):N(2){NL}{I}{I}e(3):N(4){NL}{I}e(2):N(3)",
            $"R(1){NL}{I}e(2):N(3){NL}{I}e(1):N(2){NL}{I}{I}e(3):N(4)",
        };
        Assert.IsTrue(validResults.Contains(rootStr));
    }

    [TestMethod]
    public void Stringify_OfFourLevelsTree()
    {
        var root = new Node(1, new Dictionary<Edge, Node>
        {
            [new(1)] = new(2, new Dictionary<Edge, Node>
            {
                [new(3)] = new(4),
                [new(4)] = new(5),
                [new(5)] = new(6, new Dictionary<Edge, Node>
                {
                    [new(6)] = new(7),
                }),
                [new(7)] = new(8),
            }),
            [new(2)] = new(3),
            [new(8)] = new(9, new Dictionary<Edge, Node>
            {
                [new(9)] = new(10),
            }),
        });
        AssertAreEqualBySetOfLines(
            Stringifier, 
            root,
            $"R(1)",
            $"{I}e(1):N(2)",
            $"{I}{I}e(3):N(4)",
            $"{I}{I}e(4):N(5)",
            $"{I}{I}e(5):N(6)",
            $"{I}{I}{I}e(6):N(7)",
            $"{I}{I}e(7):N(8)",
            $"{I}e(2):N(3)",
            $"{I}e(8):N(9)",
            $"{I}{I}e(9):N(10)"
        );
    }
}
