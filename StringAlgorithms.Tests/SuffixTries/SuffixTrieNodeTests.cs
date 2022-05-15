using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.SuffixTries.Tests
{
    [TestClass]
    public class SuffixTrieNodeTests
    {
        private record SuffixTrieNodeInvalidLeaf()
            : SuffixTrieNode(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { }, null);

        private record SuffixTrieNodeInvalidIntermediate()
            : SuffixTrieNode(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { [new(0)] = new Leaf(0) }, 0);

        [TestMethod]
        public void Ctor_InvalidArguments()
        {
            Assert.ThrowsException<ArgumentException>(() => new SuffixTrieNode.Leaf(-1));
            Assert.ThrowsException<ArgumentException>(() => new SuffixTrieNodeInvalidLeaf());
            Assert.ThrowsException<ArgumentException>(() => new SuffixTrieNodeInvalidIntermediate());
        }

        [TestMethod]
        public void Indexer_RetrievesChild()
        {
            var root = BuildSuffixTrieExample();
            Assert.AreEqual(root.Children[new(0)], root[new(0)]);
            Assert.AreEqual(root.Children[new(3)], root[new(3)]);
        }

        [TestMethod]
        public void Children_ImmutabilityOnGet()
        {
            var root = BuildSuffixTrieExample();
            Assert.ThrowsException<NotSupportedException>(
                () => root.Children.Clear());
            Assert.ThrowsException<NotSupportedException>(
                () => root.Children[root.Children.First().Key] = new SuffixTrieNode.Leaf(0));
            Assert.ThrowsException<NotSupportedException>(
                () => root.Children.Remove(root.Children.First().Key));
        }

        [TestMethod]
        public void Children_Immutability_FromCtorParam()
        {
            var rootChildren = new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(1)] = new SuffixTrieNode.Leaf(1),
                }),
            };
            var root = new SuffixTrieNode.Intermediate(rootChildren);
            Assert.AreEqual(1, root.Children.Count);

            rootChildren.Add(new(1), new SuffixTrieNode.Leaf(1));
            Assert.AreEqual(1, root.Children.Count);
        }

        [TestMethod]
        public void IsLeaf()
        {
            var root = BuildSuffixTrieExample();
            Assert.IsFalse(root.IsLeaf());
            Assert.IsFalse(root[new(0)][new(1)].IsLeaf());
            Assert.IsTrue(root[new(0)][new(1)][new(2)][new(3)].IsLeaf());
        }

        internal static TextWithTerminator ExampleText => new("ababaa");

        /// <remarks>
        /// The example is built from the text <see cref="ExampleText"/>.
        /// </remarks>
        internal static SuffixTrieNode BuildSuffixTrieExample()
        {
            return new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(3)] = new SuffixTrieNode.Leaf(0)
                        }),
                        [new(3)] = new SuffixTrieNode.Leaf(1),
                    }),
                    [new(3)] = new SuffixTrieNode.Leaf(2),
                }),
                [new(3)] = new SuffixTrieNode.Leaf(3),
            });
        }
    }
}
