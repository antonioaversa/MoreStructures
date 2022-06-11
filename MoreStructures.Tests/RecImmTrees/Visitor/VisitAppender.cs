using MoreStructures.RecImmTrees.Visitor;
using System.Collections.Generic;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

internal class VisitAppender
{
    public IList<(int? edgeId, int nodeId)> Visits { get; }

    public Visitor<TreeMock.Node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node>> Visitor { get; }

    public VisitAppender()
    {
        Visits = new List<(int? edgeId, int nodeId)> { };
        Visitor = (TreeMock.Node node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node> visitContext) =>
            Visits.Add((visitContext.IncomingEdge?.Id, node.Id));
    }

    public void Clear() => Visits.Clear();
}
