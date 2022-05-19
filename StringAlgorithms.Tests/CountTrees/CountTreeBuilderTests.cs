using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.CountTrees;
using System.Collections.Generic;
using static StringAlgorithms.Tests.CountTrees.TreeMock;

namespace StringAlgorithms.Tests.CountTrees;

[TestClass]
public class CountTreeBuilderTests
{
    [TestMethod]
    public void EmptyPath_WrapsUnderlyingEmptyPath()
    {
        var wrappedPath = new Builder().EmptyPath();
        var wrappingPath1 = new CountTreeBuilder<Edge, Node, Path, Builder>().EmptyPath();
        Assert.AreEqual(wrappedPath, wrappingPath1.WrappedPath);

        var wrappingPath2 = new CountTreeBuilder<Edge, Node, Path, Builder>().SingletonPath(
            new(new Edge(2)), new(new Node(4)));
        Assert.AreNotEqual(wrappedPath, wrappingPath2.WrappedPath);
    }

    [TestMethod]
    public void SingletonPath_WrapsUnderlyingSingletonPath()
    {
        var wrappedPath = new Builder().SingletonPath(new Edge(2), new Node(4));
        var wrappingPath1 = new CountTreeBuilder<Edge, Node, Path, Builder>().SingletonPath(
            new(new Edge(2)), new(new Node(4)));
        Assert.AreEqual(wrappedPath, wrappingPath1.WrappedPath);

        var wrappingPath2 = new CountTreeBuilder<Edge, Node, Path, Builder>().SingletonPath(
            new(new Edge(2)), new(new Node(5)));
        Assert.AreNotEqual(wrappedPath, wrappingPath2.WrappedPath);
    }

    [TestMethod]
    public void MultistepsPath_WrapsUnderlyingMultistepsPath_WithValueTuples()
    {
        var wrappedPath = new Builder().MultistepsPath((new Edge(2), new Node(4)), (new Edge(3), new Node(6)));
        var wrappingPath1 = new CountTreeBuilder<Edge, Node, Path, Builder>().MultistepsPath(
            (new(new Edge(2)), new(new Node(4))), (new(new Edge(3)), new(new Node(6))));
        Assert.AreEqual(wrappedPath, wrappingPath1.WrappedPath);

        var wrappingPath2 = new CountTreeBuilder<Edge, Node, Path, Builder>().MultistepsPath(
            (new(new Edge(2)), new(new Node(4))), (new(new Edge(5)), new(new Node(6))));
        Assert.AreNotEqual(wrappedPath, wrappingPath2.WrappedPath);
    }

    [TestMethod]
    public void MultistepsPath_WrapsUnderlyingMultistepsPath_WithEnumerable()
    {
        var wrappedPath = new Builder().MultistepsPath(
            new List<KeyValuePair<Edge, Node>>
            {
                new(new Edge(2), new Node(4)), new (new Edge(3), new Node(6))
            });
        var wrappingPath1 = new CountTreeBuilder<Edge, Node, Path, Builder>().MultistepsPath(
            new List<KeyValuePair<CountTreeEdge<Edge, Node, Path, Builder>, CountTreeNode<Edge, Node, Path, Builder>>> 
            { 
                new(new(new Edge(2)), new(new Node(4))), new(new(new Edge(3)), new(new Node(6)))
            });
        Assert.AreEqual(wrappedPath, wrappingPath1.WrappedPath);

        var wrappingPath2 = new CountTreeBuilder<Edge, Node, Path, Builder>().MultistepsPath(
            new List<KeyValuePair<CountTreeEdge<Edge, Node, Path, Builder>, CountTreeNode<Edge, Node, Path, Builder>>>
            {
                new(new(new Edge(2)), new(new Node(4))), new(new(new Edge(5)), new(new Node(6)))
            });
        Assert.AreNotEqual(wrappedPath, wrappingPath2.WrappedPath);
    }
}