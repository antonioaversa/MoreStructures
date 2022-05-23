using MoreStructures.RecImmTrees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreStructures.Tests.RecImmTrees.Conversions;

[ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
public static class TreeMock
{
    public record Edge(int Id) 
        : IRecImmDictIndexedTreeEdge<Edge, Node>
    {
    }

    public record Node(int Id, IDictionary<Edge, Node> Children) 
        : IRecImmDictIndexedTreeNode<Edge, Node>
    {
        public Node(int id) : this(id, new Dictionary<Edge, Node> { }) { }
    }

    public static Node BuildMostUnbalancedTree(int numberOfIntermediateNodes)
    {
        Node root = new(numberOfIntermediateNodes);
        for (int i = numberOfIntermediateNodes; i >= 1; i--)
            root = new Node(i - 1, new Dictionary<Edge, Node>
            {
                [new(i - 1)] = root,
            });
        return root;
    }
}
