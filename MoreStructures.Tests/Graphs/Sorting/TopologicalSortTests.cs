using MoreStructures.Graphs;
using MoreStructures.Graphs.Sorting;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Sorting;

public abstract class TopologicalSortTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected ITopologicalSort Sorter { get; }

    protected TopologicalSortTests(Func<int, IList<(int, int)>, IGraph> graphBuilder, ITopologicalSort sorter)
    {
        Sorter = sorter;
        GraphBuilder = graphBuilder;
    }

    [DataRow("1V, no edges", 1, new int[] { }, new int[] { }, new[] { 0 })]
    [DataRow("2V, source to sink", 2, new int[] { 0 }, new int[] { 1 }, new[] { 0, 1 })]
    [DataRow("2V, sink from source", 2, new int[] { 1 }, new int[] { 0 }, new[] { 1, 0 })]
    [DataRow("4V, 1 4-chain", 4, new int[] { 0, 1, 2 }, new int[] { 1, 2, 3 }, new[] { 0, 1, 2, 3 })]
    [DataRow("5V, 1 source, 3 intermediate, 1 sink", 5, new int[] { 0, 0, 0, 1, 1, 1, 2, 3 },
        new int[] { 1, 2, 4, 2, 3, 4, 3, 4 }, new[] { 0, 1, 2, 3, 4 })]
    [DataTestMethod]
    public void Sort_IsCorrectWhenSingleSolutionIsPossible(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] expectedTopologicalSort)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var topologicalSort = Sorter.Sort(graph);
        Assert.IsTrue(
            topologicalSort.SequenceEqual(expectedTopologicalSort),
            $"{graphDescription} - Expected [{string.Join(", ", expectedTopologicalSort)}], " +
            $"Actual: [{string.Join(", ", topologicalSort)}]");
    }

    [DataRow("2V, no edges", 2, new int[] { }, new int[] { })]
    [DataRow("4V, 2 sources pointing each to 1 sink", 4, new int[] { 0, 1 }, new int[] { 3, 2 })]
    [DataRow("4V, 2 sources pointing both to 2 sinks", 4, new int[] { 0, 0, 1, 1 }, new int[] { 2, 3, 2, 3 })]
    [DataRow("4V, 1 source, 1 intermediate, 2 sinks", 4, new int[] { 0, 0, 0, 1, 1 }, new int[] { 1, 2, 3, 2, 3 })]
    [DataRow("5V, 2 sources, 2 intermediate, 1 sink", 5, new int[] { 0, 2, 3, 3, 4 },
        new int[] { 2, 4, 1, 2, 1 })]
    [DataRow("6V, 2 sources, 3 intermediate, 1 sink", 6, new int[] { 0, 1, 1, 2, 3, 3, 5, 5 }, 
        new[] { 1, 2, 3, 4, 2, 4, 3, 4 })]
    [DataTestMethod]
    public void Sort_IsCorrectWhenMultipleSolutionsArePossible(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var topologicalSort = Sorter.Sort(graph);
        var visitor = new FullyRecursiveHashSetBasedGraphVisit(true);
        for (var vertex1 = 0; vertex1 < numberOfVertices; vertex1++)
        {
            var ts1 = topologicalSort[vertex1];
            var verticesReachableFromVertex1 = visitor.DepthFirstSearchFromVertex(graph, vertex1).ToHashSet();

            for (var vertex2 = 0; vertex2 < numberOfVertices; vertex2++)
            {
                if (vertex1 != vertex2 && verticesReachableFromVertex1.Contains(vertex2))
                {
                    var ts2 = topologicalSort[vertex2];
                    Assert.IsTrue(
                        ts1 < ts2,
                        $"{graphDescription} - vertex {vertex1} ~~> vertex {vertex2}, " +
                        $"but TS[{vertex1}] >= TS[{vertex2}]: {ts1} >= {ts2}");
                }
            }
        }
    }

    [DataRow("2V, 1 2-C", 2, new int[] { 0, 1 }, new int[] { 1, 0 })]
    [DataRow("2V, 1 2-C inverted", 2, new int[] { 1, 0 }, new int[] { 0, 1 })]
    [DataRow("3V, 1 2-C", 3, new int[] { 0, 1 }, new int[] { 1, 0 })]
    [DataRow("3V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 })]
    [DataRow("4V, 1 4-C", 4, new int[] { 0, 1, 2, 3 }, new int[] { 1, 2, 3, 0 })]
    [DataRow("4V, 1 3-chain, 1 isolated", 4, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 })]
    [DataRow("5V, 1 source, 3 intermediate, 1 sink, 1 3-C", 5, new int[] { 0, 0, 0, 1, 1, 1, 2, 3, 3 },
        new int[] { 1, 2, 4, 2, 3, 4, 3, 1, 4 })]
    [DataTestMethod]
    public void Sort_RaisesExceptionWhenNoDag(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        Assert.ThrowsException<InvalidOperationException>(() => Sorter.Sort(graph), graphDescription);
    }
}
