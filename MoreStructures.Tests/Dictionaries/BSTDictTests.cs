namespace MoreStructures.Tests.Dictionaries;

[TestClass]
public class BSTDictTests : DictTests
{
    public BSTDictTests() : base(() => new BSTDict<int, int>())
    {
    }
}