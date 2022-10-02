using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSortp;

[TestClass]
public class IdentityShuffleStrategyTests : ShuffleStrategyTests
{
    public IdentityShuffleStrategyTests() : base(new IdentityShuffleStrategy())
    {
    }
}
