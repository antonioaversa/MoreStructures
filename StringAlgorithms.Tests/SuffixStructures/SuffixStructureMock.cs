using StringAlgorithms.SuffixStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StringAlgorithms.Tests.SuffixStructures;

[ExcludeFromCodeCoverage]
internal static class SuffixStructureMock
{
    public class Edge : ISuffixStructureEdge<Edge, Node, Path, Builder>
    {
        public int Start => throw new NotImplementedException();
        public int Length => throw new NotImplementedException();
        public string Of(TextWithTerminator text) => throw new NotImplementedException();
    }

    public class Node : ISuffixStructureNode<Edge, Node, Path, Builder>
    {
        public int? Start => throw new NotImplementedException();
        public IDictionary<Edge, Node> Children => throw new NotImplementedException();
    }

    public class Path : ISuffixStructurePath<Edge, Node, Path, Builder>
    {
        public IEnumerable<KeyValuePair<Edge, Node>> PathNodes => throw new NotImplementedException();
    }

    public class Builder : ISuffixStructureBuilder<Edge, Node, Path, Builder>
    {
        public bool BuildTreeCalled { get; private set; } = false;
        public TextWithTerminator? BuildTreeArgument { get; private set; } = null;

        public Node BuildTree(TextWithTerminator text)
        {
            BuildTreeCalled = true;
            BuildTreeArgument = text;
            return new Node();
        }

        public Path EmptyPath() =>
            throw new NotImplementedException();
        public Path MultistepsPath(params (Edge edge, Node node)[] pathNodes) =>
            throw new NotImplementedException();
        public Path MultistepsPath(IEnumerable<KeyValuePair<Edge, Node>> pathNodes) =>
            throw new NotImplementedException();
        public Path SingletonPath(Edge edge, Node node) =>
            throw new NotImplementedException();
    }
}
