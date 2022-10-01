using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreLinq.Extensions;
using MoreStructures.Graphs;
using MoreStructures.Graphs.MinimumSpanningTree;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.MinimumSpanningTree;

public abstract class MstFinderTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected Func<IMstFinder> FinderBuilder { get; }

    protected MstFinderTests(Func<int, IList<(int, int)>, IGraph> graphBuilder, Func<IMstFinder> builder)
    {
        GraphBuilder = graphBuilder;
        FinderBuilder = builder;
    }

    [DataRow("Empty", 0,
            new int[] { },
            new int[] { },
            new int[] { },
            0)]
    [DataRow("1V, 1-L", 1,
            new int[] { 0 },
            new int[] { 0 },
            new int[] { 1 },
            0)]
    [DataRow("2 V, 2-L, source to sink", 2,
            new[] { 0, 0, 1 },
            new[] { 0, 1, 1 },
            new[] { 0, 4, 2 },
            4)]
    [DataRow("3 V, source to 2-chain and 1-chain sharing leaf 1", 3,
            new[] { 0, 1, 0 },
            new[] { 1, 2, 2 },
            new[] { 1, 3, 1 },
            2)]
    [DataRow("3 V, source to 2-chain and 1-chain sharing leaf 2", 3,
            new[] { 0, 1, 0 },
            new[] { 1, 2, 2 },
            new[] { 2, 3, 2 },
            4)]
    [DataRow("3 V, source to 2-chain and 1-chain sharing leaf 3", 3,
            new[] { 0, 1, 0 },
            new[] { 1, 2, 2 },
            new[] { 4, -2, 3 },
            1)]
    [DataRow("3 V, source to 2-chain and 1-chain sharing leaf back to source with negative cycle", 3,
            new[] { 0, 1, 0, 2 },
            new[] { 1, 2, 2, 0 },
            new[] { 1, 1, 3, -4 },
            -3)]
    [DataRow("4 V, source to 2 vertices of 3-C, which themselves form a 2-C 1", 4,
            new[] { 0, 1, 0, 2, 2, 3 },
            new[] { 1, 2, 2, 1, 3, 1 },
            new[] { 0, 2, 3, 1, 4, -5 },
            -4)]
    [DataRow("4 V, source to 2 vertices of 3-C, which themselves form a 2-C 2", 4,
            new[] { 0, 1, 0, 2, 2, 3 },
            new[] { 1, 2, 2, 1, 3, 1 },
            new[] { 0, 2, 3, 1, 4, 3 },
            4)]
    [DataRow("4 V, source to 2 2-C, one of which goint to sink", 4,
            new[] { 0, 1, 0, 2, 2 },
            new[] { 1, 2, 2, 1, 3 },
            new[] { 0, 2, 3, 1, 4 },
            5)]
    [DataTestMethod]
    public void Find_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances,
        int expectedMstWeight)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var finder = FinderBuilder();

        var distancesDict = starts.Zip(ends).Zip(distances).ToDictionary(t => t.First, t => t.Second);
        var graphDistances = new DictionaryAdapterGraphDistances(distancesDict);
        var mst = finder.Find(graph, graphDistances);

        // If numberOfVertices == 0 => 0 edges in MST, 0 distinct vertices in MST, 0 connected components
        if (numberOfVertices > 1)
        {
            Assert.AreEqual(numberOfVertices - 1, mst.Count);
            Assert.AreEqual(numberOfVertices,
                mst.Select(e => e.Item1).Concat(mst.Select(e => e.Item2)).Distinct().Count());
        }

        var mstGraph = GraphBuilder(numberOfVertices, mst.ToList());
        var visitor = new FullyIterativeHashSetBasedGraphVisit(false);
        Assert.AreEqual(Math.Min(1, numberOfVertices),
            visitor.ConnectedComponents(mstGraph).Values.Distinct().Count());

        Assert.AreEqual(expectedMstWeight, mst.Sum(edge => distancesDict[edge]), graphDescription);
    }

    [DataRow("2V isolated, no edges", 2,
        new int[] { },
        new int[] { },
        new int[] { })]
    [DataRow("2V isolated, 2-L", 2,
        new[] { 0, 1 },
        new[] { 0, 1 },
        new[] { 1, 2 })]
    [DataRow("3V, 1 2-C and 1 isolated", 3,
        new[] { 0, 1 },
        new[] { 1, 0 },
        new[] { 1, 2 })]
    [DataRow("12 V, source to chains merging to intermediate, to chains merging to intermediate to leaf 1, 1 isolated", 
        12,
        new[] { 0, 0, 0, 0, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        new[] { 1, 2, 3, 5, 8, 9, 1, 4, 1, 6, 7, 1, 10, 8 },
        new[] { 9, 1, 1, 1, 1, 0, 7, 1, 5, 1, 1, 3, 0, 0 })]
    [TestMethod]
    public void Find_ThrowsExceptionIfTheGraphIsNotConnected(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var finder = FinderBuilder();

        var distancesDict = starts.Zip(ends).Zip(distances).ToDictionary(t => t.First, t => t.Second);
        var graphDistances = new DictionaryAdapterGraphDistances(distancesDict);

        Assert.ThrowsException<InvalidOperationException>(() => finder.Find(graph, graphDistances), graphDescription);
    }
}
