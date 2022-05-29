using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;

namespace MoreStructures.Tests.BurrowsWheelerTransform;

[TestClass]
public class BWTransformTests
{
    [TestMethod]
    public void Length_IsTheSameOfLengthOfText()
    {
        var transform = new BWTransform(new("test"), new("ttes$"));
        Assert.AreEqual(transform.Text.Length, transform.Length);
    }
}
