using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class LockValueObjectTests
{
    [TestMethod]
    public void Equals_IsByValue()
    {
        var o1 = new LockValueObject();
        var o2 = new LockValueObject();
        Assert.AreEqual(o1, o2);
        Assert.IsTrue(o1 == o2);
        Assert.IsTrue(o1.Equals(o2));
    }

    [TestMethod]
    public void GetHashCode_IsByValue()
    {
        var o1 = new LockValueObject();
        var o2 = new LockValueObject();
        Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());
    }
}
