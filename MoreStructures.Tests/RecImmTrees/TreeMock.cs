using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Visitor;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MoreStructures.Tests.RecImmTrees;

[ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
public static class TreeMock
{
    public record Edge(int Id)
        : IRecImmDictIndexedTreeEdge<Edge, Node>, IComparable<Edge>
    {
        public int CompareTo(Edge? other) => other is Edge otherEdge ? Id - otherEdge.Id : -1;

        public override string ToString() => $"{Id}";
    }

    public record Node(int Id, IDictionary<Edge, Node> Children)
        : IRecImmDictIndexedTreeNode<Edge, Node>
    {
        public Node(int id) : this(id, new Dictionary<Edge, Node> { }) { }

        public override string ToString() => $"{Id}";
    }

    //     0
    //     |- 0 -> 1
    //     |       |- 1 -> 2
    //     |       |- 2 -> 3
    //     |       |       |- 3 -> 4
    //     |       |- 4 -> 5
    //     |- 5 -> 6
    //     |- 6 -> 7
    //             |- 7 -> 8
    //                     |- 8 -> 9
    //                     |- 9 -> 10
    public static Node BuildDocExample() =>
        new(0)
        {
            Children = new Dictionary<Edge, Node>
            {
                [new Edge(0)] = new Node(1)
                {
                    Children = new Dictionary<Edge, Node>
                    {
                        [new Edge(1)] = new Node(2),
                        [new Edge(2)] = new Node(3)
                        {
                            Children = new Dictionary<Edge, Node>
                            {
                                [new Edge(3)] = new Node(4),
                            }
                        },
                        [new Edge(4)] = new Node(5),
                    },
                },
                [new Edge(5)] = new Node(6),
                [new Edge(6)] = new Node(7)
                {
                    Children = new Dictionary<Edge, Node>
                    {
                        [new Edge(7)] = new Node(8)
                        {
                            Children = new Dictionary<Edge, Node>
                            {
                                [new Edge(8)] = new Node(9),
                                [new Edge(9)] = new Node(10),
                            }
                        },
                    },
                },
            }
        };

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

    public static Node BuildMultiLevelsBacktrackTree() =>
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
                            }
                        },
                    },
                },
                [new Edge(4)] = new Node(5)
                {
                    Children = new Dictionary<Edge, Node>
                    {
                        [new Edge(5)] = new Node(6)
                        {
                            Children = new Dictionary<Edge, Node>
                            {
                                [new Edge(6)] = new Node(7)
                                {
                                    Children = new Dictionary<Edge, Node>
                                    {
                                        [new Edge(7)] = new Node(8),
                                    }
                                },
                            }
                        },
                    },
                },
                [new Edge(8)] = new Node(9),
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

    public static IEnumerable<KeyValuePair<Edge, Node>> EdgeIdBasedChildrenSorter(
        TreeTraversalVisit<Edge, Node> visit) =>
        visit.Node.Children.OrderBy(edgeAndNode => edgeAndNode.Key.Id);

    public static IEnumerable<KeyValuePair<Edge, Node>> EdgeIdDescBasedChildrenSorter(
        TreeTraversalVisit<Edge, Node> visit) =>
        visit.Node.Children.OrderByDescending(edgeAndNode => edgeAndNode.Key.Id);

    public static IEnumerable<KeyValuePair<Edge, Node>> EdgeIdMedianBasedChildrenSorter(
        TreeTraversalVisit<Edge, Node> visit) =>
        from i in TestUtilities.MedianGenerator(0, visit.Node.Children.Count - 1)
        let edgesAndNodesSortedById = visit.Node.Children.OrderBy(edgeAndNode => edgeAndNode.Key.Id)
        select edgesAndNodesSortedById.ElementAt(i);
}
