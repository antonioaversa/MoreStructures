using MoreStructures.Lists.Searching;

namespace MoreStructures.Tests.Lists.Searching;

[TestClass]
public class LinearSearchTests : SearchTests
{
    public LinearSearchTests() : base(new LinearSearch())
    {
    }
}
