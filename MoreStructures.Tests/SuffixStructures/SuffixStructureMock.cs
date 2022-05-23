using MoreStructures.SuffixStructures;
using MoreStructures.SuffixStructures.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreStructures.Tests.SuffixStructures;

[ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
internal static class SuffixStructureMock
{
    public class Edge : ISuffixStructureEdge<Edge, Node>
    {
        public int Start => throw new NotImplementedException();
        public int Length => throw new NotImplementedException();
        public string Of(TextWithTerminator text) => throw new NotImplementedException();
    }

    public class Node : ISuffixStructureNode<Edge, Node>
    {
        public int? Start => throw new NotImplementedException();
        public IDictionary<Edge, Node> Children => throw new NotImplementedException();
    }

    public class Builder : IBuilder<Edge, Node>
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
