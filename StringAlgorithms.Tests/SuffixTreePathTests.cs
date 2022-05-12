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
        public void Empty_Correctness()
        {
            Assert.IsFalse(SuffixTreePath.Empty().PathNodes.Any());
        }

        [TestMethod]
        public void Singleton_Correctness()
        {
            var node = new SuffixTreeNode();
            var singletonPath = SuffixTreePath.Singleton(new(0, 1), node);
            Assert.AreEqual(1, singletonPath.PathNodes.Count());
            Assert.AreEqual(new(0, 1), singletonPath.PathNodes.Single().Key);
            Assert.AreEqual(node, singletonPath.PathNodes.Single().Value);
        }

        [TestMethod]
        public void PathNodes_ImmutabilityOnGet()
        {
            var path = new SuffixTreePath(new Dictionary<PrefixPath, SuffixTreeNode> { });
            if (path.PathNodes is IList<KeyValuePair<PrefixPath, SuffixTreeNode>> pathNodesAsList)
            {
                Assert.ThrowsException<NotSupportedException>(() => 
                    pathNodesAsList.Add(KeyValuePair.Create(new PrefixPath(0, 1), new SuffixTreeNode())));
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
