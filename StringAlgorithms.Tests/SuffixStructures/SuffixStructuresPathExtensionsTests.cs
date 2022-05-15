using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTrees.Tests;
using StringAlgorithms.SuffixTries.Tests;
using System.Linq;

namespace StringAlgorithms.Tests.SuffixStructures
{
    [TestClass]
    public class SuffixStructuresPathExtensionsTests
    {
        [TestMethod]
        public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTree()
        {
            var path = SuffixTreePathTests.BuildSuffixTreePathExample();
            var suffix = path.SuffixFor(SuffixTreeNodeTests.ExampleText);
            Assert.AreEqual($"abaa{SuffixTreeNodeTests.ExampleText.Terminator}", suffix);
        }

        [TestMethod]
        public void SuffixFor_IsCorrectForNonEmptyPathOnSuffixTrie()
        {
            var path = SuffixTriePathTests.BuildSuffixTriePathExample();
            var suffix = path.SuffixFor(SuffixTrieNodeTests.ExampleText);
            Assert.AreEqual($"abaa{SuffixTrieNodeTests.ExampleText.Terminator}", suffix);
        }

        [TestMethod]
        public void SuffixFor_IsCorrectForEmptyPath()
        {
            var path = new SuffixTreeBuilder().EmptyPath();
            var suffix = path.SuffixFor(SuffixTreeNodeTests.ExampleText);
            Assert.AreEqual(string.Empty, suffix);
        }

        [TestMethod]
        public void IsSuffixOf_IsCorrectForNonEmtpyPath()
        {
            var path = SuffixTreePathTests.BuildSuffixTreePathExample();
            Assert.IsTrue(path.IsSuffixOf(SuffixTreeNodeTests.ExampleText));
        }

        [TestMethod]
        public void IsSuffixOf_IsTrueForEmtpyPath()
        {
            var path = new SuffixTreeBuilder().EmptyPath();
            Assert.IsTrue(path.IsSuffixOf(new("anytext")));
        }

        [TestMethod]
        public void Concat_IsCorrectPrependingEmptyPath()
        {
            var path1 = new SuffixTreeBuilder().EmptyPath();
            var path2 = SuffixTreePathTests.BuildSuffixTreePathExample();
            var mergedPath = path1.Concat(path2);
            Assert.AreEqual(path2, mergedPath);
        }

        [TestMethod]
        public void Concat_IsCorrectAppendingEmptyPath()
        {
            var path1 = new SuffixTreeBuilder().EmptyPath();
            var path2 = SuffixTreePathTests.BuildSuffixTreePathExample();
            var mergedPath = path2.Concat(path1);
            Assert.AreEqual(path2, mergedPath);
        }

        [TestMethod]
        public void Concat_IsCorrectWithTwoNonEmptyPaths()
        {
            var path = SuffixTreePathTests.BuildSuffixTreePathExample();
            var path1 = new SuffixTreeBuilder().MultistepsPath(path.PathNodes.Take(1));
            var path2 = new SuffixTreeBuilder().MultistepsPath(path.PathNodes.Skip(1));
            var mergedPath = path1.Concat(path2);
            Assert.AreEqual(path, mergedPath);
        }

        [TestMethod]
        public void Append_IsCorrect()
        {
            var path = SuffixTreePathTests.BuildSuffixTreePathExample();
            var path1 = new SuffixTreeBuilder().MultistepsPath(path.PathNodes.SkipLast(1));
            var last = new SuffixTreeBuilder().MultistepsPath(path.PathNodes.TakeLast(1)).PathNodes.Single();
            var mergedPath = path1.Append(last.Key, last.Value);
            Assert.AreEqual(path, mergedPath);
        }
    }
}
