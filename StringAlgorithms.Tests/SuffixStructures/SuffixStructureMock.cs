using StringAlgorithms.SuffixStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StringAlgorithms.Tests.SuffixStructures;

[ExcludeFromCodeCoverage]
internal static class SuffixStructureMock
{
    public class Edge : ISuffixStructureEdge<Edge, Node, Builder>
    {
        public int Start => throw new NotImplementedException();
        public int Length => throw new NotImplementedException();
        public string Of(TextWithTerminator text) => throw new NotImplementedException();
    }

    public class Node : ISuffixStructureNode<Edge, Node, Builder>
    {
        public int? Start => throw new NotImplementedException();
        public IDictionary<Edge, Node> Children => throw new NotImplementedException();
    }

    public class Builder : ISuffixStructureBuilder<Edge, Node, Builder>
    {
        public bool BuildTreeCalled { get; private set; } = false;
        public TextWithTerminator? BuildTreeArgument { get; private set; } = null;

        public Node BuildTree(TextWithTerminator text)
        {
            BuildTreeCalled = true;
            BuildTreeArgument = text;
            return new Node();
        }
    }
}
