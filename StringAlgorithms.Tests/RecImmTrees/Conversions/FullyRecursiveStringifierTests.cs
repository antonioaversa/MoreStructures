﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees.Conversions;
using System.Collections.Generic;
using static StringAlgorithms.Tests.RecImmTrees.Conversions.TreeMock;

namespace StringAlgorithms.Tests.RecImmTrees.Conversions;

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
    public void Stringify_RunsOnNotSoDeepStructures()
    {
        var numberOfNodes = 100;
        var root = new Node(0);
        for (int i = numberOfNodes - 1; i >= 1; i--)
            root = new Node(i, new Dictionary<Edge, Node> { [new(i - 1)] = root });

        Stringifier.Stringify(root);
    }
}
