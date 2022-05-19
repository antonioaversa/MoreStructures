using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.CountTrees;
using static StringAlgorithms.Tests.CountTrees.TreeMock;

namespace StringAlgorithms.Tests.CountTrees;

[TestClass]
public class CountTreeEdgeTests
{
    [TestMethod]
    public void WrappedEdge_IsPreserved()
    {
        var wrapped = new Edge(12);
        var wrapping = new CountTreeEdge<Edge, Node, Path, Builder>(wrapped);
        Assert.AreEqual(wrapped, wrapping.WrappedEdge);
    }

    [TestMethod]
    public void Equals_BasedOnWrappedEdge()
    {
        var wrapping1 = new CountTreeEdge<Edge, Node, Path, Builder>(new Edge(12));
        var wrapping2 = new CountTreeEdge<Edge, Node, Path, Builder>(new Edge(12));
        Assert.AreEqual(wrapping1, wrapping2);

        var wrapping3 = new CountTreeEdge<Edge, Node, Path, Builder>(new Edge(13));
        Assert.AreNotEqual(wrapping1, wrapping3);
    }
}
