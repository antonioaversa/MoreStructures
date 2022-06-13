using MoreStructures.RecImmTrees.Visitor;
using System;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

internal class VisitCounter
{
    public int CountOfVisitedNodes { get; private set; }

    public Func<TreeTraversalVisit<TreeMock.Edge, TreeMock.Node>, int> Visitor { get; }

    public VisitCounter()
    {
        CountOfVisitedNodes = 0;
        Visitor = visit => CountOfVisitedNodes++;
    }

    public void Reset() => CountOfVisitedNodes = 0;
}