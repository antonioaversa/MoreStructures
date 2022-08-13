using MoreStructures.AdjacencyListGraphs;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Visitor;

[TestClass]
public class FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyListGraph : VisitStrategyTests
{
    public FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyListGraph() : base(
        (numberOfVertices, edges) => new AdjacencyListGraph(BuildNeighborhoods(numberOfVertices, edges)),
        directedGraph => new FullyRecursiveHashSetBasedGraphVisit(directedGraph))
    {
    }

    private static IList<ISet<int>> BuildNeighborhoods(int numberOfVertices, IList<(int start, int end)> edges)
    {
        var neighborhoods = new ISet<int>[numberOfVertices];
        for (var i = 0; i < numberOfVertices; i++)
        {
            neighborhoods[i] = new HashSet<int>();
        }

        foreach (var edge in edges)
        {
            neighborhoods[edge.start].Add(edge.end);
        }

        return neighborhoods;
    }
}
