using MoreStructures.Strings.Sorting;

namespace MoreStructures.Tests.Strings.Sorting;

[TestClass]
public class CountingSortCharsSorterTests : CharsSorterTests
{
    public CountingSortCharsSorterTests() 
        : base(maybeTerminator => new CountingSortCharsSorter(
            Enumerable.Range(0, 26).ToDictionary(i => (char)('a' + i), i => i)))
    {
    }
}
