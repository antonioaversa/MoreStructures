using MoreStructures.EdgeListGraphs;
using MoreStructures.EdgeListGraphs.Visitor;

namespace MoreStructures.Tests.EdgeListGraphs.Visitor;

public abstract class VisitStrategyTests
{
    protected Func<bool, IVisitStrategy> VisitorBuilder { get; }

    protected VisitStrategyTests(Func<bool, IVisitStrategy> visitorBuilder)
    {
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
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 3, new int[] { 0, 1, 2, 3 }, 
        new int[] { 1, 3, 1, 2 }, 0, 
        new int[] { 0, 1, 3, 2 }, new int[] { 0, 1, 3, 2 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 3, new int[] { 0, 1, 2, 3 },
        new int[] { 1, 3, 1, 2 }, 1,
        new int[] { 1, 3, 2 }, new int[] { 1, 0, 2, 3 })]
    [DataRow("4 V, 2 pointing to 2-chain jumping back", 3, new int[] { 0, 1, 2, 3 },
        new int[] { 1, 3, 1, 2 }, 2,
        new int[] { 2, 1, 3 }, new int[] { 2, 1, 0, 3 })]
    [DataTestMethod]
    public void Explore_IsCorrect(
        string graphDescription,int numberOfVertices, int[] starts, int[] ends, int start,
        int[] expectedDirectedGraphResult, int[] expectedUndirectedGraphResult)
    {
        var graph = new EdgeListGraph(numberOfVertices, starts.Zip(ends).ToList());
        var directedGraphVisitor = VisitorBuilder(true);
        var directedGraphResult = directedGraphVisitor.Visit(graph, start);
        Assert.IsTrue(
            directedGraphResult.ToHashSet().SetEquals(expectedDirectedGraphResult.ToHashSet()), 
            $"Failed visit of {graphDescription} as directed graph: " +
            $"expected [{string.Join(", ", expectedDirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", directedGraphResult)}]");

        var undirectedGraphVisitor = VisitorBuilder(false);
        var undirectedGraphResult = undirectedGraphVisitor.Visit(graph, start);
        Assert.IsTrue(
            undirectedGraphResult.ToHashSet().SetEquals(expectedUndirectedGraphResult.ToHashSet()),
            $"Failed visit of {graphDescription} as undirected graph: " +
            $"expected [{string.Join(", ", expectedUndirectedGraphResult)}], " +
            $"actual: [{string.Join(", ", undirectedGraphResult)}]");
    }
}

