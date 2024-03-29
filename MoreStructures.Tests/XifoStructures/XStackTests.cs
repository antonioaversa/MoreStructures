﻿using MoreStructures.XifoStructures;

namespace MoreStructures.Tests.XifoStructures;

[TestClass]
public class XStackTests : XifoStructureTests
{
    public XStackTests() : base(() => new XStack<int>())
    {
    }

    [TestMethod]
    public void PushAndPop_AreInStackOrder()
    {
        var xifo = XifoBuilder();
        xifo.Push(3);
        xifo.Push(5);
        xifo.Push(7);
        Assert.AreEqual(7, xifo.Pop());
        Assert.AreEqual(5, xifo.Pop());
        Assert.AreEqual(3, xifo.Pop());
    }
}
