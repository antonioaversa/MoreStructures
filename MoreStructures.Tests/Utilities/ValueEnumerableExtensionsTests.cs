using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class ValueEnumerableExtensionsTests
{
    [TestMethod]
    public void AsValue_WrapsProvidedEnumerableIntoAValueEnumerable()
    {
        var e1 = new HashSet<double?> { 0.0, -1.2, 4.5 };
        var ve1 = e1.AsValue();
        Assert.AreEqual(new ValueEnumerable<double?>(e1), ve1);
    }
}
