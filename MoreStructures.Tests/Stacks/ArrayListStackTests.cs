using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Stacks;

namespace MoreStructures.Tests.Stack;

[TestClass]
public class ArrayListStackTests : StackTests
{
    public ArrayListStackTests() : base(() => new ArrayListStack<int>(4))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionOnInvalidParams()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new ArrayListStack<int>(0, 2));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new ArrayListStack<int>(-1, 2));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new ArrayListStack<int>(4, 0.0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new ArrayListStack<int>(4, -2.0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new ArrayListStack<int>(4, 0.9));
    }

    [TestMethod]
    public void Ctor_SetsTheIncreasingFactor()
    {
        var stack = new ArrayListStack<int>(4, 3.0);
        Assert.AreEqual(3.0, stack.IncreasingFactor);
    }

    [TestMethod]
    public void Push_ResizesEnoughEvenWithLowIncreasingFactor()
    {
        var stack = new ArrayListStack<int>(1, 1.1);
        stack.Push(2);
        Assert.AreEqual(1, stack.Count);
        stack.Push(3);
        Assert.AreEqual(2, stack.Count);
    }
}
