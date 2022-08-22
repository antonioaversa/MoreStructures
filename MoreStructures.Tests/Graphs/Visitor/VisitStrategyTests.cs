using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [DataRow("1 V, 0 E", 1, new int[] { }, new int[] { },  0,
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
        new int[] { 1, 0 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 inverted 2-C", 2, new int[] { 1, 0 }, new int[] { 0, 1 }, 0,
        new int[] { 0, 1 }, new int[] { 0, 1 })]
    [DataRow("2 V, 1 inverted 2-C", 2, new int[] { 1, 0 }, new int[] { 0, 1 }, 1,
        new int[] { 1, 0 }, new int[] { 0, 1 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 0,
        new int[] { 0 }, new int[] { 0 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 1,
        new int[] { 1, 2 }, new int[] { 1, 2 })]
    [DataRow("3 V, 1 L, 1 2-C", 3, new int[] { 0, 1, 2 }, new int[] { 0, 2, 1 }, 2,
        new int[] { 2, 1 }, new int[] { 2, 1 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 1,
        new int[] { 1, 2, 0 }, new int[] { 1, 2, 0 })]
    [DataRow("3 V, 1 3-C", 3, new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, 2,
        new int[] { 2, 0, 1 }, new int[] { 2, 0, 1 })]
    [DataRow("3 V, 3 2-C", 3, new int[] { 0, 0, 1, 1, 2, 2 }, new int[] { 1, 2, 0, 2, 0, 1 }, 0,
        new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 }, 
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 0, 
        new int[] { 1, 2, 0 }, new int[] { 1, 2, 0 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 1,
        new int[] { 2, 0, 1 }, new int[] { 2, 0, 1 })]
    [DataRow("3 V, 3 L, 3 2-C", 3, new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
        new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, 2,
        new int[] { 1, 0, 2 }, new int[] { 1, 0, 2 })]

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
        new int[] { 0, 1, 3, 2 }, new int[] { 0, 1, 3, 2 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 4, new int[] { 0, 1, 2, 3 },
        new int[] { 1, 3, 1, 2 }, 1,
        new int[] { 1, 3, 2 }, new int[] { 1, 0, 2, 3 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 4, new int[] { 0, 1, 2, 3 },
        new int[] { 1, 3, 1, 2 }, 2,
        new int[] { 2, 1, 3 }, new int[] { 2, 1, 0, 3 })]
    [DataTestMethod]
    public void Visit_IsCorrect(
        string graphDescription,int numberOfVertices, int[] starts, int[] ends, int start,
        int[] expectedDirectedGraphResult, int[] expectedUndirectedGraphResult)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var directedGraphVisitor = VisitorBuilder(true);
        var directedGraphResult = directedGraphVisitor.Visit(graph, start);
        Assert.IsTrue(
            directedGraphResult.ToHashSet().SetEquals(expectedDirectedGraphResult.ToHashSet()), 
            $"Failed {nameof(IVisitStrategy.Visit)} of {graphDescription} as directed graph: " +
            $"expected [{string.Join(", ", expectedDirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", directedGraphResult)}]");

        var undirectedGraphVisitor = VisitorBuilder(false);
        var undirectedGraphResult = undirectedGraphVisitor.Visit(graph, start);
        Assert.IsTrue(
            undirectedGraphResult.ToHashSet().SetEquals(expectedUndirectedGraphResult.ToHashSet()),
            $"Failed {nameof(IVisitStrategy.Visit)} of {graphDescription} as undirected graph: " +
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
    public void DepthFirstSearch_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends,
        int[] expectedDirectedGraphResult, int[] expectedUndirectedGraphResult)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var directedGraphVisitor = VisitorBuilder(true);
        var directedGraphResult = directedGraphVisitor.DepthFirstSearch(graph);
        Assert.IsTrue(
            directedGraphResult.SequenceEqual(expectedDirectedGraphResult),
            $"Failed {nameof(IVisitStrategy.DepthFirstSearch)} of {graphDescription} as directed graph: " +
            $"expected [{string.Join(", ", expectedDirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", directedGraphResult)}]");

        var undirectedGraphVisitor = VisitorBuilder(false);
        var undirectedGraphResult = undirectedGraphVisitor.DepthFirstSearch(graph);
        Assert.IsTrue(
            undirectedGraphResult.SequenceEqual(expectedUndirectedGraphResult),
            $"Failed {nameof(IVisitStrategy.DepthFirstSearch)} of {graphDescription} as undirected graph: " +
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
    public void VisitEvents_InDepthFirstSearch()
    {
        TestVisitingEventsForMethod(
            (graph, visitor) => MoreLinq.MoreEnumerable.Consume(visitor.DepthFirstSearch(graph)));
    }

    [TestMethod]
    public void VisitEvents_InConnectedComponents()
    {
        TestVisitingEventsForMethod(
            (graph, visitor) => MoreLinq.MoreEnumerable.Consume(visitor.ConnectedComponents(graph)));
    }

    [TestMethod]
    public void VisitEvents_InVisit()
    {
        TestVisitingEventsForMethod(
            (graph, visitor) => MoreLinq.MoreEnumerable.Consume(visitor.Visit(graph, 0)));
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

        // The order of events is correct
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

