using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.Tests.SuffixStructures
{
    [TestClass]
    public class SuffixStructuresConvertTests
    {
        [TestMethod]
        public void TrieToTree_BuildsCorrectTree()
        {
            var trieRoot = SuffixTrieNodeTests.BuildSuffixTrieExample();
            Assert.ThrowsException<NotImplementedException>(
                () => SuffixStructuresConvert.TrieToTree(trieRoot));
            // TODO: continue with the implementaton
        }
    }
}
