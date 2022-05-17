using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.Tests.SuffixTries;
using System;

namespace StringAlgorithms.Tests.SuffixStructures;

[TestClass]
public class SuffixStructuresConverterTests
{
    [TestMethod]
    public void TrieToTree_BuildsCorrectTree()
    {
        var trieRoot = SuffixTrieNodeTests.BuildSuffixTrieExample();
        Assert.ThrowsException<NotImplementedException>(
            () => SuffixStructuresConverter.TrieToTree(trieRoot));
        // TODO: continue with the implementaton
    }
}
