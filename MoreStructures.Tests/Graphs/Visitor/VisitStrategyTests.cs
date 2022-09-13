using MoreStructures.Graphs;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Visitor;

public abstract class VisitStrategyTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected Func<bool, IVisitStrategy> VisitorBuilder { get; }

    protected VisitStrategyTests(
        Func<int, IList<(int, int)>, IGraph> graphBuilder,
        Func<bool, IVisitStrategy> visitorBuilder)
    {
        GraphBuilder = graphBuilder;
        VisitorBuilder = visitorBuilder;
    }

    // Legend: V = Vertex, E = Edge, L = Loop, n-C = n-Cycle

    // Loops and Cycles
    [DataRow("1 V, 0 E", 1, new int[] { }, new int[] { }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("1 V, 1 L", 1, new int[] { 0 }, new int[] { 0 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("1 V, 2 L", 1, new int[] { 0, 0 }, new int[] { 0, 0 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("2 V, 0 E", 2, new int[] { }, new int[] { }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("2 V, 0 E", 2, new int[] { }, new int[] { }, 1,
        new int[] { 1 }, new int[] { 1 })]
    [DataRow("2 V, each 1 L", 2, new int[] { 0, 1 }, new int[] { 0, 1 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("2 V, each 1 L", 2, new int[] { 0, 1 }, new int[] { 0, 1 }, 1,
        new int[] { 1 }, new int[] { 1 })]
    [DataRow("2 V, 1 2-C", 2, new int[] { 0, 1 }, new int[] { 1, 0 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 2-C", 2, new int[] { 0, 1 }, new int[] { 1, 0 }, 1,
        new int[] { 1, 0 }, new int[] { 1, 0 })]
    [DataRow("2 V, 1 inverted 2-C", 2, new int[] { 1, 0 }, new int[] { 0, 1 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 inverted 2-C", 2, new int[] { 1, 0 }, new int[] { 0, 1 }, 1,
        new int[] { 1, 0 }, new int[] { 1, 0 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 1,
        new int[] { 1, 2 }, new int[] { 1, 2 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 2,
        new int[] { 2, 1 }, new int[] { 2, 1 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 1,
        new int[] { 1, 2, 0 }, new int[] { 1, 0, 2 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 2,
        new int[] { 2, 0, 1 }, new int[] { 2, 0, 1 })]
    [DataRow("3 V, 3 2-C", 3, new int[] { 0, 0, 1, 1, 2, 2 }, new int[] { 1, 2, 0, 2, 0, 1 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 1,
        new int[] { 1, 0, 2 }, new int[] { 1, 0, 2 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 2,
        new int[] { 2, 0, 1 }, new int[] { 2, 0, 1 })]

    // Sink
    [DataRow("2 V, 1 pointing to global sink", 2, new int[] { 0 }, new int[] { 1 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 pointing to global sink", 2, new int[] { 0 }, new int[] { 1 }, 1,
        new int[] { 1 }, new int[] { 1, 0 })]
    [DataRow("3 V, 2 pointing to global sink", 3, new int[] { 0, 2 }, new int[] { 1, 1 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 2 pointing to global sink", 3, new int[] { 0, 2 }, new int[] { 1, 1 }, 1,
        new int[] { 1 }, new int[] { 1, 0, 2 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 4, new int[] { 0, 1, 2, 3 },
        new int[] { 1, 3, 1, 2 }, 0,
        new int[] { 0, 1, 3, 2 }, new int[] { 0, 1, 2, 3 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 4, new int[] { 0, 1, 2, 3 },
        new int[] { 1, 3, 1, 2 }, 1,
        new int[] { 1, 3, 2 }, new int[] { 1, 0, 2, 3 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 4, new int[] { 0, 1, 2, 3 },
        new int[] { 1, 3, 1, 2 }, 2,
        new int[] { 2, 1, 3 }, new int[] { 2, 1, 0, 3 })]
    [DataTestMethod]
    public void DepthFirstSearchFromVertex_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int start,
        int[] expectedDirectedGraphResult, int[] expectedUndirectedGraphResult)
    {
        TestXFirstSearchMethod(
            nameof(IVisitStrategy.DepthFirstSearchFromVertex),
            (visitor, graph, start) => visitor.DepthFirstSearchFromVertex(graph, start),
            graphDescription, numberOfVertices, starts, ends, start,
            expectedDirectedGraphResult, expectedUndirectedGraphResult);
    }

    // Loops and Cycles
    [DataRow("1 V, 0 E", 1, new int[] { }, new int[] { }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("1 V, 1 L", 1, new int[] { 0 }, new int[] { 0 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("1 V, 2 L", 1, new int[] { 0, 0 }, new int[] { 0, 0 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("2 V, 0 E", 2, new int[] { }, new int[] { }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("2 V, 0 E", 2, new int[] { }, new int[] { }, 1,
        new int[] { 1 }, new int[] { 1 })]
    [DataRow("2 V, each 1 L", 2, new int[] { 0, 1 }, new int[] { 0, 1 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("2 V, each 1 L", 2, new int[] { 0, 1 }, new int[] { 0, 1 }, 1,
        new int[] { 1 }, new int[] { 1 })]
    [DataRow("2 V, 1 2-C", 2, new int[] { 0, 1 }, new int[] { 1, 0 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 2-C", 2, new int[] { 0, 1 }, new int[] { 1, 0 }, 1,
        new int[] { 1, 0 }, new int[] { 1, 0 })]
    [DataRow("2 V, 1 inverted 2-C", 2, new int[] { 1, 0 }, new int[] { 0, 1 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 inverted 2-C", 2, new int[] { 1, 0 }, new int[] { 0, 1 }, 1,
        new int[] { 1, 0 }, new int[] { 1, 0 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 1,
        new int[] { 1, 2 }, new int[] { 1, 2 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 2,
        new int[] { 2, 1 }, new int[] { 2, 1 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 1,
        new int[] { 1, 2, 0 }, new int[] { 1, 0, 2 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 2,
        new int[] { 2, 0, 1 }, new int[] { 2, 0, 1 })]
    [DataRow("3 V, 3 2-C", 3, new int[] { 0, 0, 1, 1, 2, 2 }, new int[] { 1, 2, 0, 2, 0, 1 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 1,
        new int[] { 1, 0, 2 }, new int[] { 1, 0, 2 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 2,
        new int[] { 2, 0, 1 }, new int[] { 2, 0, 1 })]

    // Sink
    [DataRow("2 V, 1 pointing to global sink", 2, new int[] { 0 }, new int[] { 1 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 pointing to global sink", 2, new int[] { 0 }, new int[] { 1 }, 1,
        new int[] { 1 }, new int[] { 1, 0 })]
    [DataRow("3 V, 2 pointing to global sink", 3, new int[] { 0, 2 }, new int[] { 1, 1 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 2 pointing to global sink", 3, new int[] { 0, 2 }, new int[] { 1, 1 }, 1,
        new int[] { 1 }, new int[] { 1, 0, 2 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back",
        4, new int[] { 0, 1, 2, 3 }, new int[] { 1, 3, 1, 2 }, 0,
        new int[] { 0, 1, 3, 2 }, new int[] { 0, 1, 2, 3 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back",
        4, new int[] { 0, 1, 2, 3 }, new int[] { 1, 3, 1, 2 }, 1,
        new int[] { 1, 3, 2 }, new int[] { 1, 0, 2, 3 })]

    // BSF different from DFS
    [DataRow("4 V, 2 pointing to 2-chain jumping back",
        4, new int[] { 0, 1, 2, 3 }, new int[] { 1, 3, 1, 2 }, 2,
        new int[] { 2, 1, 3 }, new int[] { 2, 1, 3, 0 })]
    [DataRow("4 V, source pointing to leaf and 2-chain",
        4, new int[] { 0, 1, 0 }, new int[] { 1, 2, 3 }, 0,
        new int[] { 0, 1, 3, 2 }, new int[] { 0, 1, 3, 2 })]
    [DataRow("4 V, source pointing to leaf and 2-chain",
        4, new int[] { 0, 1, 3 }, new int[] { 1, 2, 0 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 3, 2 })]
    [DataRow("6 V, 1 source, 4 intermediate, 1 sink",
        6, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 3, 4 }, new int[] { 1, 2, 5, 2, 3, 4, 4, 5, 4, 5 }, 0,
        new int[] { 0, 1, 2, 5, 3, 4 }, new int[] { 0, 1, 2, 5, 3, 4 })]
    [DataRow("7 V, 1 3-C, 1 2-chain, 1 2-chain sharing leaf with 1-chain",
        7, new int[] { 0, 0, 0, 0, 1, 3, 4, 5 }, new int[] { 1, 2, 3, 5, 2, 4, 0, 6 }, 0,
        new int[] { 0, 1, 2, 3, 5, 4, 6 }, new int[] { 0, 1, 2, 3, 4, 5, 6 })]
    [DataTestMethod]
    public void BreadthSearchFromVertex_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int start,
        int[] expectedDirectedGraphResult, int[] expectedUndirectedGraphResult)
    {
        TestXFirstSearchMethod(
            nameof(IVisitStrategy.BreadthFirstSearchFromVertex),
            (visitor, graph, start) => visitor.BreadthFirstSearchFromVertex(graph, start),
            graphDescription, numberOfVertices, starts, ends, start,
            expectedDirectedGraphResult, expectedUndirectedGraphResult);
    }

    private void TestXFirstSearchMethod(
        string methodName, Func<IVisitStrategy, IGraph, int, IEnumerable<int>> visitStrategyAction,
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int start,
        int[] expectedDirectedGraphResult, int[] expectedUndirectedGraphResult)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var directedGraphVisitor = VisitorBuilder(true);
        var directedGraphResult = visitStrategyAction(directedGraphVisitor, graph, start).ToList();
        Assert.IsTrue(
            directedGraphResult.SequenceEqual(expectedDirectedGraphResult),
            $"Failed {methodName} of {graphDescription} as directed graph: " +
            $"expected [{string.Join(", ", expectedDirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", directedGraphResult)}]");

        var undirectedGraphVisitor = VisitorBuilder(false);
        var undirectedGraphResult = visitStrategyAction(undirectedGraphVisitor, graph, start).ToList();
        Assert.IsTrue(
            undirectedGraphResult.SequenceEqual(expectedUndirectedGraphResult),
            $"Failed {methodName} of {graphDescription} as undirected graph: " +
            $"expected [{string.Join(", ", expectedUndirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", undirectedGraphResult)}]");
    }

    [DataRow("4 V, source to 1 sink and 1 intermediate to sink", 4, new[] { 0, 0, 1 }, new[] { 1, 2, 3 },
        new int[] { 0, 1, 3, 2 }, new[] { 0, 1, 3, 2 })]
    [DataRow("5 V, 3 isolated, 1 source to sink", 5, new[] { 1 }, new[] { 3 },
        new int[] { 0, 1, 3, 2, 4 }, new[] { 0, 1, 3, 2, 4 })]
    [DataRow("5 V, 3 isolated, 1 source to sink inverted", 5, new[] { 3 }, new[] { 1 },
        new int[] { 0, 1, 2, 3, 4 }, new[] { 0, 1, 3, 2, 4 })]
    [DataRow("6 V, 1 isolated, 1 source to sink, 1 3-C", 6, new[] { 2, 3, 4, 5 }, new[] { 1, 4, 5, 3 },
        new int[] { 0, 1, 2, 3, 4, 5 }, new[] { 0, 1, 2, 3, 4, 5 })]
    [DataRow("5 V, 1 isolated, 1 source to sink, 1 3-C inverted", 6, new int[] { 2, 3, 4, 5 }, new[] { 1, 5, 3, 4 },
        new int[] { 0, 1, 2, 3, 5, 4 }, new[] { 0, 1, 2, 3, 4, 5 })]
    [DataRow("5 V, 2-roots dag with central vertex", 5, new[] { 3, 1, 2, 2 }, new[] { 2, 2, 0, 4 },
        new int[] { 0, 1, 2, 4, 3 }, new[] { 0, 2, 1, 3, 4 })]
    [DataRow("7 V, tree, 1 2-chain, 1 3-chain, 1 4-chain", 7, new[] { 0, 0, 0, 1, 2, 4 }, new[] { 2, 1, 5, 3, 4, 6 },
        new int[] { 0, 1, 3, 2, 4, 6, 5 }, new[] { 0, 1, 3, 2, 4, 6, 5 })]
    [DataRow("7 V, quasi-tree, 1 2-chain, 1 3-chain, 1 4-chain", 7, new[] { 0, 0, 0, 1, 2, 4 },
        new[] { 2, 3, 5, 3, 4, 6 },
        new int[] { 0, 2, 4, 6, 3, 5, 1 }, new[] { 0, 2, 4, 6, 3, 1, 5 })]
    [DataTestMethod]
    public void DepthFirstSearchOfGraph_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends,
        int[] expectedDirectedGraphResult, int[] expectedUndirectedGraphResult)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var directedGraphVisitor = VisitorBuilder(true);
        var directedGraphResult = directedGraphVisitor.DepthFirstSearchOfGraph(graph);
        Assert.IsTrue(
            directedGraphResult.SequenceEqual(expectedDirectedGraphResult),
            $"Failed {nameof(IVisitStrategy.DepthFirstSearchOfGraph)} of {graphDescription} as directed graph: " +
            $"expected [{string.Join(", ", expectedDirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", directedGraphResult)}]");

        var undirectedGraphVisitor = VisitorBuilder(false);
        var undirectedGraphResult = undirectedGraphVisitor.DepthFirstSearchOfGraph(graph);
        Assert.IsTrue(
            undirectedGraphResult.SequenceEqual(expectedUndirectedGraphResult),
            $"Failed {nameof(IVisitStrategy.DepthFirstSearchOfGraph)} of {graphDescription} as undirected graph: " +
            $"expected [{string.Join(", ", expectedUndirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", undirectedGraphResult)}]");
    }

    [DataRow("1 V, 1 isolated", 1, new int[] { }, new int[] { },
        new int[] { 0 })]
    [DataRow("2 V, 2 isolated", 2, new int[] { }, new int[] { },
        new int[] { 0, 1 })]
    [DataRow("2 V, 2 isolated", 2, new int[] { 0, 1 }, new int[] { 0, 1 },
        new int[] { 0, 1 })]
    [DataRow("2 V, 1 2-C", 2, new int[] { 0, 1 }, new int[] { 1, 0 },
        new int[] { 0, 0 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 },
        new int[] { 0, 0, 0 })]
    [DataRow("3 V, 1 2-C, 1 isolated", 3, new int[] { 0, 1 }, new int[] { 1, 0 },
        new int[] { 0, 0, 1 })]
    [DataRow("3 V, 1 2-C, 1 isolated with loop", 3, new int[] { 0, 1, 2 }, new int[] { 1, 0, 2 },
        new int[] { 0, 0, 1 })]
    [DataRow("3 V, 1 isolated, 1 2-C", 3, new int[] { 1, 2 }, new int[] { 2, 1 },
        new int[] { 0, 1, 1 })]
    [DataTestMethod]
    public void ConnectedComponents_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends,
        int[] expectedUndirectedGraphResult)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var undirectedGraphVisitor = VisitorBuilder(false);
        var undirectedGraphResult = undirectedGraphVisitor.ConnectedComponents(graph);
        Assert.IsTrue(
            undirectedGraphResult
                .Select(kvp => (kvp.Value, kvp.Key))
                .ToHashSet()
                .SetEquals(expectedUndirectedGraphResult.Zip(Enumerable.Range(0, int.MaxValue))),
            $"Failed {nameof(IVisitStrategy.ConnectedComponents)} of {graphDescription} as undirected graph: " +
            $"expected [{string.Join(", ", expectedUndirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", undirectedGraphResult.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value))}]");
    }

    [TestMethod]
    public void VisitEvents_BasicChecksInDepthFirstSearch()
    {
        TestVisitingEventsForMethod(
            (graph, visitor) => MoreLinq.MoreEnumerable.Consume(visitor.DepthFirstSearchOfGraph(graph)));
    }

    [TestMethod]
    public void VisitEvents_BasicChecksInConnectedComponents()
    {
        TestVisitingEventsForMethod(
            (graph, visitor) => MoreLinq.MoreEnumerable.Consume(visitor.ConnectedComponents(graph)));
    }

    [TestMethod]
    public void VisitEvents_BasicChecksInVisit()
    {
        TestVisitingEventsForMethod(
            (graph, visitor) => MoreLinq.MoreEnumerable.Consume(visitor.DepthFirstSearchFromVertex(graph, 0)));
    }

    [DataRow("2V, source to sink", 2, new[] { 0 }, new[] { 1 })]
    [DataRow("2V, sink to source", 2, new[] { 1 }, new[] { 0 })]
    [DataRow("3V, source to intermediate to sink", 3, new[] { 0, 1 }, new[] { 1, 2 })]
    [DataRow("4V, 2 sources to 1 intermediate to 1 sink", 4, new[] { 3, 0, 1 }, new[] { 1, 1, 2 })]
    [DataRow("4V, 1 source to 2 intermediate to 1 sink", 4, new[] { 0, 0, 1, 2 }, new[] { 1, 2, 3, 3 })]
    [DataRow("4V, 2 3-C overlapping on 2 vertices", 4, new[] { 0, 1, 2, 0, 3, 1 }, new[] { 1, 2, 0, 3, 1, 0 })]
    [DataTestMethod]
    public void VisitingVertex_InDepthFirstSearchOfGraphIsAccordingToOutput(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var visitStrategy = VisitorBuilder(true);

        var preOrder = new List<int>();
        visitStrategy.VisitingVertex += (o, e) => preOrder.Add(e.Vertex);

        var visitOutput = visitStrategy.DepthFirstSearchOfGraph(graph).ToList();
        Assert.IsTrue(
            visitOutput.SequenceEqual(preOrder),
            $"{graphDescription} - Expected: [{string.Join(", ", visitOutput)}], " +
            $"Actual: [{string.Join(", ", preOrder)}]");
    }

    [DataRow("1V, 1-L", 1, new[] { 0 }, new[] { 0 },
        new[] { 0 }, new[] { 1 }, new[] { -1 })]
    [DataRow("2V, source to sink", 2, new[] { 0 }, new[] { 1 },
        new[] { 0, 1 }, new[] { 3, 2 }, new[] { -1, 0 })]
    [DataRow("2V, sink from source", 2, new[] { 1 }, new[] { 0 },
        new[] { 0, 2 }, new[] { 1, 3 }, new[] { -1, -1 })]
    [DataRow("3V, source to intermediate to sink", 3, new[] { 0, 1 }, new[] { 1, 2 },
        new[] { 0, 1, 2 }, new[] { 5, 4, 3 }, new[] { -1, 0, 1 })]
    [DataRow("4V, 2 sources to 1 intermediate to 1 sink", 4, new[] { 3, 0, 1 }, new[] { 1, 1, 2 },
        new[] { 0, 1, 2, 6 }, new[] { 5, 4, 3, 7 }, new[] { -1, 0, 1, -1 })]
    [DataRow("4V, 1 source to 2 intermediate to 1 sink", 4, new[] { 0, 0, 1, 2 }, new[] { 1, 2, 3, 3 },
        new[] { 0, 1, 5, 2 }, new[] { 7, 4, 6, 3 }, new[] { -1, 0, 1, 0 })]
    [DataRow("4V, 2 3-C overlapping on 2 vertices", 4, new[] { 0, 1, 2, 0, 3, 1 }, new[] { 1, 2, 0, 3, 1, 0 },
        new[] { 0, 1, 2, 5 }, new[] { 7, 4, 3, 6 }, new[] { -1, 0, 1, 0 })]
    [DataTestMethod]
    public void VisitingAndVisitedVertex_InDepthFirstSearchOfGraph_AreCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends,
        int[] expectedPreVisitValues, int[] expectedPostVisitValues,
        int[] expectedPreviousVertices)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var visitStrategy = VisitorBuilder(true);

        var preVisitValues = new int[numberOfVertices];
        var preVisitConnectedComponents = new Dictionary<int, int>();
        var preVisitPreviousVertices = new List<int>();
        var postVisitValues = new int[numberOfVertices];
        var postVisitConnectedComponents = new Dictionary<int, int>();
        var postVisitPreviousVertices = new List<int>();

        var counter = 0;
        visitStrategy.VisitingVertex += (o, e) =>
        {
            preVisitValues[e.Vertex] = counter++;
            preVisitConnectedComponents[e.Vertex] = e.ConnectedComponent;
            preVisitPreviousVertices.Add(e.PreviousVertex ?? -1);
        };
        visitStrategy.VisitedVertex += (o, e) =>
        {
            postVisitValues[e.Vertex] = counter++;
            postVisitConnectedComponents[e.Vertex] = e.ConnectedComponent;
            postVisitPreviousVertices.Add(e.PreviousVertex ?? -1);
        };

        var visitOutput = visitStrategy.DepthFirstSearchOfGraph(graph).ToList();
        Assert.IsTrue(
            expectedPreVisitValues.SequenceEqual(preVisitValues),
            $"{graphDescription} - Expected: [{string.Join(", ", expectedPreVisitValues)}], " +
            $"Actual: [{string.Join(", ", preVisitValues)}]");
        Assert.IsTrue(
            expectedPostVisitValues.SequenceEqual(postVisitValues),
            $"{graphDescription} - Expected: [{string.Join(", ", expectedPostVisitValues)}], " +
            $"Actual: [{string.Join(", ", postVisitValues)}]");

        // Check connected components equality between VisitingVertex and VisitedVertex
        Assert.IsTrue(preVisitConnectedComponents.All(kvp =>
            postVisitConnectedComponents.ContainsKey(kvp.Key) && postVisitConnectedComponents[kvp.Key] == kvp.Value));

        // Check that previousVertices are the expected and the same in VisitingVertex and VisitedVertex
        Assert.IsTrue(
            preVisitPreviousVertices.SequenceEqual(expectedPreviousVertices),
            $"{graphDescription} - Expected: [{string.Join(", ", preVisitPreviousVertices)}], " +
            $"Actual: [{string.Join(", ", expectedPreviousVertices)}]");
        Assert.IsTrue(
            preVisitPreviousVertices.ToHashSet().SetEquals(postVisitPreviousVertices.ToHashSet()),
            $"{graphDescription} - Expected: [{string.Join(", ", preVisitPreviousVertices)}], " +
            $"Actual: [{string.Join(", ", postVisitPreviousVertices)}]");
    }

    [DataRow("1V, 1-L", 1, new[] { 0 }, new[] { 0 },
        new[] { 0 }, new[] { 0 }, new int[] { 0 })]
    [DataRow("2V, 1 2-C", 2, new[] { 0, 1 }, new[] { 1, 0 },
        new[] { 0 }, new[] { 0 }, new[] { 1 })]
    [DataRow("3V, 1 3-C", 3, new[] { 0, 1, 2 }, new[] { 1, 2, 0 },
        new[] { 0 }, new[] { 0 }, new[] { 2 })]
    [DataRow("3V, 1 3-C, 1 2-C", 3, new[] { 0, 0, 1, 2 }, new[] { 1, 2, 0, 1 },
        new[] { 0, 1 }, new[] { 0, 0 }, new[] { 1, 2 })]
    [DataRow("5V, 1 5-C, 1 4-C, 1 3-C, 1 2-C", 5, new[] { 0, 1, 2, 3, 3, 3, 4, 4 }, new[] { 2, 3, 1, 1, 2, 4, 0, 2 },
        new[] { 1, 2, 0, 2  }, new[] { 0, 0, 0, 0 }, new[] { 3, 3, 4, 4 })]
    [DataTestMethod]
    public void AlreadyVisitedVertex_InDepthFirstSearchOfGraph_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends,
        int[] expectedAlreadyVisitedVertices, int[] expectedAlreadyVisitedConnectedComponents, 
        int[] expectedPreviousVertices)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var visitStrategy = VisitorBuilder(true);
        var alreadyVisitedVertexArgs = new List<VisitEventArgs>();

        visitStrategy.AlreadyVisitedVertex += (o, e) => alreadyVisitedVertexArgs.Add(e);

        MoreLinq.MoreEnumerable.Consume(visitStrategy.DepthFirstSearchOfGraph(graph));
        var expectedAlreadyVisitedVertexArgs = expectedAlreadyVisitedVertices
            .Zip(expectedAlreadyVisitedConnectedComponents)
            .Zip(expectedPreviousVertices.Select(v => v >= 0 ? v : null as int?))
            .Select(p => new VisitEventArgs(p.First.First, p.First.Second, p.Second))
            .ToList();

        Assert.IsTrue(
            expectedAlreadyVisitedVertexArgs.SequenceEqual(alreadyVisitedVertexArgs),
            $"{graphDescription} - Expected: {string.Join(", ", expectedAlreadyVisitedVertexArgs)}, " + 
            $"Actual: {string.Join(", " , alreadyVisitedVertexArgs)}");
    }

    private enum JournalEventType { Visiting, Visited };

    private void TestVisitingEventsForMethod(Action<IGraph, IVisitStrategy> visitStrategyAction)
    {
        var graph = GraphBuilder(3, Enumerable.Empty<(int, int)>().ToList());
        var directedGraphVisitor = VisitorBuilder(true);
        var visitingVertexEventInvoked = new Dictionary<int, int>();
        var visitedVertexEventInvoked = new Dictionary<int, int>();
        var currentClock = 0;
        var journal = new Dictionary<(JournalEventType journalEventType, int vertex), int>();

        directedGraphVisitor.VisitingVertex += (s, e) =>
        {
            visitingVertexEventInvoked[e.Vertex] =
                visitingVertexEventInvoked.TryGetValue(e.Vertex, out var count) ? count + 1 : 1;
            journal[(JournalEventType.Visiting, e.Vertex)] = currentClock++;
        };

        directedGraphVisitor.VisitedVertex += (s, e) =>
        {
            visitedVertexEventInvoked[e.Vertex] =
                visitedVertexEventInvoked.TryGetValue(e.Vertex, out var count) ? count + 1 : 1;
            journal[(JournalEventType.Visited, e.Vertex)] = currentClock++;
        };

        // Events are not invoked before the action is invoked
        Assert.IsFalse(visitingVertexEventInvoked.Any());
        Assert.IsFalse(visitedVertexEventInvoked.Any());

        visitStrategyAction(graph, directedGraphVisitor);

        // Events were invoked during action execution
        Assert.IsTrue(visitingVertexEventInvoked.All(kvp => kvp.Value == 1));

        Assert.IsTrue(visitedVertexEventInvoked.All(kvp => kvp.Value == 1));

        // Pre-post-visit intervals don't partially overlap: they are either disjoint or fully contained
        var verticesExplored = journal.Select(kvp => kvp.Key.vertex).Distinct().ToList();
        foreach (var v1 in verticesExplored)
        {
            Assert.IsTrue(journal[(JournalEventType.Visiting, v1)] < journal[(JournalEventType.Visited, v1)]);
            Assert.IsTrue((
                from v2 in verticesExplored.Except(new[] { v1 } )
                let v1s = journal[(JournalEventType.Visiting, v1)]
                let v1e = journal[(JournalEventType.Visited, v1)]
                let v2s = journal[(JournalEventType.Visiting, v2)]
                let v2e = journal[(JournalEventType.Visited, v2)]
                select v2s > v1e || v1s > v2e || (v1s < v2s && v2e < v1e) || (v2s < v1s && v1e < v2e))
                .All(b => b));
        }
    }
}

