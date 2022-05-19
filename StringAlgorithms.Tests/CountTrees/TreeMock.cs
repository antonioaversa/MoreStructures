using StringAlgorithms.RecImmTrees;
using StringAlgorithms.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace StringAlgorithms.Tests.CountTrees;

[ExcludeFromCodeCoverage]
internal static class TreeMock
{
    public record Edge(int Id) : IRecImmDictIndexedTreeEdge<Edge, Node, Path, Builder>
    {
    }

    public record Node(int Id) : IRecImmDictIndexedTreeNode<Edge, Node, Path, Builder>
    {
        private ValueReadOnlyDictionary<Edge, Node> _children = new(new Dictionary<Edge, Node> { });
        public IDictionary<Edge, Node> Children 
        {
            get => _children;
            set => _children = new(new Dictionary<Edge, Node>(value));
        }
    }

    public record Path() : IRecImmDictIndexedTreePath<Edge, Node, Path, Builder>
    {
        private ValueReadOnlyCollection<KeyValuePair<Edge, Node>> _pathNodes = 
            new(new List<KeyValuePair<Edge, Node>> { });
        public IEnumerable<KeyValuePair<Edge, Node>> PathNodes
        {
            get => _pathNodes;
            set => _pathNodes = new(new List<KeyValuePair<Edge, Node>>(value));
        }
    }

    public class Builder : IRecImmDictIndexedTreeBuilder<Edge, Node, Path, Builder>
    {
        public Path EmptyPath() => new();
        public Path MultistepsPath(params (Edge edge, Node node)[] pathNodes) => new()
        {
            PathNodes = pathNodes.Select(pn => KeyValuePair.Create(pn.edge, pn.node))
        };
        public Path MultistepsPath(IEnumerable<KeyValuePair<Edge, Node>> pathNodes) => new()
        {
            PathNodes = pathNodes
        };
        public Path SingletonPath(Edge edge, Node node) => new() 
        { 
            PathNodes = new List<KeyValuePair<Edge, Node>> { new(edge, node)}
        };
    }
}
