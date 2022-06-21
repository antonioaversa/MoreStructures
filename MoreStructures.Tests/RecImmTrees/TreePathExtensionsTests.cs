using MoreStructures.RecImmTrees;
using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.RecImmTrees
{
    [TestClass]
    public class TreePathExtensionsTests
    {
        [TestMethod]
        public void Concat_IsCorrectPrependingEmptyPath()
        {
            var path1 = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
            var path2 = TreePathTests.BuildSuffixTreePathExample();
            var mergedPath = path1.Concat(path2);
            Assert.AreEqual(path2, mergedPath);
        }

        [TestMethod]
        public void Concat_IsCorrectAppendingEmptyPath()
        {
            var path1 = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
            var path2 = TreePathTests.BuildSuffixTreePathExample();
            var mergedPath = path2.Concat(path1);
            Assert.AreEqual(path2, mergedPath);
        }

        [TestMethod]
        public void Concat_IsCorrectWithTwoNonEmptyPaths()
        {
            var path = TreePathTests.BuildSuffixTreePathExample();
            var path1 = new TreePath<SuffixTreeEdge, SuffixTreeNode>(path.PathNodes.Take(1));
            var path2 = new TreePath<SuffixTreeEdge, SuffixTreeNode>(path.PathNodes.Skip(1));
            var mergedPath = path1.Concat(path2);
            Assert.AreEqual(path, mergedPath);
        }

        [TestMethod]
        public void Append_IsCorrect()
        {
            var path = TreePathTests.BuildSuffixTreePathExample();
            var path1 = new TreePath<SuffixTreeEdge, SuffixTreeNode>(path.PathNodes.SkipLast(1));
            var last = new TreePath<SuffixTreeEdge, SuffixTreeNode>(path.PathNodes.TakeLast(1))
                .PathNodes.Single();
            var mergedPath = path1.Append(last.Key, last.Value);
            Assert.AreEqual(path, mergedPath);
        }
    }
}
