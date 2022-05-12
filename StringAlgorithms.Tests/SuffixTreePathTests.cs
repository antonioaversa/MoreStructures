using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms
{
    [TestClass]
    public class SuffixTreePathTests
    {
        [TestMethod]
        public void PathNodes_ImmutabilityOnGet()
        {
            var path = new SuffixTreePath(new Dictionary<PrefixPath, SuffixTreeNode> { });
            if (path is IList<SuffixTreeNode> pathAsList)
            {
                Assert.ThrowsException<NotSupportedException>(() => pathAsList.Add(new SuffixTreeNode()));
            }
        }

        [TestMethod]
        public void PathNodes_ImmutabilityOnCtorParam()
        {
            var pathNodes = new Dictionary<PrefixPath, SuffixTreeNode> { };
            var path = new SuffixTreePath(pathNodes);
            Assert.AreEqual(0, path.PathNodes.Count());
            pathNodes[new(0, 1)] = new SuffixTreeNode();
            Assert.AreEqual(0, path.PathNodes.Count());
        }
    }
}
