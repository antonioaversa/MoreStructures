﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static StringAlgorithms.SuffixTreeBuilder;

namespace StringAlgorithms.Tests
{
    [TestClass]
    public class SuffixTreeBuilderTests 
    {
        [TestMethod]
        public void Build_EmptyString()
        {
            var root = Build(string.Empty);
            Assert.AreEqual(1, root.Children.Count);
            Assert.AreEqual(new(0, 1), root.Children.Keys.Single());
            Assert.IsTrue(root.Children.Values.Single().IsLeaf);
        }

        [TestMethod]
        public void Build_StringIncludingTerminator()
        {
            Assert.ThrowsException<ArgumentException>(() => Build(DefaultTerminator + string.Empty));
            Assert.ThrowsException<ArgumentException>(() => Build($"{DefaultTerminator}a"));
            Assert.ThrowsException<ArgumentException>(() => Build($"a{DefaultTerminator}"));
            Assert.ThrowsException<ArgumentException>(() => Build($"a{DefaultTerminator}a"));
        }

        [TestMethod]
        public void Build_SingleCharString()
        {
            var root = Build("a");
            Assert.AreEqual(2, root.Children.Count);
            Assert.IsTrue(root[new(0, 2)].IsLeaf);
            Assert.IsTrue(root[new(1, 1)].IsLeaf);
        }

        [TestMethod]
        public void Build_TwoCharsString_DifferentPrefixes()
        {
            SuffixTreeNode root = Build("ab");

            Assert.AreEqual(3, root.Children.Count);
            Assert.IsTrue(root[new(0, 3)].IsLeaf);
            Assert.IsTrue(root[new(1, 2)].IsLeaf);
            Assert.IsTrue(root[new(2, 1)].IsLeaf);
        }

        [TestMethod]
        public void Build_TwoCharsString_SamePrefixes()
        {
            SuffixTreeNode root = Build("aa");

            Assert.AreEqual(2, root.Children.Count);

            var child1 = root[new(0, 1)];
            Assert.IsTrue(child1.Children.Count == 2);
            Assert.IsTrue(child1[new(1, 2)].IsLeaf);
            Assert.IsTrue(child1[new(2, 1)].IsLeaf);

            var child2 = root[new(2, 1)];
            Assert.IsTrue(child2.IsLeaf);
        }

        [TestMethod]
        public void Build_ThreeCharsString_SamePrefixes()
        {
            SuffixTreeNode root = Build("aaa");

            Assert.AreEqual(2, root.Children.Count);

            var child1 = root[new(0, 1)];
            Assert.IsTrue(child1.Children.Count == 2);

            var grandChild1 = child1[new(1, 1)];
            Assert.IsTrue(grandChild1.Children.Count == 2);

            var grandGrandChild1 = grandChild1[new(2, 2)];
            Assert.IsTrue(grandGrandChild1.IsLeaf);

            var grandGrandChild2 = grandChild1[new(3, 1)];
            Assert.IsTrue(grandGrandChild2.IsLeaf);

            var grandChild2 = child1[new(3, 1)];
            Assert.IsTrue(grandChild2.IsLeaf);

            var child2 = root[new(3, 1)];
            Assert.IsTrue(child2.IsLeaf);
        }

        [TestMethod]
        public void Build_ThreeCharsString_PartiallySamePrefixes()
        {
            SuffixTreeNode root = Build("aba");

            Assert.AreEqual(3, root.Children.Count);

            var child1 = root[new(0, 1)];
            Assert.IsTrue(child1.Children.Count == 2);

            var grandChild1 = child1[new(1, 3)];
            Assert.IsTrue(grandChild1.IsLeaf);

            var grandChild2 = child1[new(3, 1)];
            Assert.IsTrue(grandChild2.IsLeaf);

            var child2 = root[new(1, 3)];
            Assert.IsTrue(child2.IsLeaf);

            var child3 = root[new(3, 1)];
            Assert.IsTrue(child3.IsLeaf);
        }

        [TestMethod]
        public void Build_ThreeCharsString_DifferentPrefixes()
        {
            SuffixTreeNode root = Build("abc");

            Assert.AreEqual(4, root.Children.Count);

            var child1 = root[new(0, 4)];
            Assert.IsTrue(child1.IsLeaf);

            var child2 = root[new(1, 3)];
            Assert.IsTrue(child2.IsLeaf);

            var child3 = root[new(2, 2)];
            Assert.IsTrue(child3.IsLeaf);

            var child4 = root[new(3, 1)];
            Assert.IsTrue(child4.IsLeaf);
        }

        [TestMethod]
        public void Build_ThreeCharsString_ExtendingPrefixes()
        {
            SuffixTreeNode root = Build("aababcabcd");

            Assert.AreEqual(5, root.Children.Count);

            Assert.IsTrue(root[new(0, 1)].Children.Count == 2);
            Assert.IsTrue(root[new(0, 1)][new(1, 10)].Children.Count == 0);
            Assert.IsTrue(root[new(0, 1)][new(2, 1)].Children.Count == 2);
            Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(3, 8)].Children.Count == 0);
            Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(5, 1)].Children.Count == 2);
            Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(5, 1)][new(6, 5)].Children.Count == 0);
            Assert.IsTrue(root[new(0, 1)][new(2, 1)][new(5, 1)][new(9, 2)].Children.Count == 0);
            Assert.IsTrue(root[new(2, 1)].Children.Count == 2);
            Assert.IsTrue(root[new(2, 1)][new(3, 8)].Children.Count == 0);
            Assert.IsTrue(root[new(2, 1)][new(5, 1)].Children.Count == 2);
            Assert.IsTrue(root[new(2, 1)][new(5, 1)][new(6, 5)].Children.Count == 0);
            Assert.IsTrue(root[new(2, 1)][new(5, 1)][new(9, 2)].Children.Count == 0);
            Assert.IsTrue(root[new(5, 1)].Children.Count == 2);
            Assert.IsTrue(root[new(5, 1)][new(6, 5)].Children.Count == 0);
            Assert.IsTrue(root[new(5, 1)][new(9, 2)].Children.Count == 0);
            Assert.IsTrue(root[new(9, 2)].Children.Count == 0);
            Assert.IsTrue(root[new(10, 1)].Children.Count == 0);
        }
    }
}
