using MoreStructures.Graphs;

namespace MoreStructures.Tests.Graphs;

public abstract class GraphTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }

    protected GraphTests(Func<int, IList<(int, int)>, IGraph> graphBuilder)
    {
        GraphBuilder = graphBuilder;
    }

    [DataRow(1, new int[] { }, new int[] { })]
    [DataRow(2, new int[] { }, new int[] { })]
    [DataRow(2, new[] { 0 }, new[] { 1 })]
    [DataRow(2, new[] { 1 }, new[] { 0 })]
    [DataRow(2, new[] { 0, 0, 1 }, new[] { 0, 1, 1 })]
    [DataRow(3, new[] { 0, 0, 1 }, new[] { 1, 2, 2 })]
    [DataRow(5, new[] { 0, 0, 1, 2, 4 }, new[] { 1, 2, 4, 3, 3 })]
    [DataRow(9, new[] { 0, 0, 1, 2, 3, 4, 4, 5, 6, 8 }, new[] { 1, 2, 4, 3, 4, 5, 6, 8, 7, 7 })]
    [DataTestMethod]
    public void Reverse_IsCorrect(int numberOfVertices, int[] starts, int[] ends)
    {
        Test(true);
        Test(false);

        void Test(bool takeIntoAccountEdgeDirection)
        {
            var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
            var reverseGraph = graph.Reverse();

            // Vertices are the same
            Assert.AreEqual(graph.GetNumberOfVertices(), reverseGraph.GetNumberOfVertices());

            // Edges are reversed
            var neighbors = (
                from vertex in Enumerable.Range(0, numberOfVertices)
                select (
                    vertex,
                    neighbors:
                        graph.GetAdjacentVerticesAndEdges(vertex, takeIntoAccountEdgeDirection).ToHashSet(),
                    reverseNeighbors:
                        reverseGraph.GetAdjacentVerticesAndEdges(vertex, takeIntoAccountEdgeDirection).ToHashSet()))
                .ToDictionary(t => t.vertex);

            for (var vertex = 0; vertex < numberOfVertices; vertex++)
                foreach (var (neighborVertex, edgeStart, edgeEnd) in neighbors[vertex].neighbors)
                    Assert.IsTrue(
                        neighbors[neighborVertex].reverseNeighbors.Contains(new(vertex, edgeEnd, edgeStart)));
        }
    }

    [DataRow(1, new int[] { }, new int[] { })]
    [DataRow(2, new int[] { }, new int[] { })]
    [DataRow(2, new[] { 0 }, new[] { 1 })]
    [DataRow(2, new[] { 1 }, new[] { 0 })]
    [DataRow(2, new[] { 0, 0, 1 }, new[] { 0, 1, 1 })]
    [DataRow(3, new[] { 0, 0, 1 }, new[] { 1, 2, 2 })]
    [DataRow(5, new[] { 0, 0, 1, 2, 4 }, new[] { 1, 2, 4, 3, 3 })]
    [DataRow(9, new[] { 0, 0, 1, 2, 3, 4, 4, 5, 6, 8 }, new[] { 1, 2, 4, 3, 4, 5, 6, 8, 7, 7 })]
    [DataTestMethod]
    public void Reverse_OfReverseIsInitialGraph(int numberOfVertices, int[] starts, int[] ends)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var reverseReverseGraph = graph.Reverse().Reverse();

        // Vertices are the same
        Assert.AreEqual(graph.GetNumberOfVertices(), reverseReverseGraph.GetNumberOfVertices());

        // Adjacencies are the same
        for (var vertex = 0; vertex < numberOfVertices; vertex++)
        {
            var vertexNeighborhood1 = graph.GetAdjacentVerticesAndEdges(vertex, true).ToHashSet();
            var vertexNeighborhood2 = reverseReverseGraph.GetAdjacentVerticesAndEdges(vertex, true).ToHashSet();
            Assert.IsTrue(vertexNeighborhood1.SetEquals(vertexNeighborhood2));
        }
    }
}
