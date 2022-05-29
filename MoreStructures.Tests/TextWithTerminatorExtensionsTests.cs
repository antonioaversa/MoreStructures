using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoreStructures.Tests;

[TestClass]
public class TextWithTerminatorExtensionsTests
{
    [TestMethod]
    public void ToVirtuallyRotated()
    {
        var text = new TextWithTerminator("a");
        var vrtext1 = text.ToVirtuallyRotated(1);
        Assert.AreEqual('$', vrtext1[0]);

        var vrtext2 = text.ToVirtuallyRotated(2);
        Assert.AreEqual('a', vrtext2[0]);
    }
}
