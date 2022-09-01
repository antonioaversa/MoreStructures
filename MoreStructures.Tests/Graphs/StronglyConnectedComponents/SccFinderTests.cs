using MoreStructures.Graphs;
using MoreStructures.Graphs.StronglyConnectedComponents;

namespace MoreStructures.Tests.Graphs.StronglyConnectedComponents;

public abstract class SccFinderTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected Func<ISccFinder> FinderBuilder { get; }

    protected SccFinderTests(
        Func<int, IList<(int, int)>, IGraph> graphBuilder,
        Func<ISccFinder> finderBuilder)
    {
        GraphBuilder = graphBuilder;
        FinderBuilder = finderBuilder;
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
    [DataRow("2 V, source to sink", 2, new int[] { 0 }, new int[] { 1 },
        new int[] { 0, 1 })]
    [DataRow("4 V, source to 3-C", 4, new int[] { 0, 0, 0, 1, 2, 3  }, new int[] { 1, 2, 3, 2, 3, 1 },
        new int[] { 0, 1, 1, 1 })]
    [DataTestMethod]
    public void Find_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends,
        int[] expectedScc)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var sccFinder = FinderBuilder();
        var scc = sccFinder.Find(graph);
        Assert.IsTrue(
            AreEquivalent(expectedScc, scc),
            $"{graphDescription} - Expected [{string.Join(", ", expectedScc)}], Actual: [{string.Join(", ", scc)}]");
    }

    private static bool AreEquivalent(IEnumerable<int> enumerable1, IEnumerable<int> enumerable2)
    {
        var mappings = new Dictionary<int, int>();
        using var iterator1 = enumerable1.GetEnumerator(); 
        using var iterator2 = enumerable2.GetEnumerator();

        while (iterator1.MoveNext())
        {
            if (!iterator2.MoveNext())
                return false;

            if (!mappings.ContainsKey(iterator1.Current))
                mappings[iterator1.Current] = iterator2.Current;
            else if (mappings[iterator1.Current] != iterator2.Current)
                return false;
        }

        if (iterator2.MoveNext())
            return false;

        return true;

    }
}
