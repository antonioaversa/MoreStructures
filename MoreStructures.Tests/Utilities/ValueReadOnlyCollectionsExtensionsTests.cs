using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class ValueReadOnlyCollectionsExtensionsTests
{
    [TestMethod]
    public void ToValueReadOnlyCollection_IsCorrect()
    {
        var list = new List<string>() { "a", "b" };
        var valueList1 = new ValueReadOnlyCollection<string>(list);
        var valueList2 = list.ToValueReadOnlyCollection();
        Assert.AreEqual(valueList1, valueList2);
    }

    [TestMethod]
    public void ToValueReadOnlyDictionary_IsCorrectWithDictionary()
    {
        var dictionary = new Dictionary<string, int>() { ["a"] = 1, ["b"] = 2 };
        var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(dictionary);
        var valueDictionary2 = dictionary.ToValueReadOnlyDictionary();
        Assert.AreEqual(valueDictionary1, valueDictionary2);
    }

    [TestMethod]
    public void ToValueReadOnlyDictionary_IsCorrectWithEnumerable()
    {
        var enumerable = new List<KeyValuePair<string, int>> { new("a", 1), new("b", 2) };
        var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(enumerable);
        var valueDictionary2 = enumerable.ToValueReadOnlyDictionary();
        Assert.AreEqual(valueDictionary1, valueDictionary2);
    }
}
