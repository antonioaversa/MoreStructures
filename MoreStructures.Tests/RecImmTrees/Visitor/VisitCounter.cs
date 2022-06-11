using MoreStructures.RecImmTrees.Visitor;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

internal class VisitCounter
{
    public int CountOfVisitedNodes { get; private set; }

    public Visitor<TreeMock.Node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node>> Visitor { get; }

    public VisitCounter()
    {
        CountOfVisitedNodes = 0;
        Visitor = (TreeMock.Node node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node> visitContext) =>
            CountOfVisitedNodes++;
    }

    public void Reset() => CountOfVisitedNodes = 0;
}