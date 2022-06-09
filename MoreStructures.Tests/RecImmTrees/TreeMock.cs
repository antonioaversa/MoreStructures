using MoreStructures.RecImmTrees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreStructures.Tests.RecImmTrees;

[ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
public static class TreeMock
{
    public record Edge(int Id)
        : IRecImmDictIndexedTreeEdge<Edge, Node>, IComparable<Edge>
    {
        public int CompareTo(Edge? other) => other is Edge otherEdge ? Id - otherEdge.Id : -1;
    }

    public record Node(int Id, IDictionary<Edge, Node> Children)
        : IRecImmDictIndexedTreeNode<Edge, Node>
    {
        public Node(int id) : this(id, new Dictionary<Edge, Node> { }) { }
    }

    public static Node BuildExampleTree() =>
        new(0)
        {
            Children = new Dictionary<Edge, Node>
            {
                [new Edge(0)] = new Node(1),
                [new Edge(1)] = new Node(2)
                {
                    Children = new Dictionary<Edge, Node>
                    {
                        [new Edge(2)] = new Node(3)
                        {
                            Children = new Dictionary<Edge, Node>
                            {
                                [new Edge(3)] = new Node(4),
                                [new Edge(4)] = new Node(5),
                            }
                        },
                        [new Edge(5)] = new Node(6),
                        [new Edge(6)] = new Node(7),
                    },
                },
                [new Edge(7)] = new Node(8),
            },
        };

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
