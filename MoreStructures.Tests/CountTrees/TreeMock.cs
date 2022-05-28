using MoreStructures.RecImmTrees;
using MoreStructures.Utilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreStructures.Tests.CountTrees;

[ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
internal static class TreeMock
{
    public record Edge(int Id) : IRecImmDictIndexedTreeEdge<Edge, Node>
    {
    }

    public record Node(int Id) : IRecImmDictIndexedTreeNode<Edge, Node>
    {
        private ValueReadOnlyDictionary<Edge, Node> _children = new(new Dictionary<Edge, Node> { });
        public IDictionary<Edge, Node> Children 
        {
            get => _children;
            set => _children = new(new Dictionary<Edge, Node>(value));
        }
    }

    public static Node BuildMostUnbalancedTree(int numberOfIntermediateNodes)
    {
        Node root = new(numberOfIntermediateNodes);
        for (int i = numberOfIntermediateNodes; i >= 1; i--)
            root = new Node(i - 1)
            {
                Children = new Dictionary<Edge, Node>
                {
                    [new(i - 1)] = root,
                }
            };
        return root;
    }
}
