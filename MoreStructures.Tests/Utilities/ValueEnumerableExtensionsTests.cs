using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;
using System.Collections.Generic;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class ValueEnumerableExtensionsTests
{
    [TestMethod]
    public void AsValueEnumerable_WrapsProvidedEnumerableIntoAValueEnumerable()
    {
        var e1 = new HashSet<double?> { 0.0, -1.2, 4.5 };
        var ve1 = e1.AsValueEnumerable();
        Assert.AreEqual(new ValueEnumerable<double?>(e1), ve1);
    }
}
