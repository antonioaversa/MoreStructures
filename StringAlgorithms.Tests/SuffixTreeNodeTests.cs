using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests
{
    [TestClass]
    public class SuffixTreeNodeTests
    {
        [TestMethod]
        public void GetAllNodeToLeafPaths_Correctness()
        {
            var root = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>
            {
                [new(0, 1)] = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>
                {
                    [new(1, 1)] = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>
                    {
                        [new(2, 2)] = new SuffixTreeNode(),
                        [new(3, 1)] = new SuffixTreeNode(),
                    }),
                    [new(3, 1)] = new SuffixTreeNode(),
                }),
                [new(3, 1)] = new SuffixTreeNode(),
            });

            var rootToLeafPaths = root.GetAllNodeToLeafPaths().ToList();
            Assert.AreEqual(4, rootToLeafPaths.Count);

            Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new(0, 1), new(1, 1), new(2, 2)));
            Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new(0, 1), new(1, 1), new(3, 1)));
            Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new(0, 1), new(3, 1)));
            Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new PrefixPath(3, 1)));
        }

        private static int CountOccurrencesByPrefixPaths(
            IEnumerable<IEnumerable<KeyValuePair<PrefixPath, SuffixTreeNode>>> paths,
            params PrefixPath[] pathToFind) => (
                from path in paths
                let pathKeys = path.Select(kvp => kvp.Key)
                where pathKeys.SequenceEqual(pathToFind)
                select path)
                .Count();
    }
}
