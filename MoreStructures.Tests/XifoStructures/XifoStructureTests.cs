using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.XifoStructures;

namespace MoreStructures.Tests.XifoStructures;

public abstract class XifoStructureTests
{
    protected Func<IXifoStructure<int>> XifoBuilder { get; }

    protected XifoStructureTests(Func<IXifoStructure<int>> xifoBuilder)
    {
        XifoBuilder = xifoBuilder;
    }

    [TestMethod]
    public void Count_IsCorrect()
    {
        var xifo = XifoBuilder();
        Assert.AreEqual(0, xifo.Count);
        xifo.Push(0);
        Assert.AreEqual(1, xifo.Count);
        xifo.Push(1);
        xifo.Push(0);
        Assert.AreEqual(3, xifo.Count);
        xifo.Pop();
        Assert.AreEqual(2, xifo.Count);
        xifo.Pop();
        xifo.Pop();
        Assert.AreEqual(0, xifo.Count);
    }

    [TestMethod]
    public void Pop_ThrowsExceptionOnEmptyXifo()
    {
        var xifo = XifoBuilder();
        Assert.ThrowsException<InvalidOperationException>(() => xifo.Pop());
    }

    [TestMethod]
    public void PushAndPop_AreCorrect()
    {
        var xifo = XifoBuilder();
        xifo.Push(3);
        Assert.AreEqual(3, xifo.Pop());
        xifo.Push(5);
        xifo.Push(7);
        var first = xifo.Pop();
        var second = xifo.Pop();
        Assert.IsTrue(new HashSet<int> { first, second }.SetEquals(new HashSet<int> { 5, 7 }));
    }
}
