using MoreLinq;
using MoreStructures.Strings.Sorting;

namespace MoreStructures.Tests.Strings.Sorting;

public abstract class CharsSorterTests
{
    protected ICharsSorter CharsSorter { get; }

    protected CharsSorterTests(ICharsSorter charsSorter)
    {
        CharsSorter = charsSorter;
    }

    [DataRow("")]
    [DataRow("a")]
    [DataRow("aa")]
    [DataRow("ab")]
    [DataRow("aba")]
    [DataRow("abab")]
    [DataRow("cabcba")]
    [DataRow("dceadfddbecdedaefbcecfghdd")]
    [DataTestMethod]
    public void Sort_IsCorrect(string input)
    {
        var result = CharsSorter.Sort(input);
        var expectedResult = input
            .Index()
            .OrderBy(kvp => kvp.Value)
            .Select(kvp => kvp.Key);
        Assert.IsTrue(expectedResult.SequenceEqual(result));
    }
}
