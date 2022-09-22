using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Stack;

namespace MoreStructures.Tests.Stack;

public abstract class StackTests
{
    protected Func<IStack<int>> Builder { get; }

    protected StackTests(Func<IStack<int>> builder)
    {
        Builder = builder;
    }

    [TestMethod]
    public void Ctor_CreatesAnEmptyStack()
    {
        var stack = Builder();
        Assert.AreEqual(0, stack.Count);
    }

    [TestMethod]
    public void Peek_RaisesExceptionOnEmptyStack()
    {
        var stack = Builder();
        Assert.ThrowsException<InvalidOperationException>(() => stack.Peek());
    }

    [TestMethod]
    public void Peek_ReturnsTheLastPushedItem()
    {
        var stack = Builder();
        stack.Push(4);
        Assert.AreEqual(4, stack.Peek());
        stack.Push(3);
        Assert.AreEqual(3, stack.Peek());
    }

    [TestMethod]
    public void Peek_DoesntAlterTheStack()
    {
        var stack = Builder();
        stack.Push(2);
        stack.Push(3);
        stack.Push(1);
        Assert.AreEqual(3, stack.Count);
        stack.Peek();
        Assert.AreEqual(3, stack.Count);
    }

    [TestMethod]
    public void Pop_RaisesExceptionOnEmptyStack()
    {
        var stack = Builder();
        Assert.ThrowsException<InvalidOperationException>(() => stack.Pop());
        stack.Push(12);
        stack.Pop();
        Assert.ThrowsException<InvalidOperationException>(() => stack.Pop());
    }

    [TestMethod]
    public void PushAndPop_AreCorrect()
    {
        var numberOfNumbers = 5;
        var numbers = Enumerable.Range(0, numberOfNumbers).ToList();
        foreach (var permutation in TestUtilities.GeneratePermutations(numbers))
        {
            var stack = Builder();
            foreach (var number in permutation)
                stack.Push(number);

            var poppedItems = new List<int>();
            for (var i = 0; i < numberOfNumbers; i++)
                poppedItems.Add(stack.Pop());

            Assert.IsTrue(poppedItems.Reverse<int>().SequenceEqual(permutation));
        }
    }
}
