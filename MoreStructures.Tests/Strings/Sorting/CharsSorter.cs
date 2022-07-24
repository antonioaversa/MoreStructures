using MoreLinq;
using MoreStructures.Strings.Sorting;

namespace MoreStructures.Tests.Strings.Sorting;

public abstract class CharsSorterTests
{
    protected Func<char?, ICharsSorter> CharsSorterBuilder { get; }

    protected CharsSorterTests(Func<char?, ICharsSorter> charsSorterBuilder)
    {
        CharsSorterBuilder = charsSorterBuilder;
    }

    [DataRow("", null)]
    [DataRow("a", null)]
    [DataRow("aa", null)]
    [DataRow("ab", null)]
    [DataRow("aba", null)]
    [DataRow("abab", null)]
    [DataRow("cabcba", null)]
    [DataRow("dceadfddbecdedaefbcecfghdd", null)]
    [DataTestMethod]
    public void Sort_IsCorrect(string input, char? maybeTerminator)
    {
        var charsSorter = CharsSorterBuilder(maybeTerminator);
        var result = charsSorter.Sort(input);
        var expectedResult = input
            .Index()
            .OrderBy(kvp => kvp.Value)
            .Select(kvp => kvp.Key);
        Assert.IsTrue(expectedResult.SequenceEqual(result));
    }
}
