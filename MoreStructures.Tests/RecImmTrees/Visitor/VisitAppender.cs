using MoreStructures.RecImmTrees.Visitor;
using System;
using System.Collections.Generic;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

internal class VisitAppender
{
    public IList<(int? edgeId, int nodeId)> Visits { get; }

    public Func<TreeTraversalVisit<TreeMock.Edge, TreeMock.Node>, int> Visitor { get; }

    public VisitAppender()
    {
        Visits = new List<(int? edgeId, int nodeId)> { };
        Visitor = visit => 
        {
            Visits.Add((visit.IncomingEdge?.Id, visit.Node.Id)); 
            return 0; 
        };
    }

    public void Clear() => Visits.Clear();
}
