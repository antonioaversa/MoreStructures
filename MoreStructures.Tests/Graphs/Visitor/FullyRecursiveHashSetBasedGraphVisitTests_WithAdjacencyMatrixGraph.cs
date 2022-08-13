using MoreStructures.AdjacencyMatrixGraphs;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Visitor;

[TestClass]
public class FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyMatrixGraph : VisitStrategyTests
{
    public FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyMatrixGraph() : base(
        (numberOfVertices, edges) => new AdjacencyMatrixGraph(BuildMatrix(numberOfVertices, edges)),
        directedGraph => new FullyRecursiveHashSetBasedGraphVisit(directedGraph))
    {
    }

    private static bool[,] BuildMatrix(int numberOfVertices, IList<(int start, int end)> edges)
    {
        var matrix = new bool[numberOfVertices, numberOfVertices];

        foreach (var (start, end) in edges)
        {
            matrix[start, end] = true;
        }

        return matrix;
    }
}