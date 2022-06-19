using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class StringValueEnumerableTests
{
    [TestMethod]
    public void Equals_ByValueOnItemsOfUnderlyingEnumerable()
    {
        var ve1 = new StringValueEnumerable("abc");
        var ve2 = new StringValueEnumerable("abc");
        Assert.AreEqual(ve1, ve2);

        var ve3 = new StringValueEnumerable("abd");
        Assert.AreNotEqual(ve1, ve3);

        Assert.IsFalse(ve1.Equals(null));
    }
}