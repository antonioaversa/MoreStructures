using StringAlgorithms.RecImmTrees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StringAlgorithms.Tests.RecImmTrees.Stringifiable;

[ExcludeFromCodeCoverage]
public static class TreeMock
{
    public record Edge(int Id) 
        : IRecImmDictIndexedTreeEdge<Edge, Node, Builder>
    {
    }

    public record Node(int Id, IDictionary<Edge, Node> Children) 
        : IRecImmDictIndexedTreeNode<Edge, Node, Builder>
    {
        public Node(int id) : this(id, new Dictionary<Edge, Node> { }) { }
    }

    public class Builder : IRecImmDictIndexedTreeBuilder<Edge, Node, Builder>
    {
    }
}
