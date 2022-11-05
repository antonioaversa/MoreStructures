namespace MoreStructures.Tests.Dictionaries;

public abstract class DictTests
{
    protected Func<IDictionary<int, int>> Builder { get; }

    protected DictTests(Func<IDictionary<int, int>> builder)
    {
        Builder = builder;
    }

    [TestMethod]
    public void Count_IsCorrect()
    {
        var dictionary = Builder();
        dictionary.Count();
    }
}
