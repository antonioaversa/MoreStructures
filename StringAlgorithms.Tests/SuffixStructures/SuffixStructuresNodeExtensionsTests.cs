﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTrees.Tests;
using StringAlgorithms.SuffixTries;
using StringAlgorithms.SuffixTries.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.SuffixStructures.Tests
{
    [TestClass]
    public class SuffixStructuresNodeExtensionsTests
    {
        [TestMethod]
        public void GetAllNodeToLeafPaths_IsCorrect()
        {
            int CountOccurrencesByEdges(
                IEnumerable<SuffixTriePath> paths,
                params SuffixTrieEdge[] pathToFind) => (
                    from path in paths
                    let pathKeys = path.PathNodes.Select(kvp => kvp.Key)
                    where pathKeys.SequenceEqual(pathToFind)
                    select path)
                    .Count();

            var root = SuffixTrieNodeTests.BuildSuffixTrieExample();
            var rootToLeafPaths = root.GetAllNodeToLeafPaths().ToList();

            Assert.AreEqual(4, rootToLeafPaths.Count);

            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(1), new(2), new(3)));
            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(1), new(3)));
            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(3)));
            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new SuffixTrieEdge(3)));
        }

        [TestMethod]
        public void GetAllSuffixesFor_IsCorrect()
        {
            var text = new TextWithTerminator("abc");
            var root = SuffixTrieNodeTests.BuildSuffixTrieExample();
            var suffixes = root.GetAllSuffixesFor(text);

            var t = text.Terminator;
            Assert.IsTrue(suffixes.OrderBy(s => s).SequenceEqual(
                new List<string> { $"abc{t}", $"ab{t}", $"a{t}", $"{t}" }.OrderBy(s => s)));
        }

        [TestMethod]
        public void GetAllNodeToLeafPaths_IsCorrect2()
        {
            int CountOccurrencesByEdges(
                IEnumerable<SuffixTreePath> paths,
                params SuffixTreeEdge[] pathToFind) => (
                    from path in paths
                    let pathKeys = path.PathNodes.Select(kvp => kvp.Key)
                    where pathKeys.SequenceEqual(pathToFind)
                    select path)
                    .Count();

            var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
            var rootToLeafPaths = root.GetAllNodeToLeafPaths().ToList();

            Assert.AreEqual(4, rootToLeafPaths.Count);

            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(2, 2)));
            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(3, 1)));
            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(3, 1)));
            Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new SuffixTreeEdge(3, 1)));
        }

        [TestMethod]
        public void GetAllSuffixesFor_IsCorrect2()
        {
            var text = new TextWithTerminator("abc");
            var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
            var suffixes = root.GetAllSuffixesFor(text);

            var t = text.Terminator;
            Assert.IsTrue(suffixes.OrderBy(s => s).SequenceEqual(
                new List<string> { $"abc{t}", $"ab{t}", $"a{t}", $"{t}" }.OrderBy(s => s)));
        }
    }
}
