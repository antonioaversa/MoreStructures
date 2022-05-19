using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.CountTrees;
using System.Collections.Generic;
using static StringAlgorithms.Tests.CountTrees.TreeMock;

namespace StringAlgorithms.Tests.CountTrees;

[TestClass]
public class CountTreePathTests
{
    [TestMethod]
    public void Equals_BasedOnWrappedPath()
    {
        var wrapping1 = new CountTreePath<Edge, Node, Path, Builder>(new Path() 
        { 
            PathNodes = new List<KeyValuePair<Edge, Node>> { new(new(0), new(11)), new(new(1), new(12)) }
        });
        var wrapping2 = new CountTreePath<Edge, Node, Path, Builder>(new Path()
        {
            PathNodes = new List<KeyValuePair<Edge, Node>> { new(new(0), new(11)), new(new(1), new(12)) }
        });
        Assert.AreEqual(wrapping1, wrapping2);
        Assert.AreEqual(wrapping1.PathNodes, wrapping2.PathNodes);

        var wrapping3 = new CountTreePath<Edge, Node, Path, Builder>(new Path()
        {
            PathNodes = new List<KeyValuePair<Edge, Node>> { new(new(0), new(11)), new(new(1), new(13)) }
        });
        Assert.AreNotEqual(wrapping1, wrapping3);
    }
}
