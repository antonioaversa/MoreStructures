using StringAlgorithms.RecImmTrees;
using StringAlgorithms.Utilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StringAlgorithms.Tests.CountTrees;

[ExcludeFromCodeCoverage]
internal static class TreeMock
{
    public record Edge(int Id) : IRecImmDictIndexedTreeEdge<Edge, Node, Builder>
    {
    }

    public record Node(int Id) : IRecImmDictIndexedTreeNode<Edge, Node, Builder>
    {
        private ValueReadOnlyDictionary<Edge, Node> _children = new(new Dictionary<Edge, Node> { });
        public IDictionary<Edge, Node> Children 
        {
            get => _children;
            set => _children = new(new Dictionary<Edge, Node>(value));
        }
    }

    public class Builder : IRecImmDictIndexedTreeBuilder<Edge, Node, Builder>
    {
    }
}
