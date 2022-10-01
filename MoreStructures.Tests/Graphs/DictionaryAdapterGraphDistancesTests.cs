using MoreStructures.Graphs;

namespace MoreStructures.Tests.Graphs;

[TestClass]
public class DictionaryAdapterGraphDistancesTests
{
    [TestMethod]
    public void Indexer_TakesDataFromUnderlyingDictionary()
    {
        var dictionary = new Dictionary<(int, int), int>
        {
            [(0, 0)] = 0,
            [(0, 1)] = 1,
            [(0, 2)] = 2,
            [(1, 0)] = 3,
        };
        var graphDistances = new DictionaryAdapterGraphDistances(dictionary);

        foreach (var key in dictionary.Keys)
            Assert.AreEqual(dictionary[key], graphDistances[key]);
    }

    [TestMethod]
    public void Indexer_RaisesExceptionWhenProvidedEdgeIsUnknown()
    {
        var dictionary = new Dictionary<(int, int), int>
        {
            [(0, 0)] = 0,
            [(0, 1)] = 1,
            [(0, 2)] = 2,
            [(1, 0)] = 3,
        };
        var graphDistances = new DictionaryAdapterGraphDistances(dictionary);

        Assert.ThrowsException<KeyNotFoundException>(() => graphDistances[(1, 2)]);
        Assert.ThrowsException<KeyNotFoundException>(() => graphDistances[(2, 0)]);
    }
}
