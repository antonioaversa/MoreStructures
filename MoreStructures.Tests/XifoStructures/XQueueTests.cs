using MoreStructures.XifoStructures;

namespace MoreStructures.Tests.XifoStructures;

[TestClass]
public class XQueueTests : XifoStructureTests
{
    public XQueueTests() : base(() => new XQueue<int>())
    {
    }

    [TestMethod]
    public void PushAndPop_AreInQueueOrder()
    {
        var xifo = XifoBuilder();
        xifo.Push(3);
        xifo.Push(5);
        xifo.Push(7);
        Assert.AreEqual(3, xifo.Pop());
        Assert.AreEqual(5, xifo.Pop());
        Assert.AreEqual(7, xifo.Pop());
    }
}