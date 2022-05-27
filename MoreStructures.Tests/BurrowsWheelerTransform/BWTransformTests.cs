using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;

namespace MoreStructures.Tests.BurrowsWheelerTransform;

[TestClass]
public class BWTransformTests
{
    [TestMethod]
    public void ToString_IncludesTheContent()
    {
        var transform = new BWTransform(new("ab"), "ttes$");
        Assert.IsTrue(transform.ToString().Contains(transform.Content));
    }
}
