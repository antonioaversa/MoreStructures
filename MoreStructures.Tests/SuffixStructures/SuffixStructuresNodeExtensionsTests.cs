﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixStructures;
using MoreStructures.Tests.SuffixTrees;
using MoreStructures.Tests.SuffixTries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.SuffixStructures;

[TestClass]
public class SuffixStructuresNodeExtensionsTests
{
    [TestMethod]
    public void GetAllSuffixesFor_IsCorrect()
    {
        var text = new TextWithTerminator("abc");
        var root = SuffixTrieNodeTests.BuildSuffixTrieExample();
        var suffixes = root.GetAllSuffixesFor(text).Select(s => string.Concat(s));

        var t = text.Terminator;
        Assert.IsTrue(suffixes.OrderBy(s => s).SequenceEqual(
            new List<string> { $"abc{t}", $"ab{t}", $"a{t}", $"{t}" }.OrderBy(s => s)));
    }

    [TestMethod]
    public void GetAllSuffixesFor_IsCorrect2()
    {
        var text = new TextWithTerminator("abc");
        var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
        var suffixes = root.GetAllSuffixesFor(text).Select(s => string.Concat(s)); ;

        var t = text.Terminator;
        Assert.IsTrue(suffixes.OrderBy(s => s).SequenceEqual(
            new List<string> { $"abc{t}", $"ab{t}", $"a{t}", $"{t}" }.OrderBy(s => s)));
    }
}