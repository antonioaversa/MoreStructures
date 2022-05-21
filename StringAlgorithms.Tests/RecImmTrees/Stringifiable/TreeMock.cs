using StringAlgorithms.RecImmTrees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StringAlgorithms.Tests.RecImmTrees.Stringifiable;

[ExcludeFromCodeCoverage]
public static class TreeMock
{
    public record Edge(int Id) 
        : IRecImmDictIndexedTreeEdge<Edge, Node, Path, Builder>
    {
    }

    public record Node(int Id, IDictionary<Edge, Node> Children) 
        : IRecImmDictIndexedTreeNode<Edge, Node, Path, Builder>
    {
        public Node(int id) : this(id, new Dictionary<Edge, Node> { }) { }
    }

    public record Path(IEnumerable<KeyValuePair<Edge, Node>> PathNodes) 
        : IRecImmDictIndexedTreePath<Edge, Node, Path, Builder>
    {
        public Path() : this(new List<KeyValuePair<Edge, Node>> { }) { }
    }

    public class Builder : IRecImmDictIndexedTreeBuilder<Edge, Node, Path, Builder>
    {
        public Path EmptyPath() => 
            throw new NotSupportedException();
        public Path MultistepsPath(params (Edge edge, Node node)[] pathNodes) => 
            throw new NotSupportedException();
        public Path MultistepsPath(IEnumerable<KeyValuePair<Edge, Node>> pathNodes) => 
            throw new NotSupportedException();
        public Path SingletonPath(Edge edge, Node node) => 
            throw new NotSupportedException();
    }
}
